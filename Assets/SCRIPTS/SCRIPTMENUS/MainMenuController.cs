using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Nombres de Escenas")]
    [Tooltip("Escribe exactamente el nombre de la escena del juego")]
    public string gameSceneName = "GameScene";

    [Tooltip("Escribe exactamente el nombre de la escena de créditos")]
    public string creditsSceneName = "CreditsScene";

    [Header("Botones del Menú")]
    public Button playButton;
    public Button creditsButton;
    public Button quitButton;

    void Start()
    {
       
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (creditsButton != null)
            creditsButton.onClick.AddListener(OpenCredits);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning(" No se asignó un nombre de escena para el juego.");
        }
    }

    public void OpenCredits()
    {
        if (!string.IsNullOrEmpty(creditsSceneName))
        {
            SceneManager.LoadScene(creditsSceneName);
        }
        else
        {
            Debug.LogWarning(" No se asignó un nombre de escena para créditos.");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); // Cierra la aplicación compilada
#endif
    }




}
