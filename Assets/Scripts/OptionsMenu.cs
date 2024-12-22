using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli
using UnityEngine.UI; // UI bileþenlerine eriþim için gerekli
using Michsky.MUIP;

public class OptionsMenu : MonoBehaviour
{
    public ButtonManager returnButton; // Ana menüye dönecek buton
    public ButtonManager exitButton;   // Oyundan çýkacak buton
    public Slider volumeSlider; // Ses kontrol slider'ý

    void Start()
    {
        // Butonlarýn eventlerini ayarla
        returnButton.onClick.AddListener(ReturnToMainMenu);
        exitButton.onClick.AddListener(ExitGame);

        // Slider'ýn eventini ayarla
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Varsayýlan ses seviyesini slider'a ayarla
        volumeSlider.value = AudioListener.volume;
    }

    public void ReturnToMainMenu()
    {
        // Ana menü sahnesine dön
        SceneManager.LoadScene("MENU"); 
        Debug.Log("Returning to Main Menu");
    }

    public void ExitGame()
    {
        // Oyundan çýk
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
