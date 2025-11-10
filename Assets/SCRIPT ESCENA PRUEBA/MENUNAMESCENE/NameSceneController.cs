using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class NameSceneController : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Button btnStart;
    public string gameSceneName = "JUEGO2";

    void Start()
    {
        if (btnStart != null) btnStart.onClick.AddListener(OnStartClicked);
       
        if (PlayerProfile.Instance != null && nameInputField != null)
            nameInputField.text = PlayerProfile.Instance.playerName;
    }

    void OnStartClicked()
    {
        string n = nameInputField != null ? nameInputField.text.Trim() : "";
        if (string.IsNullOrEmpty(n)) return;
        if (PlayerProfile.Instance != null)
            PlayerProfile.Instance.SetName(n);
       
        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
    }




}
