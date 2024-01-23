using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }


    public int TotalScore { get; private set; }
    public Material firstThresholdSkybox;  // Assign the first threshold skybox material in the inspector
    public Material secondThresholdSkybox; // Assign the second threshold skybox material in the inspector
    public int firstScoreThreshold = 100;  // First score threshold to change the skybox
    public int secondScoreThreshold = 200; // Second score threshold to change the skybox

    private bool firstThresholdReached = false;
    private bool secondThresholdReached = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        TotalScore += score;
        Debug.Log("Total Score: " + TotalScore);

        if (TotalScore >= firstScoreThreshold && !firstThresholdReached)
        {
            ChangeSkybox(firstThresholdSkybox);
            firstThresholdReached = true;
        }
        else if (TotalScore >= secondScoreThreshold && !secondThresholdReached)
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
}