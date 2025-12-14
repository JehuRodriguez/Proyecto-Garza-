using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text titleText;           
    public Button playButton;           
    public Button creditsButton;        
    public GameObject creditsPanel;

    [Header("Scene")]
    public string gameSceneName = "GameScene"; 

    [Header("Audio (optional)")]
    public AudioClip backgroundMusic;  
    [Range(0f, 1f)] public float musicVolume = 0.25f;
    public bool playMusicOnAwake = true;

    AudioSource audioSource;

    void Awake()
    {
       
        if (titleText == null) Debug.LogWarning("MenuManager: titleText no asignado.");
        if (playButton == null) Debug.LogWarning("MenuManager: playButton no asignado.");
        if (creditsButton == null) Debug.LogWarning("MenuManager: creditsButton no asignado.");

        
        if (backgroundMusic != null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = musicVolume;
            if (playMusicOnAwake) audioSource.Play();
        }

       
        if (playButton != null) playButton.onClick.AddListener(OnPlayClicked);
        if (creditsButton != null) creditsButton.onClick.AddListener(OnCreditsClicked);

     
        if (creditsPanel != null) creditsPanel.SetActive(false);

       
        if (titleText != null && string.IsNullOrWhiteSpace(titleText.text))
        {
            titleText.text = "RITMO HUMEDAL";
        }
    }

    void OnDestroy()
    {
        
        if (playButton != null) playButton.onClick.RemoveListener(OnPlayClicked);
        if (creditsButton != null) creditsButton.onClick.RemoveListener(OnCreditsClicked);
    }

    public void OnPlayClicked()
    {
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();

        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("MenuManager: gameSceneName está vacío. Asigna el nombre de la escena en el Inspector y agrégala a Build Settings.");
            return;
        }

        
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnCreditsClicked()
    {
        if (creditsPanel == null)
        {
            Debug.Log("MenuManager: creditsPanel no asignado. Puedes usar este botón para abrir un panel o escena.");
            return;
        }
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }


    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }



    public void SetTitle(string text)
    {
        if (titleText != null) titleText.text = text;
    }

}
