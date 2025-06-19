using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarEngineSound : MonoBehaviour
{
    public Rigidbody2D rb;                  // 자동차의 Rigidbody2D
    public float maxSpeed = 20f;            // 최고 속도 기준
    public float volumeMultiplier = 1.0f;   // 최대 볼륨 설정 (기본은 1)

    private AudioSource exhaustAudio;
    private bool isOnGround = false;

    void Start()
    {
        exhaustAudio = GetComponent<AudioSource>();
        exhaustAudio.loop = true;
        exhaustAudio.volume = 0f;
        exhaustAudio.Play();
    }

    void Update()
    {
        if (rb == null) return;

        float speed = rb.linearVelocity.magnitude; // 수정: velocity -> linearVelocity

        if (isOnGround)
        {
            float volume = Mathf.Clamp01(speed / maxSpeed) * volumeMultiplier;
            exhaustAudio.volume = volume;
        }
        else
        {
            exhaustAudio.volume = 0f;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
}
