using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // Inspector���� 4�� ���� �Ҵ�
    public bool playRandom = false; // true�� ������, false�� �������

    private AudioSource audioSource;
    private int currentIndex = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        PlayNextBGM();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextBGM();
        }
    }

    private void PlayNextBGM()
    {
        if (bgmClips == null || bgmClips.Length == 0) return;

        if (playRandom)
        {
            int nextIndex = Random.Range(0, bgmClips.Length);
            audioSource.clip = bgmClips[nextIndex];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = bgmClips[currentIndex];
            audioSource.Play();
            currentIndex = (currentIndex + 1) % bgmClips.Length;
        }
    }
}
