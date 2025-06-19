using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private bool collected = false; // 여러번 실행 방지

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCoinSound(GameObject coinObject)
    {
        AudioSource audio = coinObject.GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            UIManager.Instance.AddScore(1);



            // 오디오가 재생된 후 오브젝트 제거 (클립 길이만큼 대기)
            Destroy(gameObject);
        }
    }
}
