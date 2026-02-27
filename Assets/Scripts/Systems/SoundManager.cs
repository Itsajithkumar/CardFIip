using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip flipSound;
    [SerializeField] private AudioClip matchSound;
    [SerializeField] private AudioClip mismatchSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip ButtonClickSound;
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

       
    }

    public void PlayFlip() => audioSource.PlayOneShot(flipSound);
    public void PlayMatch() => audioSource.PlayOneShot(matchSound);
    public void PlayMismatch() => audioSource.PlayOneShot(mismatchSound);
    public void PlayGameOver() => audioSource.PlayOneShot(gameOverSound);
    public void ButtonClick() => audioSource.PlayOneShot(ButtonClickSound);
}