using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // Inspector에서 4개 음악 할당
    public bool playRandom = false; // true면 무작위, false면 순차재생

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
