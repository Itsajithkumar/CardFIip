using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image cardImage;
    [SerializeField] private Button button;
    [SerializeField] private Sprite backSprite;

    [Header("Animation")]
    [SerializeField] private float flipDuration = 0.25f;

    private Sprite frontSprite;

    private int cardID;
    private bool isFlipped;
    private bool isMatched;
    private bool isAnimating;

    private MatchSystem matchSystem;

    public int CardID => cardID;
    public bool IsMatched => isMatched;

    public void Initialize(int id, Sprite front, MatchSystem system)
    {
        cardID = id;
        frontSprite = front;
        matchSystem = system;

        isMatched = false;
        isFlipped = false;
        isAnimating = false;

        cardImage.sprite = backSprite;
        button.interactable = true;
        transform.rotation = Quaternion.identity;
    }
    public void InstantReveal()
    {
        isFlipped = true;
        cardImage.sprite = frontSprite;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }
    public void OnClick()
    {
        if (isFlipped || isMatched || isAnimating)
            return;

        if (!matchSystem.CanAcceptInput())
            return;

        StartCoroutine(FlipForward());
        matchSystem.RegisterCard(this);
    }

    public void Flip()
    {
        if (!isAnimating)
            StartCoroutine(FlipForward());
    }

    public void ShowBack()
    {
        if (!isAnimating)
            StartCoroutine(FlipBackward());
    }

    public void SetMatched()
    {
        isMatched = true;
        button.interactable = false;
    }

    private IEnumerator FlipForward()
    {
        isAnimating = true;

        float time = 0f;

        while (time < flipDuration)
        {
            float angle = Mathf.Lerp(0f, 90f, time / flipDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        cardImage.sprite = frontSprite;

        time = 0f;

        while (time < flipDuration)
        {
            float angle = Mathf.Lerp(90f, 180f, time / flipDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        isFlipped = true;
        isAnimating = false;

        SoundManager.Instance?.PlayFlip();
    }

    private IEnumerator FlipBackward()
    {
        isAnimating = true;

        float time = 0f;

        while (time < flipDuration)
        {
            float angle = Mathf.Lerp(180f, 90f, time / flipDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        cardImage.sprite = backSprite;

        time = 0f;

        while (time < flipDuration)
        {
            float angle = Mathf.Lerp(90f, 0f, time / flipDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.identity;

        isFlipped = false;
        isAnimating = false;
    }
}