using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        UpdateScore(); // Ensure the score is updated when the script is enabled
        // Optionally subscribe to a score changed event if ScoreManager were to implement one
    }

    void Update()
    {
        UpdateScore(); // Continuously update the score each frame
    }

    private void UpdateScore()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.TotalScore.ToString();
        }
    }
}
