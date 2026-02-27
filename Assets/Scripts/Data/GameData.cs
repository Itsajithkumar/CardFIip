using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int score;
    public int combo;
    public List<int> cardIDs = new List<int>();
    public List<bool> matchedStates = new List<bool>();
}