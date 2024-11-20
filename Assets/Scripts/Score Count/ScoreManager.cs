using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static float FinalDistance { get; private set; }
    public static int FinalScore { get; private set; }
    
    [Header("Score Settings")]
    [SerializeField] private float metersPerUnit = 1f; // meters per Unity unit
    [SerializeField] private int pointsPerEnemy = 100; // Points awarded for each enemy avoided
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI distanceText; 
    [SerializeField] private TextMeshProUGUI scoreText; 
    
    private float distanceTraveled;
    private int enemiesAvoided;
    private int totalScore;
    private Transform playerTransform;
    private float lastPlayerXPosition;
    
    // Singleton instance to access from other scripts
    public static ScoreManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

     // Store final scores before death
    public void StoreFinalScores()
    {
        FinalDistance = distanceTraveled;
        FinalScore = totalScore;
        
        // Mark that the game has been played
        PlayerPrefs.SetInt("HasPlayed", 1);
        
        // Update high score if better than previous
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (totalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
        }
        
        // Update best distance if better than previous
        float currentHighDistance = PlayerPrefs.GetFloat("HighDistance", 0f);
        if (distanceTraveled > currentHighDistance)
        {
            PlayerPrefs.SetFloat("HighDistance", distanceTraveled);
        }
        
        PlayerPrefs.Save();
    }
    private void Start()
    {
        // reference to player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerXPosition = playerTransform.position.x;
        UpdateUI();
    }
    
    private void Update()
    {
        if (playerTransform != null)
        {
            // Calculate distance traveled since last frame
            float deltaDistance = playerTransform.position.x - lastPlayerXPosition;
            if (deltaDistance > 0) // Only count forward movement
            {
                distanceTraveled += deltaDistance * metersPerUnit;
                UpdateScore();
            }
            lastPlayerXPosition = playerTransform.position.x;
        }
    }
    
    public void AddEnemyAvoided()
    {
        enemiesAvoided++;
        UpdateScore();
    }
    
    private void UpdateScore()
    {
        // Calculate total score: distance (in meters) + (enemies avoided × points per enemy)
        totalScore = Mathf.RoundToInt(distanceTraveled) + (enemiesAvoided * pointsPerEnemy);
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        if (distanceText != null)
        {
            distanceText.text = $"Distance: {Mathf.RoundToInt(distanceTraveled)}m";
        }
        
        if (scoreText != null)
        {
            scoreText.text = $"Score: {totalScore}";
        }
    }
    
    // Public getters for other scripts
    public float GetDistance() => distanceTraveled;
    public int GetEnemiesAvoided() => enemiesAvoided;
    public int GetTotalScore() => totalScore;
    
    public void ResetScores()
    {
        distanceTraveled = 0f;
        enemiesAvoided = 0;
        totalScore = 0;
        lastPlayerXPosition = playerTransform.position.x;
        UpdateUI();
    }
}