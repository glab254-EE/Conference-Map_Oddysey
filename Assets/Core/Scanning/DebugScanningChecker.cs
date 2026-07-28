using System.Collections;
using TMPro;
using UnityEngine;

public class DebugScanningChecker : MonoBehaviour
{
    [SerializeField]
    private TMP_Text displayField;
    [SerializeField]
    private GameObject displayObject;
    [SerializeField]
    private float displayDuration;
    void Start()
    {
        QRScanner.Listeners += OnMessage;
    }
    void OnMessage(string message)
    {
        StopAllCoroutines();
        displayField.text = message;
        displayObject.SetActive(true);
    }
    IEnumerator displayEnumerator()
    {
        yield return new WaitForSeconds(displayDuration);
        displayObject.SetActive(false);
    }
}
