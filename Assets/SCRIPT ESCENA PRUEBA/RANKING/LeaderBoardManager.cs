using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


[Serializable]
public class PlayerRecord
{
    public string name;
    public List<int> attempts = new List<int>(); 
}

[Serializable]
public class LeaderboardData
{
    public List<PlayerRecord> players = new List<PlayerRecord>();
}


public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance { get; private set; }

    const string PREF_KEY = "LeaderboardData_v1";

    LeaderboardData data = new LeaderboardData();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Load()
    {
        if (PlayerPrefs.HasKey(PREF_KEY))
        {
            string json = PlayerPrefs.GetString(PREF_KEY);
            try { data = JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData(); }
            catch { data = new LeaderboardData(); }
        }
        else data = new LeaderboardData();
    }

    void Save()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(PREF_KEY, json);
        PlayerPrefs.Save();
    }

    public void AddAttempt(string playerName, int score)
    {
        if (string.IsNullOrEmpty(playerName)) return;

        PlayerRecord pr = data.players.Find(p => p.name == playerName);
        if (pr == null)
        {
            pr = new PlayerRecord { name = playerName };
            data.players.Add(pr);
        }

        pr.attempts.Add(score);
        Save();
    }


    public List<int> GetAttempts(string playerName)
    {
        PlayerRecord pr = data.players.Find(p => p.name == playerName);
        if (pr == null) return new List<int>();
        return new List<int>(pr.attempts);
    }

    public int GetBest(string playerName)
    {
        PlayerRecord pr = data.players.Find(p => p.name == playerName);
        if (pr == null || pr.attempts.Count == 0) return 0;
        return pr.attempts.Max();
    }

    public List<(string name, int best)> GetGlobalBestList()
    {
        var list = data.players.Select(p => (p.name, best: (p.attempts.Count > 0 ? p.attempts.Max() : 0)))
                    .OrderByDescending(x => x.best)
                    .ThenBy(x => x.name)
                    .ToList();
        return list;
    }

    public void ClearAll()
    {
        data = new LeaderboardData();
        Save();
    }
}
