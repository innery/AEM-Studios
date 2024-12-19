using System.Collections.Generic; // <-- Add this for List<T> and Dictionary<TKey, TValue>
using UnityEngine;

public class Character : MonoBehaviour
{
   [SerializeField] private Transform _weaponHolder = null;

   private Weapon _weapon = null; 
   public Weapon weapon { get { return _weapon; }}
   
   private List<Item> _items = new List<Item>(); // <-- List<Item> now recognized
   private RigManager _rigManager = null;
   private int firstWeaponIndex = 0; // <-- Declare and initialize firstWeaponIndex

   private void Awake()
   {
       _rigManager = GetComponent<RigManager>();
       Initialize(new Dictionary<string, int> { { "AKM", 1 }});

       if (_rigManager == null) // <-- Null check for RigManager
       {
           Debug.LogError("RigManager component missing on the GameObject.");
       }
   }

   public void Initialize(Dictionary<string, int> items) // <-- Dictionary<string, int> now recognized
   {
       if (items != null && PrefabManager.singleton != null) // <-- Fix typo: singleton
       {
           foreach (var itemData in items)
           {
               Item prefab = PrefabManager.singleton.GetItemPrefab(itemData.Key); // <-- Fix typo: singleton
               if(prefab != null && itemData.Value > 0)
               {
                   for(int i = 1; i <= itemData.Value; i++)
                   {
                       bool done = false;
                       Item item = Instantiate(prefab, transform);

                       if(item.GetType() == typeof(Weapon))
                       {
                           Weapon w = (Weapon)item;
                           item.transform.SetParent(_weaponHolder);
                           item.transform.localPosition = w.rightHandPosition;
                           item.transform.localEulerAngles = w.rightHandRotation; // <-- Fixed typo here: 'localEularAngles' to 'localEulerAngles'

                           if (firstWeaponIndex < 0)
                           {
                               firstWeaponIndex = _items.Count;
                           }
                       }
                       else if (item.GetType() == typeof(Ammo))
                       {   
                           Ammo a = (Ammo)item;
                           a.amount = itemData.Value;
                           done = true;
                       }
                       item.gameObject.SetActive(false);
                       _items.Add(item);
                       if (done)
                       {
                           break;
                       }
                   }
               }
           }
           if (firstWeaponIndex >= 0 && _weapon == null && _items.Count > firstWeaponIndex)
           {
               if (_items[firstWeaponIndex] is Weapon weapon) // <-- Check if the item is a Weapon before casting
               {
                   EquipWeapon(weapon);
               }
           }
       }
   }

   public void EquipWeapon(Weapon weapon)
   {
       if (_weapon != null)
       {
           HolsterWeapon();
       }
       if (weapon != null)
       {
           if(weapon.transform.parent != _weaponHolder)
           {
               weapon.transform.SetParent(_weaponHolder);
               weapon.transform.localPosition = weapon.rightHandPosition;
               weapon.transform.localEulerAngles = weapon.rightHandRotation;
           }
           _rigManager.SetLeftHandGripData(weapon.leftHandPosition, weapon.leftHandRotation);
           weapon.gameObject.SetActive(true);
           _weapon = weapon;
       }
   }

   public void HolsterWeapon()
   {
       if(_weapon != null)
       {
           _weapon.gameObject.SetActive(false);
           _weapon = null;
       }
   }

   public void ApplyDamage(Character shooter, Transform hit, float damage)
   {

   }

}
