using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class QuestionTakesUpdater : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler Handler;
    [SerializeField]
    private string Format = "Попытка {0}/2";
    private TMP_Text textField;
    void Start()
    {
        textField = GetComponent<TMP_Text>();
        Handler.OnStart.AddListener(UpdateText);
        UpdateText(1);
    }
    void UpdateText(int takes)
    {
        textField.text = string.Format(Format, takes);
    }
}
