using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Oyun kapatýlýyor...");
        Application.Quit(); // Oyunu kapatýr

        // Bu satýr sadece Unity Editor için çalýþýr, final build'de çalýþmaz
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
