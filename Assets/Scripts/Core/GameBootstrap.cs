using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] private MatchSystem matchSystem;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        matchSystem.OnMatch += HandleMatch;
        matchSystem.OnMismatch += HandleMismatch;
    }

    private void OnDestroy()
    {
        matchSystem.OnMatch -= HandleMatch;
        matchSystem.OnMismatch -= HandleMismatch;
    }

    private void HandleMatch()
    {
        scoreSystem.AddMatch();
        soundManager.PlayMatch();
    }

    private void HandleMismatch()
    {
        scoreSystem.AddMismatch();
        soundManager.PlayMismatch();
    }
}