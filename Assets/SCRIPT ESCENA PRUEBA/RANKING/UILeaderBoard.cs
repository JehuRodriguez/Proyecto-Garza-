using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILeaderBoard : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject nameInputPanel;
    public TMP_InputField nameInputField;
    public Button btnStartGame;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverTitle;
    public Button btnViewScore;
    public Button btnPlayAgain;

    public GameObject leaderboardPanel;
    public Transform leaderboardContent;
    public GameObject leaderboardRowPrefab;

    public Transform myAttemptsContent;
    public GameObject attemptRowPrefab;

    void Start()
    {
        if (btnStartGame != null) btnStartGame.onClick.AddListener(OnStartClicked);
        if (btnViewScore != null) btnViewScore.onClick.AddListener(OpenLeaderboardPanel);
        if (btnPlayAgain != null) btnPlayAgain.onClick.AddListener(OnPlayAgainClicked);

        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);


        if (PlayerProfile.Instance != null && !string.IsNullOrEmpty(PlayerProfile.Instance.playerName))
        {
            if (nameInputPanel != null) nameInputPanel.SetActive(false);
        }
    }

    public void ShowGameOver(bool won)
    {
        if (gameOverPanel == null) return;
        gameOverPanel.SetActive(true);
        gameOverTitle.text = won ? "¡GANASTE!" : "PERDISTE";

        if (won)
        {
            if (btnViewScore != null) btnViewScore.gameObject.SetActive(true);
            if (btnPlayAgain != null) btnPlayAgain.gameObject.SetActive(true);
        }
        else
        {
            if (btnPlayAgain != null) btnPlayAgain.gameObject.SetActive(true);
            if (btnViewScore != null) btnViewScore.gameObject.SetActive(true); 
        }
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void OnStartClicked()
    {
        string name = nameInputField != null ? nameInputField.text.Trim() : "";
        if (string.IsNullOrEmpty(name)) return;
        PlayerProfile.Instance.SetName(name);
        if (nameInputPanel != null) nameInputPanel.SetActive(false);
    }

    public void OnPlayAgainClicked()
    {
        
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void OpenLeaderboardPanel()
    {
        RefreshLeaderboardUI();
        if (leaderboardPanel != null) leaderboardPanel.SetActive(true);
    }

    public void CloseLeaderboardPanel()
    {
        if (leaderboardPanel != null) leaderboardPanel.SetActive(false);
    }

    public void RefreshLeaderboardUI()
    {
        foreach (Transform t in leaderboardContent) Destroy(t.gameObject);
        foreach (Transform t in myAttemptsContent) Destroy(t.gameObject);

        
        var list = LeaderBoardManager.Instance.GetGlobalBestList();
        foreach (var item in list)
        {
            GameObject row = Instantiate(leaderboardRowPrefab, leaderboardContent);
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();
            
            if (texts.Length >= 2)
            {
                texts[0].text = item.name;
                texts[1].text = item.best.ToString();
            }
        }

        string me = PlayerProfile.Instance != null ? PlayerProfile.Instance.playerName : "";
        var attempts = LeaderBoardManager.Instance.GetAttempts(me);

        for (int i = 0; i < attempts.Count; i++)
        {
            GameObject row = Instantiate(attemptRowPrefab, myAttemptsContent);
            TextMeshProUGUI t = row.GetComponentInChildren<TextMeshProUGUI>();
            if (t != null) t.text = $"Intento {i + 1}: {attempts[i]}";
        }
    }

}
