using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController2 : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public GameObject tutorialPanel;
    public GameObject pausePanel;
    public GameObject victoryPanel;

    public SimpleSpawner spawner;


    public int score;
    public int targetScore = 20;


    public TMP_Text scoreText;
    public TMP_Text timeText;

    public float gameTime = 30f;
    float timeLeft;


    bool playing;

    void Start()
    {
        tutorialPanel.SetActive(true);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
        Time.timeScale = 0f;

        timeLeft = gameTime;
        UpdateUI();
    }

    void Update()
    {
        if (!playing) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            WinGame(); 
        }

        UpdateUI();
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        playing = true;

        spawner.StartSpawning();
    }

    public void AddScore(int value)
    {
        if (!playing) return;

        score += value;

        if (score < 0)
            score = 0;

        UpdateUI();

        if (score >= targetScore)
        {
            WinGame();
        }
    }


    void WinGame()
    {
        playing = false;
        Time.timeScale = 0f;

        finalScoreText.text = "PUNTAJE: " + score;

        victoryPanel.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }


    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicial");
    }

    void UpdateUI()
    {
        scoreText.text = "Puntaje: " + score;
        timeText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

}
