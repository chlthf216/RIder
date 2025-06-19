using UnityEngine;

public class ScoreByDistance : MonoBehaviour
{
    public float distanceThreshold = 1f;   // 1미터 이동 시마다 점수 증가
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position; // 시작 위치 저장
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        if (distanceMoved >= distanceThreshold)
        {
            // 점수 1 올리기 (UIManager에 맞게 호출)
            UIManager.Instance.AddScore(1);

            // 기준 위치 갱신
            lastPosition = transform.position;
        }
    }
}
