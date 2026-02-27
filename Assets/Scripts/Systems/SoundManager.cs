using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;
    [SerializeField] private AudioClip gameOverSound;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayFlip() => audioSource.PlayOneShot(flipSound);
    public void PlayMatch() => audioSource.PlayOneShot(matchSound);
    public void PlayMismatch() => audioSource.PlayOneShot(mismatchSound);
    public void PlayGameOver() => audioSource.PlayOneShot(gameOverSound);
}