using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float gameDuration = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    float timeRemaining;
    int score = 0;
    bool gameActive = true;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = gameDuration;
        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!gameActive) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            gameActive = false;
            timerText.text = "Time: 0";
            ShowGameOver();
        }
        else
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
        }
    }

    public void AddScore()
    {
        if (!gameActive) return;
        score++;
        scoreText.text = "Deliveries: " + score;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Deliveries completed: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
        scoreText.text = "Deliveries: " + score;
    }
}