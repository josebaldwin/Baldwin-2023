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

        // Add listeners to buttons
        AddButtonListeners();
    }

    void AddButtonListeners()
    {
        startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClicked);
        creditsButton.GetComponent<Button>().onClick.AddListener(OnCreditsButtonClicked);
        backButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        PlayButtonClickSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void OnCreditsButtonClicked()
    {
        PlayButtonClickSound();
        creditsPanel.SetActive(true);
        creditsScrollView.SetActive(true); // Enable the scroll view
        startButton.SetActive(false);
        creditsButton.SetActive(false);
        backButton.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        PlayButtonClickSound();
        creditsPanel.SetActive(false);
        creditsScrollView.SetActive(false); // Disable the scroll view
        startButton.SetActive(true);
        creditsButton.SetActive(true);
        backButton.SetActive(false);
    }

    void PlayButtonClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSound();
        }
    }
}
