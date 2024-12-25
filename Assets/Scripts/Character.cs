using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isLocalPlayer = false;

    [SerializeField] private string _id = "";
    public string id { get { return _id; } }

    [SerializeField] private Transform _weaponHolder = null;
    private Weapon _weapon = null;
    public Weapon weapon { get { return _weapon; } }

    private Ammo _ammo = null;
    public Ammo ammo { get { return _ammo; } }

    private List<Item> _items = new List<Item>();
    private Animator _animator = null;
    private RigManager _rigManager = null;
    private Weapon _weaponToEquip = null;
    private bool _reloading = false;
    public bool reloading { get { return _reloading; } }
    private bool _switchingWeapon = false;
    public bool switchingWeapon { get { return _switchingWeapon; } }

    private Rigidbody[] _ragdollRigidbodies = null;
    private Collider[] _ragdollColliders = null;

    private float _health = 100;

    private bool _grounded = false; public bool isGrounded { get { return _grounded; } set { _grounded = value; } }
    private bool _walking = false; public bool walking { get { return _walking; } set { _walking = value; } }
    private float _speedAnimationMultiplier = 0; public float speedAnimationMultiplier { get { return _speedAnimationMultiplier; } }
    private bool _aiming = false; public bool aiming { get { return _aiming; } set { _aiming = value; } }
    private bool _sprinting = false; public bool sprinting { get { return _sprinting; } set { _sprinting = value; } }
    private float _aimLayerWieght = 0;
    private Vector2 _aimedMovingAnimationsInput = Vector2.zero;
    private float aimRigWieght = 0;
    private float leftHandWieght = 0;
    private Vector3 _aimTarget = Vector3.zero; public Vector3 aimTarget { get { return _aimTarget; } set { _aimTarget = value; } }
    private Vector3 _lastPosition = Vector3.zero;

    private void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _ragdollColliders = GetComponentsInChildren<Collider>();

        if (_ragdollRigidbodies != null)
        {
            for (int i = 0; i < _ragdollRigidbodies.Length; i++)
            {
                _ragdollRigidbodies[i].mass *= 50;
            }
        }

        if (_ragdollColliders != null)
        {
            for (int i = 0; i < _ragdollColliders.Length; i++)
            {
                _ragdollColliders[i].isTrigger = false;
            }
        }

        SetRagdollStatus(false);
        _rigManager = GetComponent<RigManager>();
        _animator = GetComponent<Animator>();
        Initialize(new Dictionary<string, int> { { "Scar", 1 }, { "AKM", 1 }, { "7.62x39mm", 1000 } });
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            SetLayer(transform, LayerMask.NameToLayer("LocalPlayer"));
        }
        else
        {
            SetLayer(transform, LayerMask.NameToLayer("NetworkPlayer"));
        }
    }
    
    private void Update()
    {
        bool armed = _weapon != null;

        _aimLayerWieght = Mathf.Lerp(_aimLayerWieght, _switchingWeapon || (armed && (_aiming || _reloading)) ? 1f : 0f, 10f * Time.deltaTime);
        _animator.SetLayerWeight(1, _aimLayerWieght);

        aimRigWieght = Mathf.Lerp(aimRigWieght, armed && _aiming && !_reloading ? 1f : 0f, 10f * Time.deltaTime);
        leftHandWieght = Mathf.Lerp(leftHandWieght, armed && _switchingWeapon == false && !_reloading && (_aiming || (_grounded && _weapon.type == Weapon.Handle.TwoHanded)) ? 1f : 0f, 10f * Time.deltaTime);

        _rigManager.aimTarget = _aimTarget;
        _rigManager.aimWeight = aimRigWieght;
        _rigManager.leftHandWeight = leftHandWieght;

        if (_sprinting)
        {
            _speedAnimationMultiplier = 3;
        }
        else if (_walking)
        {
            _speedAnimationMultiplier = 1;
        }
        else
        {
            _speedAnimationMultiplier = 2;
        }

        Vector3 deltaPosition = transform.InverseTransformDirection(transform.position - _lastPosition).normalized;

        _aimedMovingAnimationsInput = Vector2.Lerp(_aimedMovingAnimationsInput, new Vector2(deltaPosition.x, deltaPosition.z) * _speedAnimationMultiplier, 10f * Time.deltaTime);
        _animator.SetFloat("Speed_X", _aimedMovingAnimationsInput.x);
        _animator.SetFloat("Speed_Y", _aimedMovingAnimationsInput.y);
        _animator.SetFloat("Armed", armed ? 1f : 0f);
        _animator.SetFloat("Aimed", _aiming ? 1f : 0f);
    }
    
    private void LateUpdate()
    {
        _lastPosition = transform.position;
    }

    private void SetRagdollStatus(bool enabled)
    {
        if (_ragdollRigidbodies != null)
        {
            for (int i = 0; i < _ragdollRigidbodies.Length; i++)
            {
                _ragdollRigidbodies[i].isKinematic = !enabled;
            }
        }
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

            // Equip the first weapon automatically
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
        if (x != 0 && !_switchingWeapon)
        {
            if (x > 0)
            {
                NextWeapon();  // Switch to next weapon
            }
            else
            {
                PrevWeapon();  // Switch to previous weapon
            }
        }
    }

    private void NextWeapon()
    {
        int current = -1;
        int first = -1;
        // Loop through all items to find the next weapon
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] is Weapon weapon)
            {
                // If we find the currently equipped weapon
                if (_weapon != null && _items[i] == _weapon)
                {
                    current = i;
                }
                else if (current >= 0) // After the currently equipped weapon
                {
                    EquipWeapon(weapon);  // Equip the next weapon
                    return;
                }
                else if (first < 0)  // First weapon in the list
                {
                    first = i;
                }
            }
        }

        // If no weapon was found after the current one, equip the first weapon
        if (current == -1 && first >= 0)
        {
            EquipWeapon((Weapon)_items[first]);
        }
    }

    private void PrevWeapon()
    {
        int current = -1;
        int last = -1;
        // Loop through all items in reverse order to find the previous weapon
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            if (_items[i] is Weapon weapon)
            {
                // If we find the currently equipped weapon
                if (_weapon != null && _items[i] == _weapon)
                {
                    current = i;
                }
                else if (current >= 0) // After the currently equipped weapon
                {
                    EquipWeapon(weapon);  // Equip the previous weapon
                    return;
                }
                else if (last < 0)  // Last weapon in the list
                {
                    last = i;
                }
            }
        }

        // If no weapon was found before the current one, equip the last weapon
        if (current == -1 && last >= 0)
        {
            EquipWeapon((Weapon)_items[last]);
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (_switchingWeapon || weapon == null) return;  // Don't switch if already switching

        _weaponToEquip = weapon;

        if (_weapon != null)
        {
            HolsterWeapon();
        }
        else
        {
            _switchingWeapon = true;
            _animator.SetTrigger("Equip");  // Trigger animation for equipping
        }
    }

    private void _EquipWeapon()
    {
        if (_weaponToEquip != null)
        {
            _weapon = _weaponToEquip;  // Update the equipped weapon
            _weaponToEquip = null;

            if (_weapon.transform.parent != _weaponHolder)
            {
                _weapon.transform.SetParent(_weaponHolder);  // Attach weapon to the holder
                _weapon.transform.localPosition = _weapon.rightHandPosition;
                _weapon.transform.localEulerAngles = _weapon.rightHandRotation;
            }

            _rigManager.SetLeftHandGripData(_weapon.leftHandPosition, _weapon.leftHandRotation);  // Set grip data
            _weapon.gameObject.SetActive(true);  // Activate weapon

            // Find the corresponding ammo
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

    public void HolsterWeapon()
    {
        if (_switchingWeapon) return;  // Don't holster if switching weapons

        if (_weapon != null)
        {
            _switchingWeapon = true;
            _animator.SetTrigger("Holster");  // Trigger animation for holstering
        }
    }

    private void _HolsterWeapon()
    {
        if (_weapon != null)
        {
            _weapon.gameObject.SetActive(false);  // Deactivate the current weapon
            _weapon = null;
            _ammo = null;  // Remove ammo reference
        }
    }

    public void OnEquip()
    {
        _EquipWeapon();
        _switchingWeapon = false;  // Silah deðiþtirme iþlemi tamamlandý
    }

    public void OnHolster()
    {
        _HolsterWeapon();

        if (_weaponToEquip != null)
        {
            OnEquip();
        }

        _switchingWeapon = false; // Silah kýlýfa koyma iþlemi tamamlandý
    }

    public void ApplyDamage(Character shooter, Transform hit, float damage)
    {
        if (_health > 0)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                SetRagdollStatus(true);

                // Handle missing RigBuilder and ThirdPersonController gracefully
                if (_rigManager != null)
                {
                    Destroy(_rigManager);
                }

                Animator animator = GetComponent<Animator>();
                if (animator != null)
                {
                    Destroy(animator);
                }

                CharacterController controller = GetComponent<CharacterController>();
                if (controller != null)
                {
                    Destroy(controller);
                }

                Destroy(this);
            }
        }
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

    private void SetLayer(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
