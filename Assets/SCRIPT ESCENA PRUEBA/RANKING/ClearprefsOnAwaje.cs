using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearprefsOnAwaje : MonoBehaviour
{
    public bool runOnce = true;

    void Awake()
    {
        if (!runOnce) return;

        PlayerPrefs.DeleteKey("SimpleLeaderboard_v1");
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.Save();
        Debug.Log("[ClearPrefsOnAwake] Borradas claves PlayerPrefs.");

        var sm = MyGame.Profiles.SimpleManager.Instance;
        if (sm != null) sm.ClearAll();

        var spm = FindObjectOfType<SimpleProfileManager>();
        if (spm != null) spm.ClearAll();

        
        runOnce = false;
    }

}
