using System.Collections;
using TMPro;
using UnityEngine;

public class DebugScanningChecker : MonoBehaviour, IScanDetectedReciever
{
    [SerializeField]
    private TMP_Text displayField;
    [SerializeField]
    private GameObject displayObject;
    [SerializeField]
    private float displayDuration;
    private Coroutine displayCoroutine;
    void Start()
    {
        QRScanner.instance.AddReciever(this);
    }
    public void OnScanRecieve(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
        if (displayCoroutine != null) StopCoroutine(displayCoroutine);
        displayField.text = message;
        displayObject.SetActive(true);
        displayCoroutine = StartCoroutine(DisplayEnumerator());
#endif
    }
    IEnumerator DisplayEnumerator()
    {
        yield return new WaitForSeconds(displayDuration);
        displayObject.SetActive(false);
    }
}
