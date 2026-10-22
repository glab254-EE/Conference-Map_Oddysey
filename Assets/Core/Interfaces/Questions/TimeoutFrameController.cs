using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeoutFrameController : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler _questHandler;
    [SerializeField]
    private float ViewDurationMax = 4f;
    [SerializeField]
    private GameObject Main_Object;
    [SerializeField]
    private Button button;
    [SerializeField]
    private UnityEvent OnEndEvent;
    [SerializeField]
    private UnityEvent OnAppearEvent;
    void Start()
    {
        _questHandler.OnTimeOut.AddListener(OnEnd);
    }
    void OnEnd(int takes)
    {
        Main_Object.SetActive(true);
        OnAppearEvent?.Invoke();
        if (button == null) StartCoroutine(ViewEnumerator());
        else
        {
            button.onClick.AddListener(HideFrame);
        }
    }
    void HideFrame()
    {
        Main_Object.SetActive(false);
        if (_questHandler.TryStartRun(null, null) == false)
        {
            OnEndEvent?.Invoke();
            Time.timeScale = 1;
        }
    }
    private IEnumerator ViewEnumerator()
    {
        yield return new WaitForSecondsRealtime(ViewDurationMax);
        HideFrame();
    }
}
