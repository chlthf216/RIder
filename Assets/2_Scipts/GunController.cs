using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public class GunController : MonoBehaviour
{
    [Header("총 관련")]
    public Transform firePoint;           // 총구 위치
    public GameObject hitEffect;// 맞았을 때 재생할 파티클 프리팹 (PlayOnAwake, Loop 꺼짐)
    public GameObject boostedHitEffect;
    public float jumpForce = 10f;         // 땅 맞았을 때 자동차 점프 힘
    public float obstacleHitForce = 5f;   // 장애물 맞았을 때 가하는 힘
    [SerializeField] private GameObject bulletTrailPrefab;

    [Header("탄창 관련")]
    public int maxAmmo = 8;          // 최대 탄환 수
    public float reloadTime = 2f;    // 재장전 시간
    private int currentAmmo;
    private bool isReloading = false;

    [Header("부스트 관련")]
    public float boostDuration = 5f;
    private bool isBoosted = false;
    private Coroutine boostCoroutine;

    private Rigidbody2D carRb;
    private Camera mainCam;

    private AudioSource audioSource;

    private void Start()
    {
        carRb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        Cursor.visible = false;  // 마우스 커서 숨기기
        currentAmmo = maxAmmo;
        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0 || isBoosted)
            {
                Fire();
                
            }
            else
            {
                Debug.Log("탄환 없음! 재장전 필요 (마우스 우클릭)");
                // 여기서 '빈 총' 사운드를 재생해도 좋아
                AudioSource reloadAudio = transform.Find("ReloadSound")?.GetComponent<AudioSource>();
                if (reloadAudio != null)
                {
                    reloadAudio.Play();
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("재장전 완료!");
    }

    public void ActivateBoost()
    {
        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(BoostRoutine());
    }

    private IEnumerator BoostRoutine()
    {
        isBoosted = true;
        UIManager.Instance.UpdateAmmoText(-1, maxAmmo);  // -1은 무한 표시용
        yield return new WaitForSeconds(boostDuration);
        isBoosted = false;
        UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);
    }

    private void Fire()
    {
        if(currentAmmo <= 0 ) return;

        if (!isBoosted)
        {
            currentAmmo--;
            UIManager.Instance.UpdateAmmoText(currentAmmo, maxAmmo);
        }

        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, direction, 100f);

        foreach (var hit in hits)
        {
            // 자기 자신 충돌체 무시
            if (hit.collider.gameObject == this.gameObject)
                continue;

            Debug.Log("Raycast Hit: " + hit.collider.name + ", Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacle"))
            {
                // 부스트 상태에 따라 이펙트 프리팹 선택
                GameObject effectPrefab = isBoosted ? boostedHitEffect : hitEffect;

                // 맞은 위치에 이펙트 생성 및 재생
                GameObject effect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
                Destroy(effect, 2f);

                if (hit.collider.CompareTag("Ground"))
                {
                    // 바닥 맞았을 때 반동 방향은 맞은 표면의 법선 방향
                    Vector2 jumpDirection = hit.normal.normalized;
                    carRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
                }

                if (hit.collider.CompareTag("Obstacle"))
                {
                    DestructibleObstacle destructible = hit.collider.GetComponent<DestructibleObstacle>();
                    if (destructible != null)
                    {
                        destructible.Hit(isBoosted); // 히트 호출해서 처리
                    }

                    Rigidbody2D obstacleRb = hit.collider.GetComponent<Rigidbody2D>();
                    if (obstacleRb != null)
                    {
                        obstacleRb.AddForce(direction * obstacleHitForce, ForceMode2D.Impulse);
                    }
                }

                // 첫 유효 히트만 처리하고 종료
                break;
            }

        }
        // 총알 궤적 (LineRenderer로 표시)
        GameObject trailObj = new GameObject("BulletTrail");
        LineRenderer lr = trailObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, firePoint.position + (Vector3)(direction * 100f));

        // 굵기 설정 (부스트 상태에 따라 다르게)
        if (isBoosted)
        {
            lr.startWidth = 0.3f;
            lr.endWidth = 0.3f;
        }
        else
        {
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
        }

        // 머터리얼 및 색상 설정
        lr.material = new Material(Shader.Find("Sprites/Default"));
        Color trailColor = isBoosted ? new Color(0.2f, 0.6f, 1f, 0.8f) : new Color(0.5f, 0.5f, 0.5f, 1f);
        lr.startColor = trailColor;
        lr.endColor = trailColor;

        lr.sortingOrder = 10;

        // trail은 잠깐 보이고 사라짐
        Destroy(trailObj, 0.05f);



    }
}
