using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
        Debug.Log("[SimpleUIManager] HandleGameOver called. won=" + won + " finalScore=" + finalScore + " currentPlayerName=" + currentPlayerName);

      
        string playerName = !string.IsNullOrEmpty(currentPlayerName)
            ? currentPlayerName
            : (PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : "");

        if (!string.IsNullOrEmpty(playerName))
        {
            var mgr = MyGame.Profiles.SimpleManager.Instance;
            if (mgr != null) mgr.AddAttempt(playerName, finalScore);
            else Debug.LogWarning("[SimpleUIManager] SimpleManager.Instance es null al guardar intento.");
        }
        else
        {
            Debug.LogWarning("[SimpleUIManager] No se tiene nombre de jugador al guardar intento.");
        }

        PlayerPrefs.SetInt("LastScore", finalScore);
        PlayerPrefs.Save();

        
        if (mainPanel != null) mainPanel.SetActive(true);
        if (startSection != null) startSection.SetActive(false);
        if (leaderboardSection != null) leaderboardSection.SetActive(false);
        if (gameOverSection != null) gameOverSection.SetActive(true);

        if (gameOverTitle != null) gameOverTitle.text = won ? "¡GANASTE!" : "PERDISTE";

        if (gameOverScoreText != null)
        {
            if (won)
            {
                gameOverScoreText.gameObject.SetActive(true);
                gameOverScoreText.text = $"Puntaje: {finalScore}";
            }
            else
            {
                gameOverScoreText.gameObject.SetActive(false);
            }
        }

        if (btnViewScore != null) btnViewScore.gameObject.SetActive(won);
        if (btnPlayAgain != null) btnPlayAgain.gameObject.SetActive(!won);
        if (btnExitToMainMenu != null) btnExitToMainMenu.gameObject.SetActive(true);

    }

    public void OnViewScoreClicked()
    {
        StartCoroutine(RefreshLeaderboardUICoroutine());
    }


    private IEnumerator RefreshLeaderboardUICoroutine()
    {
        yield return null;

        float wait = 0f;
        var manager = MyGame.Profiles.SimpleManager.Instance;
        while (manager == null && wait < 0.5f)
        {
            yield return null;
            wait += Time.deltaTime;
            manager = MyGame.Profiles.SimpleManager.Instance;
        }

        if (manager == null)
        {
            Debug.LogError("[SimpleUIManager] SimpleManager.Instance sigue siendo NULL después de esperar. Asegúrate de que exista y tenga DontDestroyOnLoad.");
        }

        RefreshLeaderboardUI();
        if (leaderboardContent != null)
        {
            var layout = leaderboardContent.GetComponent<RectTransform>();
            if (layout != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
            }
        }

        if (myAttemptsContent != null)
        {
            var layout2 = myAttemptsContent.GetComponent<RectTransform>();
            if (layout2 != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout2);
            }
        }

        if (leaderboardSection != null) leaderboardSection.SetActive(true);
        if (gameOverSection != null) gameOverSection.SetActive(false);

        yield break;
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
        Debug.Log("[SimpleUIManager] RefreshLeaderboardUI called. currentPlayerName=" + currentPlayerName);

        if (leaderboardContent != null)
        {
            foreach (Transform t in leaderboardContent) Destroy(t.gameObject);
        }
        if (myAttemptsContent != null)
        {
            foreach (Transform t in myAttemptsContent) Destroy(t.gameObject);
        }

        var manager = MyGame.Profiles.SimpleManager.Instance;
        if (manager == null)
        {
            Debug.LogError("[SimpleUIManager] SimpleManager.Instance es null. Asegurate de haberlo creado en la escena inicial y que tenga DontDestroyOnLoad.");
        }

        if (leaderboardContent != null && rowPrefab != null)
        {
            var list = manager.GetGlobalBestList();
            Debug.Log("[SimpleUIManager] Global list count = " + (list != null ? list.Count : 0));
            if (list != null)
            {
                foreach (var item in list)
                {
                    GameObject row = Instantiate(rowPrefab, leaderboardContent);

                    var nameTxt = row.transform.Find("TxtName")?.GetComponent<TextMeshProUGUI>();
                    var scoreTxt = row.transform.Find("TxtScore")?.GetComponent<TextMeshProUGUI>();

                    if (nameTxt != null) nameTxt.text = item.name;
                    else Debug.LogWarning("[SimpleUIManager] rowPrefab no contiene hijo 'TxtName' con TextMeshProUGUI.");

                    if (scoreTxt != null) scoreTxt.text = item.best.ToString();
                    else Debug.LogWarning("[SimpleUIManager] rowPrefab no contiene hijo 'TxtScore' con TextMeshProUGUI.");
                }
            }
        }
        else
        {
            if (rowPrefab == null) Debug.LogWarning("[SimpleUIManager] rowPrefab NO asignado en el inspector.");
            if (leaderboardContent == null) Debug.LogWarning("[SimpleUIManager] leaderboardContent NO asignado en el inspector.");
        }

        string me = !string.IsNullOrEmpty(currentPlayerName) ? currentPlayerName : (PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : "");
        Debug.Log("[SimpleUIManager] RefreshLeaderboardUI -> buscando intentos para: '" + me + "'");

        if (manager != null && myAttemptsContent != null && attemptRowPrefab != null && !string.IsNullOrEmpty(me))
        {
            var attempts = manager.GetAttempts(me);
            Debug.Log("[SimpleUIManager] Attempts count for " + me + " = " + (attempts != null ? attempts.Count : 0));
            if (attempts != null)
            {
                for (int i = 0; i < attempts.Count; i++)
                {
                    GameObject r = Instantiate(attemptRowPrefab, myAttemptsContent);
                    var t = r.GetComponentInChildren<TextMeshProUGUI>();
                    if (t != null) t.text = $"Intento {i + 1}: {attempts[i]}";
                    else Debug.LogWarning("[SimpleUIManager] attemptRowPrefab no tiene TextMeshProUGUI en su hijo.");
                }
            }
        }

        else
        {
            if (string.IsNullOrEmpty(me)) Debug.LogWarning("[SimpleUIManager] Nombre del jugador vacío; no se mostrarán intentos personales.");
            if (attemptRowPrefab == null) Debug.LogWarning("[SimpleUIManager] attemptRowPrefab NO asignado en el inspector.");
            if (myAttemptsContent == null) Debug.LogWarning("[SimpleUIManager] myAttemptsContent NO asignado en el inspector.");
        }

    }

}
