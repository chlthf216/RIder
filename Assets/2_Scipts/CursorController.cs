using UnityEngine;

public class CursorController : MonoBehaviour
{
    private Camera mainCam;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainCam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 기본 유니티 커서 숨기기
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // 카메라에서 10단위 앞쪽으로 보정(필요에 따라 조절)
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(worldPos.x, worldPos.y, 0f);
    }
}

