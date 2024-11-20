using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private string gameSceneName = "DarkForest";
    [SerializeField] private string menuSceneName = "MainMenu";

    private void Start()
    {
        if (finalDistanceText != null)
            finalDistanceText.text = $"Distance: {Mathf.RoundToInt(ScoreManager.FinalDistance)}m";
        
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {ScoreManager.FinalScore}";
    }

    public void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnMainMenuClicked()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}