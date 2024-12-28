using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public Character targetCharacter; // Sa�l��� g�sterilecek karakter
    private TextMeshProUGUI healthText; // Sa�l�k metni

    void Start()
    {
        // TextMeshPro bile�enini bul
        healthText = GetComponent<TextMeshProUGUI>();

        if (healthText == null)
        {
            Debug.LogError("TextMeshPro bile�eni bulunamad�! L�tfen bu scripti bir TextMeshPro GameObject'ine ekledi�inizden emin olun.");
        }

        if (targetCharacter == null)
        {
            Debug.LogError("Hedef karakter atanmad�! Inspector'dan bir hedef atay�n.");
        }
    }

    void Update()
    {
        if (targetCharacter != null && healthText != null)
        {
            // Karakterin sa�l�k de�erini g�ncelle
            healthText.text = $"Health: {targetCharacter._health}";
        }
    }
}
