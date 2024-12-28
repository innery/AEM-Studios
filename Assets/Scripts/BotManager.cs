using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BotManager : MonoBehaviour
{
    public string botTag = "Bot"; // Botlarýn sahip olduðu tag
    public string winSceneName = "WinScene"; // Kazanma sahnesinin adý
    public TextMeshProUGUI botCountText; // Bot sayýsýný gösterecek TextMeshPro referansý

    private int botCount;
    private int totalBots;

    void Start()
    {
        // Sahnedeki botlarý bul ve say
        GameObject[] bots = GameObject.FindGameObjectsWithTag(botTag);
        botCount = bots.Length;
        totalBots = botCount;

        Debug.Log($"Toplam Bot Sayýsý: {botCount}");

        // Bot sayýsýný UI'ye yazdýr
        UpdateBotCountUI();
    }

    public void OnBotKilled()
    {
        botCount--;

        Debug.Log($"Kalan Bot Sayýsý: {botCount}");

        // Bot sayýsýný UI'ye yazdýr
        UpdateBotCountUI();

        // Eðer tüm botlar öldüyse
        if (botCount <= 0)
        {
            Debug.Log("Tüm botlar öldü! Kazandýnýz.");
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
            Debug.LogWarning("Bot Count TextMeshPro referansý atanmadý!");
        }
    }
}
