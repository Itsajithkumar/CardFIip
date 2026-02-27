using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string saveKey = "FullGameSave";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveGame(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public GameData LoadGame()
    {
        if (!HasSave())
            return null;

        string json = PlayerPrefs.GetString(saveKey);
        return JsonUtility.FromJson<GameData>(json);
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey(saveKey);
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(saveKey);
    }
}