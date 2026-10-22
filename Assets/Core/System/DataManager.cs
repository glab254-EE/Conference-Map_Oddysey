using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager 
{
    public static GameData GData { get; private set; }
    public static event Action<GameData> OnDataChange;
    public static void OnStart(IEnumerable<string> islands)
    {
        GData = TryGetGameData(islands);
    }
    private static DateTime GetUpdatedCooldown()
    {
       return DateTime.Now.AddDays(1);
    }
    private static bool TrySaveData()
    {
        try
        {
            foreach (var data in GData.IsleInformation)
            {
                PlayerPrefs.SetFloat(data.Key + "_Score", data.Value.Item1);
                PlayerPrefs.SetInt(data.Key + "_Takes", data.Value.Item2);
            }
            if (GData.playerData != null)
            {
                PlayerPrefs.SetString("Player_User", GData.playerData.username);
                PlayerPrefs.SetString("Player_Pass", GData.playerData.password);
            }
            if (GData.HaveDoneTutorial)
            {
                PlayerPrefs.SetString("TutorialComplete", "true");
            }
            PlayerPrefs.Save();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            return false;
        }
    }
    private static GameData TryGetGameData(IEnumerable<string> islands)
    {
        GameData output = new();
        try
        {
            foreach (string isle in islands)
            {
                if (!PlayerPrefs.HasKey(isle + "_Score") || !PlayerPrefs.HasKey(isle + "_Takes")) continue;
                float item1 = PlayerPrefs.GetFloat(isle + "_Score");
                int item2 = PlayerPrefs.GetInt(isle + "_Takes");
                output.IsleInformation ??= new();
                output.IsleInformation.Add(isle, new (item1, item2, GetUpdatedCooldown()));
            }
            if (PlayerPrefs.HasKey("Player_User") == true)
            {
                string user = PlayerPrefs.GetString("Player_User");
                if (output.playerData == null)
                {
                    output.playerData = new() { username = user, displayname = user };
                }
                else
                {
                    output.playerData.username = user;
                    output.playerData.displayname = user;
                }
            }
            if (PlayerPrefs.HasKey("Player_Pass") == true)
            {
                output.playerData.password = PlayerPrefs.GetString("Player_Pass");
            }
            if (PlayerPrefs.HasKey("TutorialComplete"))
            {
                output.HaveDoneTutorial = PlayerPrefs.HasKey("TutorialComplete");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            return new();
        }
        OnDataChange?.Invoke(output);
        return output;
    }
    public static bool TrySetPlayer(PlayerData pl)
    {
        if (pl == null) return false;
        GData.playerData = pl;
        TrySaveData();
        return true;
    }
    public static void UpdateTutorialStatus()
    {
        GData.HaveDoneTutorial = true;
        TrySaveData();
    }
    public static bool TryUpdateTakes(string IsleName, int takes)
    {

        if (GData == null || IsleName == null) return false;
        if (GData.IsleInformation == null) return false;
        if (GData.IsleInformation.ContainsKey(IsleName) == false)
        {
            GData.IsleInformation.Add(IsleName, (0, takes, System.DateTime.Now));
        }
        else
        {
            var old = GData.IsleInformation[IsleName];
            GData.IsleInformation.Remove(IsleName);
            (float, int, DateTime) newD = new(old.Item1, takes, System.DateTime.Now);
            GData.IsleInformation.Add(IsleName, newD);
        }
        TrySaveData();
        return true;
    }
    public static bool TryUpdateScoreCount(string IsleName, float score)
    {
        if (GData == null || IsleName == null) return false;
        if (GData.IsleInformation == null) return false;
        if (GData.IsleInformation.ContainsKey(IsleName) == false)
        {
            GData.IsleInformation.Add(IsleName, (score, 0, GetUpdatedCooldown()));
        }
        else
        {
            var old = GData.IsleInformation[IsleName];
            (float, int, DateTime) newD = new(score, old.Item2, GetUpdatedCooldown());
            GData.IsleInformation[IsleName] = newD;
        }
        TrySaveData();
        return true;
    }
    public static void DeleteData()
    {
        GData.IsleInformation = new();
        GData.playerData = null;
        GData.HaveDoneTutorial = false;
        TrySaveData();
        OnDataChange?.Invoke(GData);
    }
}
