using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;           
    public Button btnContinue;              
    public Button btnMainMenu;              
    public Button btnExit;

    [Header("Main menu scene (name)")]
    public string mainMenuSceneName = "MenuPrincipal"; 

    bool isPaused = false;
    float previousTimeScale = 1f;

    void Start()
    {
       
        if (pausePanel != null) pausePanel.SetActive(false);

       
        if (btnContinue != null) btnContinue.onClick.AddListener(Resume);
        if (btnMainMenu != null) btnMainMenu.onClick.AddListener(GoToMainMenu);
        if (btnExit != null) btnExit.onClick.AddListener(ExitGame);

    
        previousTimeScale = Time.timeScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;

        
        if (pausePanel != null) pausePanel.SetActive(true);

        
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

     
        AudioListener.pause = true;

     
        if (TimeManager.Instance != null) TimeManager.Instance.StopTimer();
    }

    public void Resume()
    {
        if (!isPaused) return;
        isPaused = false;

        
        if (pausePanel != null) pausePanel.SetActive(false);

       
        Time.timeScale = previousTimeScale == 0f ? 1f : previousTimeScale;
        AudioListener.pause = false;

        
        if (TimeManager.Instance != null) TimeManager.Instance.StartTimer();
    }

    public void GoToMainMenu()
    {
        
        Time.timeScale = 1f;
        AudioListener.pause = false;

        
        if (!string.IsNullOrEmpty(mainMenuSceneName))
            SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ExitGame()
    {
        
        Time.timeScale = 1f;
        AudioListener.pause = false;

#if UNITY_EDITOR
      
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }




}
