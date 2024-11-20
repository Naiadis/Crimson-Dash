using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject highScorePanel;  // Parent panel/object containing score texts
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highDistanceText;
    [SerializeField] private string gameSceneName = "DarkForest";

    private void Start()
    {
        // Check if there's a recorded high score
        if (PlayerPrefs.HasKey("HasPlayed"))
        {
            if (highScorePanel != null)
                highScorePanel.SetActive(true);
                
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            float highDistance = PlayerPrefs.GetFloat("HighDistance", 0f);
            
            if (highScoreText != null)
                highScoreText.text = $"High Score: {highScore}";
            
            if (highDistanceText != null)
                highDistanceText.text = $"Best Distance: {Mathf.RoundToInt(highDistance)}m";
        }
        else if (highScorePanel != null)
        {
            highScorePanel.SetActive(false);
        }
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}