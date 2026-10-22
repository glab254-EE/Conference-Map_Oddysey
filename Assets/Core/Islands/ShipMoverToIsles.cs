using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ShipMover))]
public class ShipMoverToIsles : MonoBehaviour, IScanDetectedReciever
{
	[SerializeField]
	private GlobalScoreCounter MainCounter;
    [SerializeField]
	private List<IslandVisualInstance> Islands;
	[SerializeField]
	private InterfacesHandler Interfaces_Handler;
	[SerializeField]
	private int VisitedFrameIndex;
    private ShipMover mover;
	private bool Available = true;
	void Awake()
	{
		mover = GetComponent<ShipMover>();
        try
        {
			List<string> Isles = new();
			foreach(var i in Islands)
			{
				Isles.Add(i.Island_ID);
			}
			DataManager.OnStart(Isles);
            var data = DataManager.GData.IsleInformation;
			float score = 0;
            foreach (var isleData in data)
            {
				if (isleData.Value.Item2 > 0)
                {
                    var found = Islands.First((a) => a.Island_ID == isleData.Key);
                    if (found != null)
                        found.UnlockVisual(isleData.Value.Item1 > 0);
                    score += isleData.Value.Item2;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
	void Start()
    {
        Available = true;
        QRScanner.instance.AddReciever(this);
    }
	public void RemoveData()
	{
		DataManager.DeleteData();
		foreach (var isle in Islands)
		{
			isle.LockIsland();
		}
	}
	public void OnWin()
	{
		Available = true;
    }
    public void OnScanRecieve(string text)
    {
		if (!Available) return;
        try
		{
            QRToIsleData isleData = JsonUtility.FromJson<QRToIsleData>(text);
			if (isleData == null) return;
            foreach (var isle in Islands)
			{
				if (isle.Island_ID == isleData.IsleName)
				{
					if (isle.HaveVisited())
					{
						Interfaces_Handler.OpenFrameByIndex(VisitedFrameIndex);
						return;
					}
					if (isle.TryUnlock(isleData.IsleKey,out Transform target) == true)
                    {
						QRScanner.instance.StopScan();
						Interfaces_Handler.OpenFrameByIndex(0);
                        Available = false;
                        isle.OnWinEvent += OnWin;
						mover.MoveAgent(target.position);
						return;
					}
				}
			}
        }
		catch (System.Exception e)
		{
			Debug.LogWarning(e.Message);
		}
    }
}
