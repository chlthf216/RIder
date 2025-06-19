using UnityEngine;

public class CarCollisionSound : MonoBehaviour
{
    private AudioSource coinSoundAudio;

    void Start()
    {
        // MyCar 오브젝트(자기 자신)의 자식 중 CoinSound 오브젝트에서 AudioSource 가져오기
        Transform coinSoundTransform = transform.Find("CoinSound");
        if (coinSoundTransform != null)
        {
            coinSoundAudio = coinSoundTransform.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("CoinSound 오브젝트를 찾을 수 없습니다!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 Coin 태그인지 체크
        if (other.CompareTag("Coin"))
        {
            if (coinSoundAudio != null)
            {
                coinSoundAudio.PlayOneShot(coinSoundAudio.clip);
            }
        }
    }
}

