using UnityEngine;
using UnityEngine.SceneManagement; // Sahne y�netimi i�in gerekli
using UnityEngine.UI; // UI bile�enlerine eri�im i�in gerekli
using Michsky.MUIP;

public class OptionsMenu : MonoBehaviour
{
    public ButtonManager returnButton; // Ana men�ye d�necek buton
    public ButtonManager exitButton;   // Oyundan ��kacak buton
    public Slider volumeSlider; // Ses kontrol slider'�

    void Start()
    {
        // Butonlar�n eventlerini ayarla
        returnButton.onClick.AddListener(ReturnToMainMenu);
        exitButton.onClick.AddListener(ExitGame);

        // Slider'�n eventini ayarla
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Varsay�lan ses seviyesini slider'a ayarla
        volumeSlider.value = AudioListener.volume;
    }

    public void ReturnToMainMenu()
    {
        // Ana men� sahnesine d�n
        SceneManager.LoadScene("MENU"); 
        Debug.Log("Returning to Main Menu");
    }

    public void ExitGame()
    {
        // Oyundan ��k
        Application.Quit();
        Debug.Log("Exiting game...");
    }

    public void SetVolume(float volume)
    {
        // Ses seviyesini ayarla
        AudioListener.volume = volume;
        Debug.Log("Volume set to: " + volume);
    }
}
