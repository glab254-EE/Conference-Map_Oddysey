using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestionElement : MonoBehaviour
{
    [SerializeField]
    public Color CorrectPick_Color = Color.darkOliveGreen;
    [SerializeField]
    public Color IncorrectPick_Color = Color.maroon;
    [SerializeField]
    public Color NotCorrect_Color = Color.gray;
    [SerializeField]
    private TMP_Text Text_Field;
    [SerializeField]
    private Image Button_Image;
    public UnityEvent onPick;
    private bool is_Correct = false;
    private Color default_Color;
    private Button button;
    private bool isOver = false;
    void Awake()
    {
        button = GetComponent<Button>();
        default_Color = Button_Image.color;
        button.onClick.AddListener(() =>
        {
            onPick?.Invoke();
        });
    }
    public void SetUp(string text, bool isCorrect)
    {
        Button_Image.color = default_Color;
        Text_Field.text = text;
        isOver = false;
        this.is_Correct = isCorrect;
    }
    public void AfterPick(bool Picked)
    {
        if (isOver) return;
        isOver = true;
        onPick.RemoveAllListeners();
        if (is_Correct)
        {
            Button_Image.color = CorrectPick_Color;
        } else if (Picked && !is_Correct)
        {
            Button_Image.color = IncorrectPick_Color;
        } else
        {
            Button_Image.color= NotCorrect_Color;
        }
    }
}
