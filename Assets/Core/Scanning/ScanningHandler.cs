using UnityEngine;

public class ScanningHandler : MonoBehaviour
{
    void OnDisable()
    {
        StopScanning();
    }
    void OnEnable()
    {
        StartScanning();
    }
    public void StartScanning()
    {
        QRScanner.instance.Scan();
    }
    public void StopScanning()
    {
        QRScanner.instance.StopScan();  
    }
}
