using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Camera mainCam;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // �⺻ ����Ƽ Ŀ�� �����
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // ī�޶󿡼� 10���� �������� ����(�ʿ信 ���� ����)
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }
}

