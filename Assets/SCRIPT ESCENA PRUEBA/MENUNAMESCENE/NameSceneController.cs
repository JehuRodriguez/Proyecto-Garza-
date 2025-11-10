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

    const string PREF_KEY_NAME = "PlayerName";

    void Start()
    {
        if (btnStart != null) btnStart.onClick.AddListener(OnStartClicked);

        
        if (nameInputField != null && PlayerPrefs.HasKey(PREF_KEY_NAME))
        {
            nameInputField.text = PlayerPrefs.GetString(PREF_KEY_NAME);
        }
    }

    void OnStartClicked()
    {
        string n = nameInputField != null ? nameInputField.text.Trim() : "";
        if (string.IsNullOrEmpty(n)) return;

       
        PlayerPrefs.SetString(PREF_KEY_NAME, n);
        PlayerPrefs.Save();

        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
    }

}
