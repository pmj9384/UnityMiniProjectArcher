using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    public bool IsGameOver { get; private set; }
    public bool IsGamePause { get; private set; }

    public UiManager uiManager;
    public int CurrentWave { get; private set; } = 0;
    private int remainingZombies;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsGamePause)
            {
                PauseGame(); 
            }
            else
            {
                ResumeGame();  
            }
        }
    }

    public void AddScore(int add)
    {
        if (IsGameOver) return;
        score += add;
        uiManager.UpdateScoreText(score);
    }

    public void DecrementZombieCount()
    {
        remainingZombies--;
        uiManager.UpdateWaveText(CurrentWave, remainingZombies);

        if (remainingZombies <= 0)
        {
            CurrentWave++;
            remainingZombies = 3 * CurrentWave;  
            uiManager.UpdateWaveText(CurrentWave, remainingZombies);
        }
    }

    public void OnGameOver()
    {
        IsGameOver = true;
        uiManager.ShowGameOverPanel(true);
        Time.timeScale = 0f;  
    }

    public void PauseGame()
    {
        IsGamePause = true;
        Time.timeScale = 0f;  
        uiManager.ShowGamePausePanel(true);  
    }

    public void ResumeGame()
    {
        IsGamePause = false;
        Time.timeScale = 1f;  
        uiManager.HideGamePausePanel();  
    }
}
