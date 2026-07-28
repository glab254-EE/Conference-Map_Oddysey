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
    private Button button;
    public UnityEvent OnFinishedEvent;
    void Start()
    {
        _questHandler.OnFinish.AddListener(OnEnd);
    }
    void OnEnd(int val, int maxval)
    {
        Text_Field.text = string.Format(Win_Format, val, maxval);
        Main_Object.SetActive(true);
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
        OnFinishedEvent?.Invoke();
    }
    private IEnumerator ViewEnumerator()
    {
        yield return new WaitForSeconds(ViewDurationMax);
        HideFrame();
    }
}
