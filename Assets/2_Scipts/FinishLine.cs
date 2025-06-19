using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject finishPanel;
    public GameObject effectPrefab;
    public Text totalScoreText;
    public Text timeText;

    private float startTime;
    private bool finished = false;

    private void Start()
    {
        startTime = Time.time;

        if (finishPanel != null)
            finishPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {
            finished = true;

            // 도착 이펙트
            if (effectPrefab != null)
                Instantiate(effectPrefab, transform.position, Quaternion.identity);

            // 점수와 시간 계산
            int score = UIManager.Instance != null ? UIManager.Instance.GetScore() : 0;
            float elapsedTime = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // 텍스트 UI에 표시
            if (totalScoreText != null)
                totalScoreText.text = $"총 점수: {score}";

            if (timeText != null)
                timeText.text = $"걸린 시간: {minutes}:{seconds:00}";

            // 패널 표시
            if (finishPanel != null)
                finishPanel.SetActive(true);

            // 게임 일시정지
            Time.timeScale = 0f;
        }
    }

    // Retry 버튼에서 호출할 함수
    public void RetryGame()
    {
        Time.timeScale = 1f; // 일시정지 해제
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }
}


