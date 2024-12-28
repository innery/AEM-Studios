using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoText; // TextMeshPro UI bileþeni
    public Character character;      // Karakter scripti

    private void Update()
    {
        if (character != null && character.weapon != null)
        {
            // Weapon scriptinden mermi deðerlerini al
            int currentAmmo = character.weapon.ammo;
            int clipSize = character.weapon._clipSize;

            // TextMeshPro metnini güncelle
            ammoText.text = $"{currentAmmo}/{clipSize}";
        }
        else
        {
            // Eðer silah yoksa ya da atanmadýysa
            ammoText.text = "No Weapon";
        }
    }
}
