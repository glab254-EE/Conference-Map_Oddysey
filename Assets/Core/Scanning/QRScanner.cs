using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class QRScanner : MonoBehaviour
{
    public static QRScanner instance;
    private List<IScanDetectedReciever> recievers = new();
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartQRScanner();

    [DllImport("__Internal")]
    private static extern void StopQRScanner();
#endif
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Scan()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartQRScanner();
#else
        Debug.Log("Scanning started!");
        OnQRCodeDetected("Fake Scan");
#endif
    }
    public void StopScan()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StopQRScanner();
#else
        Debug.Log("Scanning stopped!");
#endif
    }
    public void AddReciever(IScanDetectedReciever reciever)
    {
        if (recievers.Contains(reciever)) return;
        recievers.Add(reciever);
    }
    public void RemoveReciever(IScanDetectedReciever reciever)
    {
        if (recievers.Contains(reciever)) recievers.Remove(reciever);
    }
    void OnQRCodeDetected(string text)
    {
        foreach (var rec in recievers)
        {
            rec.OnScanRecieve(text);
        }
    }
}
