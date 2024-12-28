using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TMP_Text healthText; // TextMeshPro öðesi referansý
    public Character playerCharacter; // Oyuncu karakterine referans

    public void UpdateHealthUI()
    {
        if (playerCharacter != null)
        {
            // Saðlýk metnini güncelle
            healthText.text = "Health: " + Mathf.Max(playerCharacter._health, 0).ToString("F0");
        }
    }
}
