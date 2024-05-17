using UnityEngine;

public class Utilities : MonoBehaviour
{
    [ContextMenu("Reset High Scores")]
    public void ResetHighScores()
    {
        PlayerPrefs.DeleteKey("HighScores");
        PlayerPrefs.Save();
        Debug.Log("High scores reset.");
    }
}
