using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public HighScoreManager highScoreManager;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;

    public static event Action OnRestartRequested = delegate { };  // Static event for restart

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        OnRestartRequested += ReloadScene;  // Subscribe to the static event
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnRestartRequested -= ReloadScene;  // Unsubscribe to prevent memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIComponents();
    }

    void AssignUIComponents()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas)
        {
            gameOverPanel = canvas.transform.Find("GameOver").gameObject;
            if (gameOverPanel)
            {
                gameOverPanel.SetActive(false);
                Button restartButton = gameOverPanel.GetComponentInChildren<Button>(true);
                if (restartButton)
                {
                    restartButton.onClick.RemoveAllListeners();
                    restartButton.onClick.AddListener(() => OnRestartRequested());  // Use static event to trigger
                }
                scoreText = gameOverPanel.transform.Find("FinalScoreText").GetComponent<TMP_Text>();
            }
        }
    }

    public void GameOver()
    {
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            DisplayScore();
        }
    }

    private void DisplayScore()
    {
        if (scoreText && ScoreManager.Instance)
            scoreText.text = "Final Score: " + ScoreManager.Instance.TotalScore.ToString();
        else
            Debug.LogError("Score Text component is null or ScoreManager is not available when trying to display score.");
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.ResetScore();
        }
    }
}
