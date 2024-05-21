using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject startButton;
    public GameObject creditsButton;
    public GameObject backButton;
    public GameObject creditsScrollView; // Reference to the Scroll View

    void Start()
    {
        creditsPanel.SetActive(false);
        backButton.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void OnCreditsButtonClicked()
    {
        creditsPanel.SetActive(true);
        creditsScrollView.SetActive(true); // Enable the scroll view
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        creditsPanel.SetActive(false);
        creditsScrollView.SetActive(false); // Disable the scroll view
        startButton.SetActive(true);
        creditsButton.SetActive(true);
        backButton.SetActive(false);
    }
}
