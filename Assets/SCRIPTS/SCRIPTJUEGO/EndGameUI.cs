using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndGameUI : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button saveButton;
    public Transform leaderboardContent; 
    public GameObject scoreRowPrefab; 
    public GameObject finalSummaryPanel; 
    public GameManager gm;

    void Start()
    {
        if (saveButton != null)
            saveButton.onClick.AddListener(OnSaveClicked);

        if (gm == null) gm = FindObjectOfType<GameManager>();
    }

    void OnSaveClicked()
    {
        string playerName = string.IsNullOrEmpty(nameInput.text) ? "ANONIMO" : nameInput.text;
        int total = gm != null ? gm.GetTotalScoreAcrossAttempts() : 0;

        LeaderboardManager.Instance.AddEntry(playerName, total);

        RefreshLeaderboard();

        if (finalSummaryPanel != null) finalSummaryPanel.SetActive(false);
    }

    public void RefreshLeaderboard()
    {
        if (leaderboardContent == null || scoreRowPrefab == null) return;

        foreach (Transform t in leaderboardContent) Destroy(t.gameObject);

        var list = LeaderboardManager.Instance.GetTop(20);
        foreach (var e in list)
        {
            var go = Instantiate(scoreRowPrefab, leaderboardContent);
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = $"{e.name} — {e.score}";
        }

    }

}
