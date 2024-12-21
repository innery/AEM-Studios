using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Michsky.MUIP;

public class CharacterSelectionManager : MonoBehaviour
{
    // Karakter se�im butonlar�
    public ButtonManager character1Button;
    public ButtonManager character2Button;
    public ButtonManager character3Button;

    // Iconlar�n referanslar�
    public Image character1Icon;
    public Image character2Icon;
    public Image character3Icon;

    // Start, Options, Exit butonlar�
    public ButtonManager startButton;
    public ButtonManager optionsButton;
    public ButtonManager exitButton;

    // Se�ili karakter ID'si
    private int selectedCharacter = 0;

    // Renkler
    private Color defaultColor = Color.gray;
    private Color selectedColor = Color.green;

   public void Start()
    {
        // Karakter se�im butonlar� olaylar�n� ba�la
        character1Button.onClick.AddListener(() => SelectCharacter(1));
        character2Button.onClick.AddListener(() => SelectCharacter(2));
        character3Button.onClick.AddListener(() => SelectCharacter(3));

        // Start, Options, Exit butonlar� olaylar�n� ba�la
        startButton.onClick.AddListener(OnStartGame);
        optionsButton.onClick.AddListener(OnOptions);
        exitButton.onClick.AddListener(OnExit);

        // T�m iconlar�n rengini varsay�lan renge d�nd�r
        ResetIconColors();
    }

    public void SelectCharacter(int characterID)
    {
        selectedCharacter = characterID;

        // T�m iconlar�n rengini s�f�rla
        ResetIconColors();

        // Se�ilen karakterin iconunu ye�il yap
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

        Debug.Log($"Se�ilen karakter: {selectedCharacter}");
    }

    public void ResetIconColors()
    {
        // T�m iconlar� varsay�lan renge d�nd�r
        character1Icon.color = defaultColor;
        character2Icon.color = defaultColor;
        character3Icon.color = defaultColor;
    }

    public void OnStartGame()
    {
        if (selectedCharacter == 0)
        {
            Debug.LogWarning("L�tfen bir karakter se�in!");
            return;
        }

        // Se�ilen karaktere g�re sahne y�kle
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
        Debug.Log("Ayarlar men�s� a��ld�.");
        // Ayarlar men�s�n� buraya ekleyebilirsin
    }

    public void OnExit()
    {
        Debug.Log("Oyun kapat�ld�.");
        Application.Quit();
    }
}
