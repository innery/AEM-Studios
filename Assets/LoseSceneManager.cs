using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseSceneManager : MonoBehaviour
{
    public Character character;

    void Start()
    {
        // Bu scriptin baðlý olduðu GameObject'teki Character scriptine referans al
        character = GetComponent<Character>();

        if (character == null)
        {
            Debug.LogError("Character scripti bulunamadý! Bu script ana karakterin GameObject'ine eklenmelidir.");
        }
    }

    void Update()
    {
        // Eðer ana karakterin caný sýfýrsa Lose sahnesine geç
        if (character != null && character._health <= 10)
        {
            SceneManager.LoadScene("Lose"); // "Lose" sahnesinin adýný doðru yazdýðýnýzdan emin olun
        }
    }
}
