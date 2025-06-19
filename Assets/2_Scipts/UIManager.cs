using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text timeText;
    public Text surfaceSpeedText;
    public Text carSpeedText;
    public Text scoreText;
    public GameObject gameOverPanel; // 패널 참조 추가
    private int currentScore = 0;
    public Text ammoText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTimeText(string time)
    {
        timeText.text = time;
    }
    public void UpdateSurfaceText(string speed)
    {
        surfaceSpeedText.text = speed;
    }
    public void UpdateCarSpeedText(string speed)
    {
        carSpeedText.text = speed;
    }

    public void ShowGameOverPanel(bool show)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(show);
    }

    public bool IsGameOverPanelActive()
    {
        return gameOverPanel != null && gameOverPanel.activeSelf;
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }
    public void UpdateAmmoText(int current, int max)
    {
        if (current < 0)
            ammoText.text = "∞ / " + max.ToString();
        else
            ammoText.text = current + " / " + max;
    }
    public int GetScore()
    {
        return currentScore;
    }
}
