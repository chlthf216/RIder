using UnityEngine;

public class DestructibleObstacle : MonoBehaviour
{
    [SerializeField] private int maxHits = 5;
    private int currentHits;
    private SpriteRenderer sr;

    private void Awake()
    {
        currentHits = maxHits;
        sr = GetComponent<SpriteRenderer>();
    }

    // ����: �μ� ���� Hit
    public void Hit()
    {
        ApplyHit(1);
    }

    // ���� �߰�: �ν�Ʈ ���θ� �޾� ó���ϴ� Hit
    public void Hit(bool isBoosted)
    {
        if (isBoosted)
        {
            ApplyHit(maxHits); // �� �濡 �ı�
        }
        else
        {
            ApplyHit(1); // ����ó�� 1ȸ ������
        }
    }

    // ���� ������ ��� ������ �Լ��� �и�
    private void ApplyHit(int damage)
    {
        currentHits -= damage;

        float alpha = Mathf.Clamp01((float)currentHits / maxHits);
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }

        if (currentHits <= 0)
        {
            Destroy(gameObject);
        }
    }
}


