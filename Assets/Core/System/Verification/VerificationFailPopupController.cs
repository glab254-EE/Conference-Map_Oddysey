using System.Collections;
using TMPro;
using UnityEngine;

public class VerificationFailPopupController : MonoBehaviour
{
    [SerializeField]
    private VerificationHandler handler;
    [SerializeField]
    private GameObject TargetGameobject;
    [SerializeField]
    private TMP_Text TargetText;
    [SerializeField]
    private float TargetViewDuration;
    void Start()
    {
        handler.OnError += OnErrorRecieve;
    }
    void OnErrorRecieve(string error)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayEnumerator(error));
    }
    IEnumerator DisplayEnumerator(string error)
    {
        TargetGameobject.SetActive(true);
        TargetText.text = error;
        yield return new WaitForSecondsRealtime(TargetViewDuration);
        TargetGameobject.SetActive(false);
    }
}
