using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoText; // TextMeshPro UI bile�eni
    public Character character;      // Karakter scripti

    private void Update()
    {
        if (character != null && character.weapon != null)
        {
            // Weapon scriptinden mermi de�erlerini al
            int currentAmmo = character.weapon.ammo;
            int clipSize = character.weapon._clipSize;

            // TextMeshPro metnini g�ncelle
            ammoText.text = $"{currentAmmo}/{clipSize}";
        }
        else
        {
            // E�er silah yoksa ya da atanmad�ysa
            ammoText.text = "No Weapon";
        }
    }
}
