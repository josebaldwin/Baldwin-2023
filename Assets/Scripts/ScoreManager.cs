using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int TotalScore { get; private set; }
    public Material firstThresholdSkybox;  // Assign the first threshold skybox material in the inspector
    public Material secondThresholdSkybox; // Assign the second threshold skybox material in the inspector
    public Material defaultSkybox;         // Assign a default skybox material for resetting
    public int firstScoreThreshold = 200;  // First score threshold to change the skybox
    public int secondScoreThreshold = 500; // Second score threshold to change the skybox

    private bool firstThresholdReached = false;
    private bool secondThresholdReached = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // Ensure no duplicate managers exist
        }
    }


    public void AddScore(int score)
    {
        TotalScore += score;
        CheckScoreThresholds();
    }

    private void CheckScoreThresholds()
    {
        if (TotalScore >= firstScoreThreshold && !firstThresholdReached)
        {
            ChangeSkybox(firstThresholdSkybox);
            firstThresholdReached = true;
        }
        if (TotalScore >= secondScoreThreshold && !secondThresholdReached)
        {
            ChangeSkybox(secondThresholdSkybox);
            secondThresholdReached = true;
        }
    }

    private void ChangeSkybox(Material newSkyboxMaterial)
    {
        if (newSkyboxMaterial != null)
        {
            RenderSettings.skybox = newSkyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogError("New skybox material not assigned in ScoreManager.");
        }
    }

    public void ResetScore()
    {
        TotalScore = 0;
        firstThresholdReached = false;
        secondThresholdReached = false;
        // Reset skybox to default when the score is reset
        if (defaultSkybox != null)
        {
            RenderSettings.skybox = defaultSkybox;
        }
        else
        {
            Debug.LogError("Default skybox material not assigned in ScoreManager.");
        }
        DynamicGI.UpdateEnvironment();
        // Notify any listeners that the score has been reset if necessary
    }
}
