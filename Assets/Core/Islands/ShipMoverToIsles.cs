using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ShipMover))]
public class ShipMoverToIsles : MonoBehaviour, IScanDetectedReciever
{
	[SerializeField]
	private List<IslandVisualInstance> Islands;
	[SerializeField]
	private InterfacesHandler Interfaces_Handler;
    private ShipMover mover;
	private bool Available = true;
	void Start()
	{
		mover = GetComponent<ShipMover>();
		QRScanner.instance.AddReciever(this);
		Available = true;
        try
        {
            var data = DataManager.GData.IsleInformation;
            foreach (var isleData in data)
            {
                var found = Islands.First((a) => a.name == isleData.Key);
                if (found != null)
                {
                    found.UnlockVisual(isleData.Value.Item1 > 0);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
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
