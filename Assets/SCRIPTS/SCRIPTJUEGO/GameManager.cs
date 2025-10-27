using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Tiempo y puntaje")]
    public float maxTime = 60f;
    public int enemyAddSeconds = 3;
    public int allySubtractSeconds = 5;
    public bool clampToMax = true;

    [Header("Intentos")]
    public int totalAttempts = 3;

    [Header("UI (TMP)")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject roundOverPanel; 
    public TextMeshProUGUI roundOverText; 
    public GameObject finalSummaryPanel; 
    public TextMeshProUGUI finalSummaryText;

    private float timer;
    private int score;
    private int attemptsLeft;
    private List<int> attemptsScores = new List<int>();
    private bool running = true;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartRound();
    }

    void Update()
    {
        if (!running) return;

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 0f;
            EndRound();
        }

        UpdateUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    void UpdateUI()
    {
        if (timerText != null) timerText.text = Mathf.CeilToInt(timer).ToString() + "s";
        if (scoreText != null) scoreText.text = "Puntaje: " + score;
    }

    public void StartRound()
    {
        if (attemptsLeft == 0 && attemptsScores.Count == 0) attemptsLeft = totalAttempts;

        timer = maxTime;
        score = 0;
        running = true;
        if (roundOverPanel != null) roundOverPanel.SetActive(false);
        if (finalSummaryPanel != null) finalSummaryPanel.SetActive(false);
    }

    public void OnEnemyTapped(int addScore)
    {
        score += addScore;

        timer += enemyAddSeconds;
        if (clampToMax && timer > maxTime) timer = maxTime;
    }

    public void OnAllyTapped()
    {
        timer -= allySubtractSeconds;
        if (timer < 0f) timer = 0f;
       
    }

    void EndRound()
    {
        if (!running) return;
        running = false;

        attemptsScores.Add(score);
        attemptsLeft--;
        var sp = FindObjectOfType<Spawner>();
        if (sp != null) sp.StopSpawner();

        if (roundOverPanel != null)
        {
            roundOverPanel.SetActive(true);
            if (roundOverText != null) roundOverText.text = $"Intento acabado.\nPuntaje: {score}\nIntentos restantes: {attemptsLeft}";
        }

        if (attemptsLeft <= 0) ShowFinalSummary();
    }

    public void PlayAgainRound()
    {
        if (attemptsLeft > 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
            ShowFinalSummary();
    }

    void ShowFinalSummary()
    {
        if (finalSummaryPanel == null) return;

        int total = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Resumen Final:");
        for (int i = 0; i < attemptsScores.Count; i++)
        {
            sb.AppendLine($"Intento {i + 1}: {attemptsScores[i]} pts");
            total += attemptsScores[i];
        }

        sb.AppendLine("");
        sb.AppendLine("Total: " + total + " pts");

        if (finalSummaryText != null) finalSummaryText.text = sb.ToString();
        finalSummaryPanel.SetActive(true);
    }

    public int GetTotalScoreAcrossAttempts()
    {
        int t = 0;
        foreach (var s in attemptsScores) t += s;
        return t;
    }

    public void ForceEndRound() { timer = 0f; }

}
