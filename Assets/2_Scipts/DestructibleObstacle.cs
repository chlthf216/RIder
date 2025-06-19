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

    // 기존: 인수 없는 Hit
    public void Hit()
    {
        ApplyHit(1);
    }

    // 새로 추가: 부스트 여부를 받아 처리하는 Hit
    public void Hit(bool isBoosted)
    {
        if (isBoosted)
        {
            ApplyHit(maxHits); // 한 방에 파괴
        }
        else
        {
            ApplyHit(1); // 기존처럼 1회 데미지
        }
    }

    // 실제 데미지 계산 로직을 함수로 분리
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


