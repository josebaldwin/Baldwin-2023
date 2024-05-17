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
    public static HighScoreManager Instance { get; private set; }
    private List<HighScoreEntry> highScores = new List<HighScoreEntry>();
    private const string HighScoreKey = "HighScores";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsHighScore(int score)
    {
        return highScores.Count < 5 || score > highScores.Last().score; // Assuming top 5 scores
    }

    public void AddHighScore(string name, int score)
    {
        HighScoreEntry newEntry = new HighScoreEntry { playerName = name, score = score };
        highScores.Add(newEntry);
        highScores = highScores.OrderByDescending(x => x.score).ToList();

        if (highScores.Count > 5)  // Keep only the top 5
            highScores.RemoveRange(5, highScores.Count - 5);

        SaveHighScores();
        Debug.Log($"High score added: {name} with score {score}");
    }

    public List<HighScoreEntry> GetHighScores()
    {
        return highScores;
    }

    private void LoadHighScores()
    {
        string jsonData = PlayerPrefs.GetString(HighScoreKey, "");
        if (!string.IsNullOrEmpty(jsonData))
        {
            HighScoresListWrapper wrapper = JsonUtility.FromJson<HighScoresListWrapper>(jsonData);
            highScores = wrapper.highScores;
        }
    }

    [System.Serializable]
    public class HighScoresListWrapper
    {
        public List<HighScoreEntry> highScores;
    }

    private void SaveHighScores()
    {
        HighScoresListWrapper wrapper = new HighScoresListWrapper { highScores = highScores };
        string jsonData = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(HighScoreKey, jsonData);
        PlayerPrefs.Save();
        Debug.Log("High scores saved.");
    }
}
