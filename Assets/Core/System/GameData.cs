using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Dictionary<string, (float,int,DateTime)> IsleInformation = new();
    public PlayerData playerData = null;
    public bool HaveDoneTutorial = false;
}
