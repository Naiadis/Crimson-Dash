using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highDistanceText;
    [SerializeField] private string gameSceneName = "DarkForest";
    [SerializeField] private float transitionDelay = 1f;

    private void Start()
    {
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
        StartCoroutine(LoadGameWithDelay());
    }

    private IEnumerator LoadGameWithDelay()
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(gameSceneName);
    }
}