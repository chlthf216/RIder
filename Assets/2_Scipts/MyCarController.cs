using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool isGameOver = false;

    // �ӵ� �� ȸ�� ���� ����
    [SerializeField] private float maxSpeed = 10f;         // Inspector���� ������ �ִ� �ӵ�
    [SerializeField] private float acceleration = 10f;      // ���ӵ�
    [SerializeField] private float deceleration = 15f;      // ���ӵ�
    [SerializeField] private float rotationSpeed = 100f;    // ȸ�� �ӵ�
    [SerializeField] private float angularDrag = 1f;        // ȸ�� ����
    [SerializeField] private AudioSource explosionAudio;
    private float currentSpeed = 0f;


    // ������ ���� ���� ����
    public float flipCheckDuration = 2f;
    private float flipTimer = 0f;

    // �ڵ��� ���־� ������Ʈ (Inspector���� �Ҵ�)
    public GameObject carVisual;

    // �ڽ����� �پ��ִ� ������ ����Ʈ ������Ʈ (��Ȱ��ȭ ���·� �μ���)
    public GameObject breakEffectObject;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<SurfaceEffector2D>(out var effector))
        {
            surfaceEffector2D = effector;
        }

        if (collision.gameObject.CompareTag("Killzone") || collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }

        
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance.IsGameOverPanelActive())
            {
                UIManager.Instance.ShowGameOverPanel(false);
                isGameOver = false;
                Time.timeScale = 1f;
                return;
            }
            else
            {
                UIManager.Instance.ShowGameOverPanel(true);
                Time.timeScale = 0f;
            }
            return;
        }

        if (isGameOver) return;

        // ������ ����
        float z = transform.eulerAngles.z;
        bool isFlipped = (z > 90f && z < 270f);
        if (isFlipped)
        {
            flipTimer += Time.deltaTime;
            if (flipTimer >= flipCheckDuration)
            {
                GameOver();
            }
        }
        else
        {
            flipTimer = 0f;
        }

        if (surfaceEffector2D == null) return;

        // --- �� ����Ű: ����/���� ---
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            // ���� ó��
            if (currentSpeed > 0)
                currentSpeed -= deceleration * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += deceleration * Time.deltaTime;
        }

        // �ӵ� ���� ���� (���� �ִ�ӵ� ~ ������ �ִ�ӵ�)
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // ���� ����
        surfaceEffector2D.speed = currentSpeed;


        // �ӵ� ���� ���� (-maxSpeed ~ +maxSpeed)
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // �ݿ�
        surfaceEffector2D.speed = currentSpeed;


        // --- �¿� ����Ű: ȸ�� ---
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-rotationSpeed * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            rb.angularVelocity *= (1 - angularDrag * Time.deltaTime);
        }

        if (currentSpeed <= 0f)
        {
            surfaceEffector2D.speed = 0f;  // �� ����
        }
        else
        {
            surfaceEffector2D.speed = currentSpeed; // ���ó�� �б�
        }

        UIManager.Instance.UpdateSurfaceText($"Surface Speed : {surfaceEffector2D.speed:F1}");
        UIManager.Instance.UpdateCarSpeedText($"Car Speed : {rb.linearVelocity.magnitude:F1}");
    }

    private void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (explosionAudio != null)
        {
            explosionAudio.Play();
        }
        else
        {
            Debug.LogWarning(" explosionAudio�� �Ҵ���� �ʾҽ��ϴ�!");
        }

        if (carVisual != null)
            carVisual.SetActive(false);

        if (breakEffectObject != null)
        {
            if (breakEffectObject.TryGetComponent<ParticleSystem>(out var ps))
            {
                breakEffectObject.SetActive(true);
                ps.Play();

            }
            else
            {
                breakEffectObject.SetActive(true);
            }
        }

        StartCoroutine(ShowPanelAfterDelay(0.5f));
    }

    private IEnumerator ShowPanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        UIManager.Instance.ShowGameOverPanel(true);
        Time.timeScale = 0f;
    }
}
