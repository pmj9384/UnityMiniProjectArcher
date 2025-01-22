using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;  // **추가된 Pause Panel**

    // public void UpdateScoreText(int newScore)
    // {
    //     scoreText.text = $"SCORE: {newScore}";
    // }

    // public void UpdateWaveText(int wave, int count)
    // {
    //     waveText.text = $"Wave: {wave}\nEnemy Left: {count}";
    // }

    public void ShowGameOverPanel(bool active)
    {
        gameOverPanel.SetActive(active);
    }

    public void ShowGamePausePanel(bool active)
    {
        pausePanel.SetActive(active);  
    }

    public void HideGamePausePanel()
    {
        pausePanel.SetActive(false);  
    }
}
