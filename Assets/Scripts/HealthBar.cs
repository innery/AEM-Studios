using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Saðlýk barý için slider
    public Character character; // Saðlýk bilgisini alacaðýmýz Character script'i

    private void Update()
    {
        if (character != null && healthSlider != null)
        {
            healthSlider.value = character.GetHealth(); // Character script'inden _health deðerini çek
        }
    }
}