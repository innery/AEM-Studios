using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Michsky.MUIP;

public class CharacterSelectionManager : MonoBehaviour
{
    // Karakter seçim butonlarý
    public ButtonManager character1Button;
    public ButtonManager character2Button;
    public ButtonManager character3Button;

    // Iconlarýn referanslarý
    public Image character1Icon;
    public Image character2Icon;
    public Image character3Icon;

    // Start, Options, Exit butonlarý
    public ButtonManager startButton;
    public ButtonManager optionsButton;
    public ButtonManager exitButton;

    // Seçili karakter ID'si
    private int selectedCharacter = 0;

    // Renkler
    private Color defaultColor = Color.gray;
    private Color selectedColor = Color.green;

   public void Start()
    {
        // Karakter seçim butonlarý olaylarýný baðla
        character1Button.onClick.AddListener(() => SelectCharacter(1));
        character2Button.onClick.AddListener(() => SelectCharacter(2));
        character3Button.onClick.AddListener(() => SelectCharacter(3));

        // Start, Options, Exit butonlarý olaylarýný baðla
        startButton.onClick.AddListener(OnStartGame);
        optionsButton.onClick.AddListener(OnOptions);
        exitButton.onClick.AddListener(OnExit);

        // Tüm iconlarýn rengini varsayýlan renge döndür
        ResetIconColors();
    }

    public void SelectCharacter(int characterID)
    {
        selectedCharacter = characterID;

        // Tüm iconlarýn rengini sýfýrla
        ResetIconColors();

        // Seçilen karakterin iconunu yeþil yap
        if (characterID == 1)
        {
            character1Icon.color = selectedColor;
        }
        else if (characterID == 2)
        {
            character2Icon.color = selectedColor;
        }
        else if (characterID == 3)
        {
            character3Icon.color = selectedColor;
        }

        Debug.Log($"Seçilen karakter: {selectedCharacter}");
    }

    public void ResetIconColors()
    {
        // Tüm iconlarý varsayýlan renge döndür
        character1Icon.color = defaultColor;
        character2Icon.color = defaultColor;
        character3Icon.color = defaultColor;
    }

    public void OnStartGame()
    {
        if (selectedCharacter == 0)
        {
            Debug.LogWarning("Lütfen bir karakter seçin!");
            return;
        }

        // Seçilen karaktere göre sahne yükle
        if (selectedCharacter == 1)
        {
            SceneManager.LoadScene("1");
        }
        else if (selectedCharacter == 2)
        {
            SceneManager.LoadScene("Character2Scene");
        }
        else if (selectedCharacter == 3)
        {
            SceneManager.LoadScene("Character3Scene");
        }
    }

    public void OnOptions()
    {
        SceneManager.LoadScene("SETTINGS");
        Debug.Log("Ayarlar menüsü açýldý.");
        // Ayarlar menüsünü buraya ekleyebilirsin
    }

    public void OnExit()
    {
        Debug.Log("Oyun kapatýldý.");
        Application.Quit();
    }
}
