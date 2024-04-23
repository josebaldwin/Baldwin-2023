using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make sure UIManager persists across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // Ensures there is only one instance of the UIManager
        }
    }
}
