using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MyCarController : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector2D;
    private Rigidbody2D rb;
    private bool isGameOver = false;

    // 속도 및 회전 관련 변수
    [SerializeField] private float maxSpeed = 10f;         // Inspector에서 지정할 최대 속도
    [SerializeField] private float acceleration = 10f;      // 가속도
    [SerializeField] private float deceleration = 15f;      // 감속도
    [SerializeField] private float rotationSpeed = 100f;    // 회전 속도
    [SerializeField] private float angularDrag = 1f;        // 회전 감속
    [SerializeField] private AudioSource explosionAudio;
    private float currentSpeed = 0f;


    // 뒤집힘 감지 관련 변수
    public float flipCheckDuration = 2f;
    private float flipTimer = 0f;

    // 자동차 비주얼 오브젝트 (Inspector에서 할당)
    public GameObject carVisual;

    // 자식으로 붙어있는 터지는 이펙트 오브젝트 (비활성화 상태로 두세요)
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

        // 뒤집힘 감지
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

        // --- 위 방향키: 가속/감속 ---
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
            // 감속 처리
            if (currentSpeed > 0)
                currentSpeed -= deceleration * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += deceleration * Time.deltaTime;
        }

        // 속도 범위 제한 (왼쪽 최대속도 ~ 오른쪽 최대속도)
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // 실제 적용
        surfaceEffector2D.speed = currentSpeed;


        // 속도 범위 제한 (-maxSpeed ~ +maxSpeed)
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // 반영
        surfaceEffector2D.speed = currentSpeed;


        // --- 좌우 방향키: 회전 ---
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
            surfaceEffector2D.speed = 0f;  // 힘 제거
        }
        else
        {
            surfaceEffector2D.speed = currentSpeed; // 평소처럼 밀기
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
            Debug.LogWarning(" explosionAudio가 할당되지 않았습니다!");
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
