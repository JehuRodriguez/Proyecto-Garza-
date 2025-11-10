using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NameInputManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;

    [Header("Game Scene Name")]
    public string gameSceneName = "GameScene";

    public void OnStartGameClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Por favor ingresa un nombre antes de continuar.");
            return;
        }

        
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

       
        SceneManager.LoadScene(gameSceneName);
    }

}
