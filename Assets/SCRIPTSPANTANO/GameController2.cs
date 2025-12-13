using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController2 : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject pausePanel;
    public GameObject victoryPanel;

    public int score;
    public int targetScore = 20;


    bool playing;

    void Start()
    {
        tutorialPanel.SetActive(true);
        pausePanel.SetActive(false);
        victoryPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
        playing = true;
    }

    public void AddScore(int value)
    {
        if (!playing) return;

        score += value;
        if (score >= targetScore)
        {
            WinGame();
        }
    }


    void WinGame()
    {
        playing = false;
        Time.timeScale = 0f;
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
        SceneManager.LoadScene("MainMenu");
    }

}
