using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Sa�l�k bar� i�in slider
    public Character character; // Sa�l�k bilgisini alaca��m�z Character script'i

    private void Update()
    {
        if (character != null && healthSlider != null)
        {
            healthSlider.value = character.GetHealth(); // Character script'inden _health de�erini �ek
        }
    }
}