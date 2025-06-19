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

            // ���� ����Ʈ
            if (effectPrefab != null)
                Instantiate(effectPrefab, transform.position, Quaternion.identity);

            // ������ �ð� ���
            int score = UIManager.Instance != null ? UIManager.Instance.GetScore() : 0;
            float elapsedTime = Time.time - startTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            // �ؽ�Ʈ UI�� ǥ��
            if (totalScoreText != null)
                totalScoreText.text = $"�� ����: {score}";

            if (timeText != null)
                timeText.text = $"�ɸ� �ð�: {minutes}:{seconds:00}";

            // �г� ǥ��
            if (finishPanel != null)
                finishPanel.SetActive(true);

            // ���� �Ͻ�����
            Time.timeScale = 0f;
        }
    }

    // Retry ��ư���� ȣ���� �Լ�
    public void RetryGame()
    {
        Time.timeScale = 1f; // �Ͻ����� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �ٽ� �ε�
    }
}


