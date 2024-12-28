using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BotManager : MonoBehaviour
{
    public string botTag = "Bot"; // Botlar�n sahip oldu�u tag
    public string winSceneName = "WinScene"; // Kazanma sahnesinin ad�
    public TextMeshProUGUI botCountText; // Bot say�s�n� g�sterecek TextMeshPro referans�

    private int botCount;
    private int totalBots;

    void Start()
    {
        // Sahnedeki botlar� bul ve say
        GameObject[] bots = GameObject.FindGameObjectsWithTag(botTag);
        botCount = bots.Length;
        totalBots = botCount;

        Debug.Log($"Toplam Bot Say�s�: {botCount}");

        // Bot say�s�n� UI'ye yazd�r
        UpdateBotCountUI();
    }

    public void OnBotKilled()
    {
        botCount--;

        Debug.Log($"Kalan Bot Say�s�: {botCount}");

        // Bot say�s�n� UI'ye yazd�r
        UpdateBotCountUI();

        // E�er t�m botlar �ld�yse
        if (botCount <= 0)
        {
            Debug.Log("T�m botlar �ld�! Kazand�n�z.");
            SceneManager.LoadScene(winSceneName);
        }
    }

    private void UpdateBotCountUI()
    {
        if (botCountText != null)
        {
            botCountText.text = $"{botCount}/{totalBots}";
        }
        else
        {
            Debug.LogWarning("Bot Count TextMeshPro referans� atanmad�!");
        }
    }
}
