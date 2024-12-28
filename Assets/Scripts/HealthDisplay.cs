using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TMP_Text healthText; // TextMeshPro ��esi referans�
    public Character playerCharacter; // Oyuncu karakterine referans

    public void UpdateHealthUI()
    {
        if (playerCharacter != null)
        {
            // Sa�l�k metnini g�ncelle
            healthText.text = "Health: " + Mathf.Max(playerCharacter._health, 0).ToString("F0");
        }
    }
}
