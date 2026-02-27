using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    private int score;
    private int comboMultiplier = 1;
    private float comboDuration = 4f;
    private float comboTimer;

    private const int BASE_SCORE = 100;

    public int CurrentScore => score;
    public int CurrentCombo => comboMultiplier;

    void Update()
    {
        if (comboMultiplier > 1)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                comboMultiplier = 1;
                UpdateUI();
            }
        }
    }

    public void AddMatch()
    {
        score += BASE_SCORE * comboMultiplier;
        comboMultiplier++;
        comboTimer = comboDuration;
        UpdateUI();
    }

    public void AddMismatch()
    {
        comboMultiplier = 1;
        score = Mathf.Max(0, score - 10);
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        comboMultiplier = 1;
        UpdateUI();
    }

    public void SetScore(int savedScore, int savedCombo)
    {
        score = savedScore;
        comboMultiplier = savedCombo;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text =score.ToString();
        comboText.text = "x" + comboMultiplier;
    }
}