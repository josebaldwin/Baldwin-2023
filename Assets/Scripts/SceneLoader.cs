using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Function to load the MenuScene
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene"); // Replace "MainScene" with the exact name of your main scene
    }
}
