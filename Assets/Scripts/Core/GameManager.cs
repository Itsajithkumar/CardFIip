using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Systems")]
    [SerializeField] private MatchSystem matchSystem;
    [SerializeField] private ScoreSystem scoreSystem;
    [Header("UI")]
    [SerializeField] private Button resumeButton;

    [Header("Sprites")]
    [SerializeField] private List<Sprite> cardSprites;

    [Header("Grid")]
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;

    private List<Card> activeCards = new List<Card>();
    [SerializeField] private int totalPairs;
    [SerializeField] private int matchedPairs;
    [Header("Game State")]
    public GameObject Welcomepanel;
    public GameObject Gamepanel;
    public GameObject GameOverPanel;
    void Start()
    {
        resumeButton.interactable = SaveSystem.Instance.HasSave();
    }

    public void StartNewGame()
    {
        SaveSystem.Instance.ClearSave();
        resumeButton.interactable = false;

        matchSystem.StopProcessing();
        ClearBoard();
        scoreSystem.ResetScore();
        GenerateBoard();
         Welcomepanel.SetActive(false);
        Gamepanel.SetActive(true);
    }

    public void ResumeGame()
    {
        if (!SaveSystem.Instance.HasSave())
            return;

        matchSystem.StopProcessing();
        ClearBoard();

        GameData data = SaveSystem.Instance.LoadGame();
        GenerateBoardFromSave(data);

        scoreSystem.SetScore(data.score, data.combo);
        Welcomepanel.SetActive(false);
        Gamepanel.SetActive(true);
    }

    private void GenerateBoard()
    {
        int totalCards = rows * columns;
        totalPairs = totalCards / 2;
        matchedPairs = 0;

        List<int> ids = new List<int>();

        for (int i = 0; i < totalPairs; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            CreateCard(ids[i]);
        }

        matchSystem.OnMatch += OnMatch;
        matchSystem.OnMismatch += OnMismatch;
    }
    private void GenerateBoardFromSave(GameData data)
    {
        totalPairs = data.cardIDs.Count / 2;
        matchedPairs = 0;

        Dictionary<int, int> matchCounter = new Dictionary<int, int>();

        for (int i = 0; i < data.cardIDs.Count; i++)
        {
            Card card = CreateCard(data.cardIDs[i]);

            if (data.matchedStates[i])
            {
                card.SetMatched();
                card.InstantReveal();

                if (!matchCounter.ContainsKey(card.CardID))
                    matchCounter[card.CardID] = 0;

                matchCounter[card.CardID]++;
            }
        }

        foreach (var pair in matchCounter)
        {
            if (pair.Value == 2)
                matchedPairs++;
        }
    }

    private Card CreateCard(int id)
    {
        GameObject obj = Instantiate(cardPrefab, gridParent);
        Card card = obj.GetComponent<Card>();

        card.Initialize(id, cardSprites[id], matchSystem);
        activeCards.Add(card);
        return card;
    }

    private void OnMatch()
    {
        matchedPairs++;
        scoreSystem.AddMatch();
        SaveCurrentGame();

        if (matchedPairs >= totalPairs)
            GameWon();
    }

    private void OnMismatch()
    {
        scoreSystem.AddMismatch();
        SaveCurrentGame();
    }
    public void QuitGame()
    {
        Debug.Log("Application Quit!");
        Application.Quit();
    }

    public void RetryGame()
    {    
        matchSystem.StopProcessing();      
        SaveSystem.Instance.ClearSave();
        resumeButton.interactable = false;  
        ClearBoard();  
        scoreSystem.ResetScore();
        matchedPairs = 0;
        GenerateBoard();
    }
    private void SaveCurrentGame()
    {
        GameData data = new GameData();
        data.score = scoreSystem.CurrentScore;
        data.combo = scoreSystem.CurrentCombo;

        foreach (Card c in activeCards)
        {
            data.cardIDs.Add(c.CardID);
            data.matchedStates.Add(c.IsMatched);
        }

        SaveSystem.Instance.SaveGame(data);
        resumeButton.interactable = true;
    }
    public void sceneload()
    {
        Invoke(nameof(SceneCall), 0.5f);
    }

    private void SceneCall()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void GameWon()
    {
        SaveSystem.Instance.ClearSave();
        resumeButton.interactable = false;
        GameOverPanel.SetActive(true);
        SoundManager.Instance.PlayGameOver();
        Debug.Log("Game Completed!");
    }

    private void ClearBoard()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        activeCards.Clear();
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}