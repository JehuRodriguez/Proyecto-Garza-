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


public class SimpleProfileManager : MonoBehaviour
{
    public static SimpleProfileManager Instance { get; private set; }
    const string PREF_KEY = "SimpleLeaderboard_v1";

    LeaderboardData data = new LeaderboardData();

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); Load(); }
        else Destroy(gameObject);
    }

    void Load()
    {
        if (PlayerPrefs.HasKey(PREF_KEY))
        {
            try
            {
                string json = PlayerPrefs.GetString(PREF_KEY);
                data = JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
            }
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
        var pr = data.players.Find(p => p.name == playerName);
        if (pr == null) { pr = new PlayerRecord { name = playerName }; data.players.Add(pr); }
        pr.attempts.Add(score);
        Save();
    }

    public List<int> GetAttempts(string playerName)
    {
        var pr = data.players.Find(p => p.name == playerName);
        if (pr == null) return new List<int>();
        return new List<int>(pr.attempts);
    }

    public List<(string name, int best)> GetGlobalBestList()
    {
        return data.players
            .Select(p => (p.name, best: p.attempts.Count > 0 ? p.attempts.Max() : 0))
            .OrderByDescending(x => x.best)
            .ThenBy(x => x.name)
            .ToList();
    }

    public int GetBest(string playerName)
    {
        var pr = data.players.Find(p => p.name == playerName);
        if (pr == null || pr.attempts.Count == 0) return 0;
        return pr.attempts.Max();
    }

    public void ClearAll()
    {
        data = new LeaderboardData();
        Save();
    }
}
