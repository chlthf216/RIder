using UnityEngine;

public class CarCollisionSound : MonoBehaviour
{
    private AudioSource coinSoundAudio;

    void Start()
    {
        // MyCar ������Ʈ(�ڱ� �ڽ�)�� �ڽ� �� CoinSound ������Ʈ���� AudioSource ��������
        Transform coinSoundTransform = transform.Find("CoinSound");
        if (coinSoundTransform != null)
        {
            coinSoundAudio = coinSoundTransform.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("CoinSound ������Ʈ�� ã�� �� �����ϴ�!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ������Ʈ�� Coin �±����� üũ
        if (other.CompareTag("Coin"))
        {
            if (coinSoundAudio != null)
            {
                coinSoundAudio.PlayOneShot(coinSoundAudio.clip);
            }
        }
    }
}

