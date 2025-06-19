using UnityEngine;

public class ScoreByDistance : MonoBehaviour
{
    public float distanceThreshold = 1f;   // 1���� �̵� �ø��� ���� ����
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position; // ���� ��ġ ����
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        if (distanceMoved >= distanceThreshold)
        {
            // ���� 1 �ø��� (UIManager�� �°� ȣ��)
            UIManager.Instance.AddScore(1);

            // ���� ��ġ ����
            lastPosition = transform.position;
        }
    }
}
