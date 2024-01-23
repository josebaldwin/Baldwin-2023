using UnityEngine;
using TMPro;  // Namespace for TextMeshPro

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.TotalScore.ToString();
        }
    }
}
