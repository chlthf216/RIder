using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private AudioSource audioSource;
    private bool collected = false; // ������ ���� ����

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



            // ������� ����� �� ������Ʈ ���� (Ŭ�� ���̸�ŭ ���)
            Destroy(gameObject);
        }
    }
}
