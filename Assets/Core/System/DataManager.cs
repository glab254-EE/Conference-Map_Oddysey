using System;
using UnityEngine;

public static class DataManager 
{
    public static GameData GData { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnStart()
    {
        //GData = TryGetGameData();
    }
    private static System.DateTime GetUpdatedCooldown()
    {
       return DateTime.Now.AddSeconds(1);
    }
    private static bool TrySaveData()
    {
        //try
        //{
        //    var converted = JsonUtility.ToJson(GData);
        //    PlayerPrefs.SetString("_DATA", converted);
        //    PlayerPrefs.Save();
        //    return true;
        //}
        //catch (Exception e)
        //{
        //    Debug.LogWarning(e.Message);
        //    return false;
        //}
        return true;
    }
    private static GameData TryGetGameData()
    {
        GameData output = new();
/*        try
        {
            var storedData = PlayerPrefs.GetString("_DATA");
            output = JsonUtility.FromJson<GameData>(storedData);
            output ??= new();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
            return new();
        }*/
        return output;
    }
    public static bool CanEnterIsle(string IsleName)
    {
        return true;
        //if (GData == null || IsleName == null)
        //{
        //    return false;
        //}
        //if (GData.IsleInformation == null)
        //{
        //    GData.IsleInformation = new();
        //    return true; 
        //}
        //if (GData.IsleInformation.ContainsKey(IsleName) == false) return true;
        //if (GData.IsleInformation.TryGetValue(IsleName,out var isleData) == true)
        //{
        //    return true;
        //    //if (isleData.Item3.CompareTo(System.DateTime.Now) < 1 || isleData.Item2 < 2)
        //    //{
        //    //}
        //}
        //return false;
    }
    public static bool TryUpdateTakes(string IsleName, int takes)
    {

        return true;
        //if (GData == null || IsleName == null) return false;
        //if (GData.IsleInformation == null) return false;
        //if (GData.IsleInformation.ContainsKey(IsleName) == false)
        //{
        //    GData.IsleInformation.Add(IsleName, (0, takes, System.DateTime.Now));
        //}
        //else
        //{
        //    var old = GData.IsleInformation[IsleName];
        //    GData.IsleInformation.Remove(IsleName);
        //    (float, int, DateTime) newD = new(old.Item1, takes, System.DateTime.Now);
        //    GData.IsleInformation.Add(IsleName, newD);
        //}
        //TrySaveData();
        //return true;
    }
    public static bool TryUpdateScoreCount(string IsleName, float score)
    {
        //if (GData == null || IsleName == null) return false;
        //if (GData.IsleInformation == null) return false;
        //if (GData.IsleInformation.ContainsKey(IsleName) == false)
        //{
        //    GData.IsleInformation.Add(IsleName, (score, 0, GetUpdatedCooldown()));
        //}
        //else
        //{
        //    var old = GData.IsleInformation[IsleName];
        //    if (old.Item1 < score)
        //    {
        //        GData.IsleInformation.Remove(IsleName);
        //        (float, int, DateTime) newD = new(score, old.Item2, GetUpdatedCooldown());
        //        GData.IsleInformation.Add(IsleName, newD);
        //    }
        //}
        //TrySaveData();
        return true;
    }
}
