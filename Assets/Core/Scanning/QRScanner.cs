using System;
using System.Runtime.InteropServices;
using UnityEngine;


public static class QRScanner
{
    public static event Action<string> Listeners;
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void StartQRScanner();

    [DllImport("__Internal")]
    private static extern void StopQRScanner();
#endif

    public static void Scan()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StartQRScanner();
#else
        Debug.Log("Scanning started!");
#endif
    }
    public static void StopScan()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        StopQRScanner();
#else
        Debug.Log("Scanning stopped!");
#endif
    }

    public static void OnQRCodeDetected(string text)
    {
        Debug.Log(text);
        Listeners?.Invoke(text);
    }
}
