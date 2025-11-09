using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile Instance { get; private set; }
    public string playerName = "";

    const string PREF_KEY_NAME = "PlayerName";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void SetName(string name)
    {
        playerName = name.Trim();
        PlayerPrefs.SetString(PREF_KEY_NAME, playerName);
        PlayerPrefs.Save();
    }

    void Load()
    {
        if (PlayerPrefs.HasKey(PREF_KEY_NAME))
            playerName = PlayerPrefs.GetString(PREF_KEY_NAME);
    }
}
