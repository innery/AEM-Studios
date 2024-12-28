using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseSceneManager : MonoBehaviour
{
    public Character character;

    void Start()
    {
        // Bu scriptin ba�l� oldu�u GameObject'teki Character scriptine referans al
        character = GetComponent<Character>();

        if (character == null)
        {
            Debug.LogError("Character scripti bulunamad�! Bu script ana karakterin GameObject'ine eklenmelidir.");
        }
    }

    void Update()
    {
        // E�er ana karakterin can� s�f�rsa Lose sahnesine ge�
        if (character != null && character._health <= 10)
        {
            SceneManager.LoadScene("Lose"); // "Lose" sahnesinin ad�n� do�ru yazd���n�zdan emin olun
        }
    }
}
