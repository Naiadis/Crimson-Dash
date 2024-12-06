using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI finalDistanceText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private string gameSceneName = "DarkForest";
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private float transitionDelay = 1f;  // Added delay parameter

    private void Start()
    {
        if (finalDistanceText != null)
            finalDistanceText.text = $"Distance: {Mathf.RoundToInt(ScoreManager.FinalDistance)}m";
        
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {ScoreManager.FinalScore}";
    }

    public void OnPlayAgainClicked()
    {
        StartCoroutine(LoadSceneWithDelay(gameSceneName));
    }

    public void OnMainMenuClicked()
    {
        StartCoroutine(LoadSceneWithDelay(menuSceneName));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneName);
    }
}