using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class PlayerScore
{
    public string name;
    public int score;
}

[System.Serializable]
public class ScoreList
{
    public List<PlayerScore> entries = new List<PlayerScore>();
}


public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    private const string KEY = "PANTANOS_LEADERBOARD_V1";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddEntry(string name, int score)
    {
        ScoreList list = LoadList();
        list.entries.Add(new PlayerScore { name = name, score = score });
        SaveList(list);
    }

    public ScoreList LoadList()
    {
        if (!PlayerPrefs.HasKey(KEY)) return new ScoreList();
        string json = PlayerPrefs.GetString(KEY);
        try
        {
            var s = JsonUtility.FromJson<ScoreList>(json);
            if (s == null) return new ScoreList();
            return s;
        }
        catch
        {
            return new ScoreList();
        }
    }

    public void SaveList(ScoreList list)
    {
        string json = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public List<PlayerScore> GetTop(int max = 10)
    {
        var l = LoadList();
        return l.entries.OrderByDescending(e => e.score).Take(max).ToList();
    }

}
