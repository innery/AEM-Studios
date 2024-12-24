using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder = null;

    private Weapon _weapon = null; 
    public Weapon weapon { get { return _weapon; } }

    private Ammo _ammo = null; 
    public Ammo ammo { get { return _ammo; } }

    private List<Item> _items = new List<Item>();
    private Animator _animator = null;
    private RigManager _rigManager = null;
    private Weapon _weaponToEquip = null;
    private bool _reloading = false; public bool reloading { get { return _reloading; } }
    private bool _switchingWeapon = false; public bool switchingWeapon { get { return _switchingWeapon; } }

    private void Awake()
    {
        _rigManager = GetComponent<RigManager>();
        _animator = GetComponent<Animator>();

        if (_rigManager == null)
        {
            Debug.LogError("RigManager component missing on the GameObject.");
        }

        // Örnek baþlangýç envanteri
        Initialize(new Dictionary<string, int> { { "Scar", 1 }, { "AKM", 1 }, { "7.62x39mm", 1000 } });
    }

    public void Initialize(Dictionary<string, int> items)
    {
        if (items != null && PrefabManager.singleton != null)
        {
            foreach (var itemData in items)
            {
                Item prefab = PrefabManager.singleton.GetItemPrefab(itemData.Key);
                if (prefab != null && itemData.Value > 0)
                {
                    for (int i = 1; i <= itemData.Value; i++)
                    {
                        Item item = Instantiate(prefab, transform);

                        if (item is Weapon weapon)
                        {
                            item.transform.SetParent(_weaponHolder);
                            item.transform.localPosition = weapon.rightHandPosition;
                            item.transform.localEulerAngles = weapon.rightHandRotation;
                        }
                        else if (item is Ammo ammo)
                        {
                            ammo.amount = itemData.Value;
                        }

                        item.gameObject.SetActive(false);
                        _items.Add(item);
                    }
                }
            }

            // Ýlk silahý otomatik olarak kuþan
            EquipFirstWeapon();
        }
    }

    private void EquipFirstWeapon()
    {
        foreach (var item in _items)
        {
            if (item is Weapon firstWeapon)
            {
                _weaponToEquip = firstWeapon;
                OnEquip();
                break;
            }
        }
    }

    public void ChangeWeapon(float direction)
    {
        int x = direction > 0 ? 1 : direction < 0 ? -1 : 0;
        if (x != 0 && !_switchingWeapon && _weapon != null)
        {
            int currentIndex = -1;
            List<int> weaponIndexes = new List<int>();

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i] is Weapon)
                {
                    weaponIndexes.Add(i);
                    if (_weapon == _items[i])
                    {
                        currentIndex = weaponIndexes.Count - 1;
                    }
                }
            }

            if (currentIndex == -1 || weaponIndexes.Count == 0) return;

            int targetIndex = (currentIndex + x + weaponIndexes.Count) % weaponIndexes.Count;
            EquipWeapon((Weapon)_items[weaponIndexes[targetIndex]]);
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (_switchingWeapon || weapon == null) return;

        _weaponToEquip = weapon;

        if (_weapon != null)
        {
            HolsterWeapon();
        }
        else
        {
            _switchingWeapon = true;
            _animator.SetTrigger("Equip");
        }
    }

    private void _EquipWeapon()
    {
        if (_weaponToEquip != null)
        {
            _weapon = _weaponToEquip;
            _weaponToEquip = null;

            if (_weapon.transform.parent != _weaponHolder)
            {
                _weapon.transform.SetParent(_weaponHolder);
                _weapon.transform.localPosition = _weapon.rightHandPosition;
                _weapon.transform.localEulerAngles = _weapon.rightHandRotation;
            }

            _rigManager.SetLeftHandGripData(_weapon.leftHandPosition, _weapon.leftHandRotation);
            _weapon.gameObject.SetActive(true);

            _ammo = null;
            foreach (var item in _items)
            {
                if (item is Ammo ammo && _weapon.ammoID == ammo.id)
                {
                    _ammo = ammo;
                    break;
                }
            }
        }
    }

    public void OnEquip()
    {
        _EquipWeapon();
        _switchingWeapon = false;
    }

    private void _HolsterWeapon()
    {
        if (_weapon != null)
        {
            _weapon.gameObject.SetActive(false);
            _weapon = null;
            _ammo = null;
        }
    }

    public void HolsterWeapon()
    {
        if (_switchingWeapon) return;

        if (_weapon != null)
        {
            _switchingWeapon = true;
            _animator.SetTrigger("Holster");
        }
    }

    public void OnHolster()
    {
        _HolsterWeapon();

        if (_weaponToEquip != null)
        {
            OnEquip();
        }

        _switchingWeapon = false;
    }

    public void Reload()
    {
        if (_weapon != null && !_reloading && _weapon.ammo < _weapon.clipSize && _ammo != null && _ammo.amount > 0)
        {
            _animator.SetTrigger("Reload");
            _reloading = true;
        }
    }

    public void ReloadFinished()
    {
        if (_weapon != null && _ammo != null)
        {
            int amount = Mathf.Min(_weapon.clipSize - _weapon.ammo, _ammo.amount);
            _weapon.ammo += amount;
            _ammo.amount -= amount;
        }

        _reloading = false;
    }

    public void EquipFinished()
    {
        _switchingWeapon = false; // Silah deðiþtirme iþlemi tamamlandý
    }

    public void HolsterFinished()
    {
        _switchingWeapon = false; // Silah kýlýfa koyma iþlemi tamamlandý
    }

    public void ApplyDamage(Character shooter, Transform hit, float damage)
    {
        // Hasar verme mantýðý buraya eklenebilir
    }
}
