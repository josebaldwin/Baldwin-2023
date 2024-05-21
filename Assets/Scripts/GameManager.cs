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
    public TMP_InputField playerNameInput;
    public Button saveNameButton;
    public Button restartButton;
    public TMP_Text highScoreText;

    public static event Action OnRestartRequested = delegate { };

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
        OnRestartRequested += ReloadScene;
        Debug.Log("GameManager Awake: Event listeners added.");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnRestartRequested -= ReloadScene;
        Debug.Log("GameManager OnDestroy: Event listeners removed.");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager OnSceneLoaded: Scene loaded: " + scene.name);
        if (scene.name == "MainScene") // Replace "MainScene" with the name of your main game scene
        {
            AssignUIComponents();
            Debug.Log("UI components assigned for MainScene.");
        }
    }

    void AssignUIComponents()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas)
        {
            Debug.Log("Canvas found in the scene.");
            gameOverPanel = canvas.transform.Find("GameOver").gameObject;
            if (gameOverPanel)
            {
                Debug.Log("GameOver panel assigned.");
                gameOverPanel.SetActive(false);
                restartButton = gameOverPanel.transform.Find("RestartButton").GetComponent<Button>();
                if (restartButton)
                {
                    restartButton.onClick.RemoveAllListeners();
                    restartButton.onClick.AddListener(() => OnRestartRequested());
                    Debug.Log("Restart button listener reattached.");
                }
                else
                {
                    Debug.LogError("Restart button not found.");
                }

                saveNameButton = gameOverPanel.transform.Find("SaveNameButton").GetComponent<Button>();
                if (saveNameButton)
                {
                    saveNameButton.onClick.RemoveAllListeners();
                    saveNameButton.onClick.AddListener(SavePlayerName);
                    Debug.Log("SaveNameButton listener reattached.");
                }
                else
                {
                    Debug.LogError("SaveNameButton not found.");
                }

                scoreText = gameOverPanel.transform.Find("FinalScoreText").GetComponent<TMP_Text>();
                if (scoreText == null)
                {
                    Debug.LogError("FinalScoreText not found.");
                }

                playerNameInput = gameOverPanel.transform.Find("PlayerNameInput").GetComponent<TMP_InputField>();
                if (playerNameInput == null)
                {
                    Debug.LogError("PlayerNameInput not found.");
                }

                highScoreText = gameOverPanel.transform.Find("HighScores").GetComponent<TMP_Text>();
                if (highScoreText == null)
                {
                    Debug.LogError("HighScores not found.");
                }
            }
            else
            {
                Debug.LogError("GameOver panel not found.");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene.");
        }
    }

    public void GameOver()
    {
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
            DisplayScore();
            UpdateHighScoreDisplay();
            Debug.Log("GameOver: High score display updated.");
        }
    }

    private void DisplayScore()
    {
        if (scoreText && ScoreManager.Instance)
            scoreText.text = "Final Score: " + ScoreManager.Instance.TotalScore.ToString();
        else
            Debug.LogError("Score Text component or ScoreManager is null.");
    }

    private void SavePlayerName()
    {
        string playerName = string.IsNullOrEmpty(playerNameInput.text) ? "Player" : playerNameInput.text.Trim();
        highScoreManager.AddHighScore(playerName, ScoreManager.Instance.TotalScore);
        playerNameInput.text = "";  // Clear input field after saving
        UpdateHighScoreDisplay();  // Refresh high score display
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.ResetScore();
        }
        Debug.Log("Scene reloaded and score reset.");
    }

    private void UpdateHighScoreDisplay()
    {
        if (highScoreText)
        {
            highScoreText.text = "High Scores:\n";
            foreach (var entry in highScoreManager.GetHighScores())
            {
                highScoreText.text += $"{entry.playerName}: {entry.score}\n";
            }
            Debug.Log("High score display updated.");
        }
        else
        {
            Debug.LogError("High Score Text component is not found.");
        }
    }
}
