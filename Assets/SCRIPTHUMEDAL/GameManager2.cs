using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance;

    public int score = 0;
    public int lives = 3;
    public float roundTime = 20f;

    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text timerText;
    public GameObject endPanel;
    public TMP_Text endResultText;

    public Spawner2 spawner;

    float timeLeft;
    bool playing = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {
        ShowMainMenu(); 
    }

    public void StartGame()
    {
        score = 0;
        lives = 3;
        timeLeft = roundTime;
        playing = true;
        UpdateHUD();
        endPanel.SetActive(false);
        spawner.StartRound();
    }

    void Update()
    {
        if (!playing) return;
        timeLeft -= Time.deltaTime;
        UpdateHUD();
        if (timeLeft <= 0f)
        {
            playing = false;
            spawner.StopRound();
            OnRoundFinished();
        }
    }

    public void AddScore(int v)
    {
        score += v;
        Debug.Log("AddScore called. new score = " + score);
        UpdateHUD();
    }

    public void LoseLife(int n)
    {
        lives -= n;
        UpdateHUD();
        if (lives <= 0)
        {
            playing = false;
            spawner.StopRound();
            OnRoundFinished();
        }
    }

    void UpdateHUD()
    {
        Debug.Log("UpdateHUD: scoreText assigned? " + (scoreText != null));
        if (scoreText) scoreText.text = "Score: " + score;
        if (livesText) livesText.text = "Lives: " + lives;
        if (timerText) timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    public void OnRoundFinished()
    {
        playing = false;
        endPanel.SetActive(true);
        string result = lives > 0 ? "¡GANASTE!" : "¡PERDISTE!";
        endResultText.text = result + "\nScore: " + score;
        
        int best = PlayerPrefs.GetInt("bestScore", 0);
        if (score > best) { PlayerPrefs.SetInt("bestScore", score); }
    }

    public void ShowMainMenu()
    {
        
        StartGame();
    }

    public void PlayAgain()
    {
        StartGame();
    }



}
