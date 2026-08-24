using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionCompletedFrame : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler _questHandler;
    [SerializeField]
    private float ViewDurationMax = 4f;
    [SerializeField]
    private GameObject Main_Object;
    [SerializeField]
    private TMP_Text Text_Field;
    [SerializeField]
    private string Win_Format = "Остров пройден!\n{0} баллов из {1} возможных.";
    [SerializeField]
    private float MaximumScore;
    [SerializeField]
    private float MinimumScore;
    [SerializeField]
    private bool RepeatAfterClick = false;
    [SerializeField]
    private Button button;
    [SerializeField]
    private UnityEvent OnEndEvent;
    [SerializeField]
    private UnityEvent OnAppearEvent;
    void Start()
    {
        _questHandler.OnFinish.AddListener(OnEnd);
    }
    void OnEnd(float val, float maxval)
    {
        if (MinimumScore > val || MaximumScore < val)
        {
            return;
        }
        Text_Field.text = string.Format(Win_Format, val, maxval);
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
        button?.onClick.RemoveListener(HideFrame);
        Main_Object.SetActive(false);
        if (RepeatAfterClick == false || _questHandler.TryStartRun(null, null) == false)
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
