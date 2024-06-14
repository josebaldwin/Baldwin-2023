using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public HighScoreManager highScoreManager;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_InputField playerNameInput;
    public Button saveNameButton;
    public Button restartButton;
    public TMP_Text highScoreText;
    public static bool isGameOver = false;


    public static event Action OnRestartRequested = delegate { };

    private bool highScoreAdded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnRestartRequested += ReloadScene;
            Debug.Log("GameManager Awake: Event listeners added.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            OnRestartRequested -= ReloadScene;
            Debug.Log("GameManager OnDestroy: Event listeners removed.");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene") // Ensure this code only runs for the MainScene
        {
            AssignUIComponents();
            highScoreAdded = false;
            Debug.Log($"GameManager OnSceneLoaded: UI components assigned. Scene: {scene.name}");
        }
    }

    void AssignUIComponents()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas)
        {
            gameOverPanel = canvas.transform.Find("GameOver")?.gameObject;
            if (gameOverPanel)
            {
                gameOverPanel.SetActive(false);
                Debug.Log("GameOver panel assigned.");

                restartButton = gameOverPanel.transform.Find("RestartButton")?.GetComponent<Button>();
                if (restartButton)
                {
                    restartButton.onClick.RemoveAllListeners();
                    restartButton.onClick.AddListener(() => OnRestartRequested());
                    Debug.Log("Restart button listener reattached.");
                }

                saveNameButton = gameOverPanel.transform.Find("SaveNameButton")?.GetComponent<Button>();
                playerNameInput = gameOverPanel.transform.Find("PlayerNameInput")?.GetComponent<TMP_InputField>();
                scoreText = gameOverPanel.transform.Find("FinalScoreText")?.GetComponent<TMP_Text>();
                highScoreText = gameOverPanel.transform.Find("HighScores")?.GetComponent<TMP_Text>();

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

                if (!playerNameInput || !saveNameButton || !scoreText || !highScoreText)
                {
                    Debug.LogError("UI components are not properly assigned.");
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

            bool isHighScore = highScoreManager.IsHighScore(ScoreManager.Instance.TotalScore);
            if (isHighScore && !highScoreAdded)
            {
                playerNameInput.gameObject.SetActive(true);
                saveNameButton.gameObject.SetActive(true);
            }
            else
            {
                playerNameInput.gameObject.SetActive(false);
                saveNameButton.gameObject.SetActive(false);
            }

            UpdateHighScoreDisplay();
            Debug.Log("GameOver: High score display updated.");

            // Set the game over flag
            isGameOver = true;

            // Stop all enemy shooting sounds
            AudioManager.Instance.StopAllEnemyShootingSounds();
        }
    }

    private void DisplayScore()
    {
        if (scoreText && ScoreManager.Instance)
        {
            scoreText.text = "Final Score: " + ScoreManager.Instance.TotalScore.ToString();
        }
        else
        {
            Debug.LogError("Score Text component or ScoreManager is null.");
        }
    }

    private void SavePlayerName()
    {
        if (highScoreAdded) return;

        string playerName = string.IsNullOrEmpty(playerNameInput.text) ? "Player" : playerNameInput.text.Trim();
        highScoreManager.AddHighScore(playerName, ScoreManager.Instance.TotalScore);
        playerNameInput.text = "";

        highScoreAdded = true;
        playerNameInput.gameObject.SetActive(false);
        saveNameButton.gameObject.SetActive(false);
        UpdateHighScoreDisplay();
        Debug.Log($"Saved {playerName} with score {ScoreManager.Instance.TotalScore}");
    }

    private void ReloadScene()
    {
        highScoreAdded = false;  // Reset high score added flag
        isGameOver = false; // Reset the game over flag
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (ScoreManager.Instance)
        {
            ScoreManager.Instance.ResetScore();
        }
        Debug.Log($"Scene reloaded and score reset. Scene: {SceneManager.GetActiveScene().name}");
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
