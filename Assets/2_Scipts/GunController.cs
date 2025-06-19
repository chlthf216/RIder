using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
public class GunController : MonoBehaviour
{
    [Header("�� ����")]
    public Transform firePoint;           // �ѱ� ��ġ
    public GameObject hitEffect;// �¾��� �� ����� ��ƼŬ ������ (PlayOnAwake, Loop ����)
    public GameObject boostedHitEffect;
    public float jumpForce = 10f;         // �� �¾��� �� �ڵ��� ���� ��
    public float obstacleHitForce = 5f;   // ��ֹ� �¾��� �� ���ϴ� ��
    [SerializeField] private GameObject bulletTrailPrefab;

    [Header("źâ ����")]
    public int maxAmmo = 8;          // �ִ� źȯ ��
    public float reloadTime = 2f;    // ������ �ð�
    private int currentAmmo;
    private bool isReloading = false;

    [Header("�ν�Ʈ ����")]
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
        Cursor.visible = false;  // ���콺 Ŀ�� �����
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
                Debug.Log("źȯ ����! ������ �ʿ� (���콺 ��Ŭ��)");
                // ���⼭ '�� ��' ���带 ����ص� ����
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
        Debug.Log("������ ��...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("������ �Ϸ�!");
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
        UIManager.Instance.UpdateAmmoText(-1, maxAmmo);  // -1�� ���� ǥ�ÿ�
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
            // �ڱ� �ڽ� �浹ü ����
            if (hit.collider.gameObject == this.gameObject)
                continue;

            Debug.Log("Raycast Hit: " + hit.collider.name + ", Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Obstacle"))
            {
                // �ν�Ʈ ���¿� ���� ����Ʈ ������ ����
                GameObject effectPrefab = isBoosted ? boostedHitEffect : hitEffect;

                // ���� ��ġ�� ����Ʈ ���� �� ���
                GameObject effect = Instantiate(effectPrefab, hit.point, Quaternion.identity);
                ParticleSystem ps = effect.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
                Destroy(effect, 2f);

                if (hit.collider.CompareTag("Ground"))
                {
                    // �ٴ� �¾��� �� �ݵ� ������ ���� ǥ���� ���� ����
                    Vector2 jumpDirection = hit.normal.normalized;
                    carRb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
                }

                if (hit.collider.CompareTag("Obstacle"))
                {
                    DestructibleObstacle destructible = hit.collider.GetComponent<DestructibleObstacle>();
                    if (destructible != null)
                    {
                        destructible.Hit(isBoosted); // ��Ʈ ȣ���ؼ� ó��
                    }

                    Rigidbody2D obstacleRb = hit.collider.GetComponent<Rigidbody2D>();
                    if (obstacleRb != null)
                    {
                        obstacleRb.AddForce(direction * obstacleHitForce, ForceMode2D.Impulse);
                    }
                }

                // ù ��ȿ ��Ʈ�� ó���ϰ� ����
                break;
            }

        }
        // �Ѿ� ���� (LineRenderer�� ǥ��)
        GameObject trailObj = new GameObject("BulletTrail");
        LineRenderer lr = trailObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, firePoint.position + (Vector3)(direction * 100f));

        // ���� ���� (�ν�Ʈ ���¿� ���� �ٸ���)
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

        // ���͸��� �� ���� ����
        lr.material = new Material(Shader.Find("Sprites/Default"));
        Color trailColor = isBoosted ? new Color(0.2f, 0.6f, 1f, 0.8f) : new Color(0.5f, 0.5f, 0.5f, 1f);
        lr.startColor = trailColor;
        lr.endColor = trailColor;

        lr.sortingOrder = 10;

        // trail�� ��� ���̰� �����
        Destroy(trailObj, 0.05f);



    }
}
