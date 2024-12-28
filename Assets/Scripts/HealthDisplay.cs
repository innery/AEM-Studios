using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    public Character targetCharacter; // Saðlýðý gösterilecek karakter
    private TextMeshProUGUI healthText; // Saðlýk metni

    void Start()
    {
        // TextMeshPro bileþenini bul
        healthText = GetComponent<TextMeshProUGUI>();

        if (healthText == null)
        {
            Debug.LogError("TextMeshPro bileþeni bulunamadý! Lütfen bu scripti bir TextMeshPro GameObject'ine eklediðinizden emin olun.");
        }

        if (targetCharacter == null)
        {
            Debug.LogError("Hedef karakter atanmadý! Inspector'dan bir hedef atayýn.");
        }
    }

    void Update()
    {
        if (targetCharacter != null && healthText != null)
        {
            // Karakterin saðlýk deðerini güncelle
            healthText.text = $"Health: {targetCharacter._health}";
        }
    }
}
