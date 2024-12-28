using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Oyun kapat�l�yor...");
        Application.Quit(); // Oyunu kapat�r

        // Bu sat�r sadece Unity Editor i�in �al���r, final build'de �al��maz
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
