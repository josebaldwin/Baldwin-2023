using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}

public class HighScoreManager : MonoBehaviour
{
    public List<HighScoreEntry> highScores = new List<HighScoreEntry>();
    private const string HighScoreKey = "HighScores";

    void Awake()
    {
        LoadHighScores();
    }

    public void AddHighScore(string name, int score)
    {
        HighScoreEntry newEntry = new HighScoreEntry { playerName = name, score = score };
        highScores.Add(newEntry);
        highScores = highScores.OrderByDescending(x => x.score).ToList();

        // Keep top 10 scores
        if (highScores.Count > 10)
            highScores.RemoveRange(10, highScores.Count - 10);

        SaveHighScores();
    }

    private void LoadHighScores()
    {
        string jsonData = PlayerPrefs.GetString(HighScoreKey, "");
        if (!string.IsNullOrEmpty(jsonData))
        {
            highScores = JsonUtility.FromJson<List<HighScoreEntry>>(jsonData);
        }
    }

    private void SaveHighScores()
    {
        string jsonData = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(HighScoreKey, jsonData);
        PlayerPrefs.Save();
    }
}
//