using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VICTORYMENUCONTROLLER : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainPanel;        
    public GameObject scorePanel;       
    public TextMeshProUGUI scoreText;   
    public TextMeshProUGUI lifeText;    
    public Button btnExit;
    public Button btnViewScore;
    public Button btnBackFromScore;
    public Button btnPlayAgain;

    void Start()
    {
        if (scorePanel != null) scorePanel.SetActive(false);

       
        if (btnExit != null) btnExit.onClick.AddListener(OnExit);
        if (btnViewScore != null) btnViewScore.onClick.AddListener(OnViewScore);
        if (btnBackFromScore != null) btnBackFromScore.onClick.AddListener(OnBackFromScore);
        if (btnPlayAgain != null) btnPlayAgain.onClick.AddListener(OnPlayAgain);
    }

    void OnViewScore()
    {
        
        float life = PlayerPrefs.GetFloat("LastPlayerLife", 0f);
        int score = CalculateScore(life);
        if (scoreText != null) scoreText.text = $"Puntaje: {score}";
        if (lifeText != null) lifeText.text = $"Vida final: {Mathf.RoundToInt(life)}%";
        if (mainPanel != null) mainPanel.SetActive(false);
        if (scorePanel != null) scorePanel.SetActive(true);
    }

    void OnBackFromScore()
    {
        if (scorePanel != null) scorePanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnPlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    int CalculateScore(float life)
    {
       
        if (life >= 100f) return 1000;
        if (life >= 90f) return 800;
        if (life >= 75f) return 600;
        if (life >= 50f) return 350;
        return Mathf.RoundToInt(life * 3f);
    }
}
