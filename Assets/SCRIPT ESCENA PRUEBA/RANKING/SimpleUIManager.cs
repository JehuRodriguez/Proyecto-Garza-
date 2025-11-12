using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleUIManager : MonoBehaviour
{
    public static SimpleUIManager Instance { get; private set; }

    [Header("UI Panel (single)")]
    public GameObject mainPanel;

    [Header("Sección Start (visible al inicio)")]
    public GameObject startSection;
    public TMP_InputField nameInputField;
    public Button btnStart;

    [Header("Sección GameOver (oculta al inicio)")]
    public GameObject gameOverSection;
    public TextMeshProUGUI gameOverTitle;
    public TextMeshProUGUI gameOverScoreText;  
    public Button btnViewScore;                
    public Button btnPlayAgain;
    public Button btnExitToMainMenu;

    [Header("Sección Leaderboard (oculta al inicio)")]
    public GameObject leaderboardSection;
    public Transform leaderboardContent;
    public GameObject rowPrefab;
    public Transform myAttemptsContent;
    public GameObject attemptRowPrefab;

    [Header("Settings")]
    public string mainMenuSceneName = "Menu Principal";
    public string gameSceneName = "JUEGO2";

    string currentPlayerName = "";

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    void Start()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        ShowStartSection();

        if (btnStart != null) btnStart.onClick.AddListener(OnStartClicked);
        if (btnViewScore != null)
        {
            btnViewScore.onClick.RemoveAllListeners();
            btnViewScore.onClick.AddListener(OnViewScoreClicked);
        }
        if (btnPlayAgain != null) btnPlayAgain.onClick.AddListener(OnPlayAgainClicked);
        if (btnExitToMainMenu != null) btnExitToMainMenu.onClick.AddListener(OnExitToMainMenuClicked);

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            currentPlayerName = PlayerPrefs.GetString("PlayerName");
            if (nameInputField != null) nameInputField.text = currentPlayerName;
        }

       
        if (gameOverSection != null) gameOverSection.SetActive(false);
        if (leaderboardSection != null) leaderboardSection.SetActive(false);
    }

    void OnStartClicked()
    {
        string n = nameInputField != null ? nameInputField.text.Trim() : "";
        if (string.IsNullOrEmpty(n)) return;
        currentPlayerName = n;
        PlayerPrefs.SetString("PlayerName", currentPlayerName);
        PlayerPrefs.Save();
        ShowGamePlayState();
    }

    void ShowStartSection()
    {
        if (startSection != null) startSection.SetActive(true);
        if (gameOverSection != null) gameOverSection.SetActive(false);
        if (leaderboardSection != null) leaderboardSection.SetActive(false);
    }

    void ShowGamePlayState()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
    }

    public void HandleGameOver(bool won, int finalScore)
    {
        if (!string.IsNullOrEmpty(currentPlayerName))
            MyGame.Profiles.SimpleManager.Instance?.AddAttempt(currentPlayerName, finalScore);

        if (mainPanel != null) mainPanel.SetActive(true);
        if (startSection != null) startSection.SetActive(false);
        if (leaderboardSection != null) leaderboardSection.SetActive(false);
        if (gameOverSection != null) gameOverSection.SetActive(true);

        if (gameOverTitle != null) gameOverTitle.text = won ? "¡GANASTE!" : "PERDISTE";

        if (btnViewScore != null) btnViewScore.gameObject.SetActive(true);

        if (won)
        {
            if (btnPlayAgain != null) btnPlayAgain.gameObject.SetActive(false);
            if (btnExitToMainMenu != null) btnExitToMainMenu.gameObject.SetActive(true);
        }
        else
        {
            if (btnPlayAgain != null) btnPlayAgain.gameObject.SetActive(true);
            if (btnExitToMainMenu != null) btnExitToMainMenu.gameObject.SetActive(false);
        }
    }

    public void OnViewScoreClicked()
    {
        RefreshLeaderboardUI();
        if (leaderboardSection != null) leaderboardSection.SetActive(true);
        if (gameOverSection != null) gameOverSection.SetActive(false);
    }

    public void OnPlayAgainClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OnExitToMainMenuClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RefreshLeaderboardUI()
    {
        if (leaderboardContent != null)
        {
            foreach (Transform t in leaderboardContent) Destroy(t.gameObject);
        }
        if (myAttemptsContent != null)
        {
            foreach (Transform t in myAttemptsContent) Destroy(t.gameObject);
        }

        var manager = MyGame.Profiles.SimpleManager.Instance;
        if (manager != null && leaderboardContent != null && rowPrefab != null)
        {
            var list = manager.GetGlobalBestList();
            foreach (var item in list)
            {
                GameObject row = Instantiate(rowPrefab, leaderboardContent);
            
                var nameTxt = row.transform.Find("TxtName")?.GetComponent<TextMeshProUGUI>();
                var scoreTxt = row.transform.Find("TxtScore")?.GetComponent<TextMeshProUGUI>();

                if (nameTxt != null) nameTxt.text = item.name;
                if (scoreTxt != null) scoreTxt.text = item.best.ToString();
            }
        }

        if (manager != null && myAttemptsContent != null && attemptRowPrefab != null)
        {
            string me = currentPlayerName;
            var attempts = manager.GetAttempts(me);
            for (int i = 0; i < attempts.Count; i++)
            {
                GameObject r = Instantiate(attemptRowPrefab, myAttemptsContent);
                var t = r.GetComponentInChildren<TextMeshProUGUI>();
                if (t != null) t.text = $"Intento {i + 1}: {attempts[i]}";
            }
        }
    }

}
