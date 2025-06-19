using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var carController = collision.GetComponent<MyCarController>();
        if (carController != null)
        {
            carController.SendMessage("GameOver");
        }
    }
}
