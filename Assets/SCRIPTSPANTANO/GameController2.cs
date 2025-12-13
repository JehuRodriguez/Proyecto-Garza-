using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController2 : MonoBehaviour
{
    public static GameController2 Instance;

    public int score;
    public int lives = 3;
    public float gameTime = 30f;

    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text livesText;


    public GameObject restartButton;
    public GameObject tutorialPanel;
    public SimpleSpawner spawner;

    float timeLeft;
    bool playing = false;


    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        timeLeft = gameTime;
        UpdateUI();
        restartButton.SetActive(false);

        if (PlayerPrefs.GetInt("TutorialSeen", 0) == 0)
        {
            tutorialPanel.SetActive(true);
            playing = false;
        }

        else
        {
            StartGame();
        }
    }

    void Update()
    {
        if (!playing) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            EndGame();
        }

        UpdateUI();
    }

    public void StartGame()
    {
        score = 0;
        lives = 3;
        timeLeft = gameTime;
        playing = true;
        restartButton.SetActive(false);
        spawner.StartSpawning();
        UpdateUI();
    }

    public void AddScore()
    {
        score++;
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
            EndGame();
    }

    void EndGame()
    {
        playing = false;
        spawner.StopSpawning();
        restartButton.SetActive(true);
    }


    public void RestartGame()
    {
        StartGame();
    }

    public void CloseTutorial()
    {
        PlayerPrefs.SetInt("TutorialSeen", 1);
        tutorialPanel.SetActive(false);
        StartGame();
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE: " + score;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();
        livesText.text = lives.ToString();
    }



}
