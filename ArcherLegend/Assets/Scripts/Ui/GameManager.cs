using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 패턴
    private int score = 0;
    public bool IsGameOver { get; private set; }
    public bool IsGamePause { get; private set; }

    public UiManager uiManager;

    public int remainingZombies; // 총 적의 수
    private List<GameObject> expItems = new List<GameObject>(); // 생성된 경험치 리스트

    private void Awake()
    {
        Instance = this;
    }

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
        // uiManager.UpdateScoreText(score);
    }

     public void IncrementZombieCount()
    {
        remainingZombies++;
    }
    // 적이 처치될 때 호출되는 메서드
    public void DecrementZombieCount()
    {
        remainingZombies--;

        if (remainingZombies <= 0)
        {
            // 모든 적이 죽었으므로 경험치 아이템 끌어오기 시작
            ExperienceManager.Instance.OnAllEnemiesDead();
        }

        // uiManager.UpdateWaveText(CurrentWave, remainingZombies);
    }

    // 경험치 아이템을 등록하는 메서드
    public void RegisterExp(GameObject expItem)
    {
        expItems.Add(expItem); // 생성된 경험치 아이템 등록
    }

    private void EnableAllExpItems()
    {
        foreach (var expItem in expItems)
        {
            if (expItem != null)
            {
                expItem.GetComponent<EnemyExp>().EnableCollection();
            }
        }
        expItems.Clear(); // 리스트 초기화
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
