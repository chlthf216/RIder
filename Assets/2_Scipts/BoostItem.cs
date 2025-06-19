using UnityEngine;

public class BoostItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GunController gun = other.GetComponent<GunController>();
            if (gun != null)
            {
                gun.ActivateBoost();
            }

            Destroy(gameObject); // 아이템 제거
        }
    }
}
