using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestionsHandler : MonoBehaviour
{
    [field:SerializeField]
    public float Points { get; private set; }
    [SerializeField]
    private TMP_Text Description;
    [SerializeField]
    private List<QuestionElement> Buttons;
    [field:SerializeField]
    private List<Question> Starting_Questions;
    [SerializeField]
    private float After_Answer_Wait_Time;
    public UnityEvent<int, int> OnFinish;
    private Coroutine mainCoroutineLoop;
    private Queue<Question> Questions;
    void Start()
    {
        StartRun(Starting_Questions);
    }
    public void StartRun(IEnumerable<Question> questions)
    {
        if (questions == null && (Questions == null || Questions.Count == 0))
        {
            return;
        }
        if (questions != null)
        {
            Questions = new(questions);
        }
        if (mainCoroutineLoop != null)
        {
            StopCoroutine(mainCoroutineLoop);
        }
        Debug.Log(Questions.ToCommaSeparatedString());
        mainCoroutineLoop = StartCoroutine(MainLoop());
    }
    private IEnumerator MainLoop()
    {
        if (Questions == null || Questions.Count == 0)
        {
            yield break;
        }
        int _correctPicks = 0;
        int _startingCount = Questions.Count;
        do
        {
            bool _picked = false;
            Question question = Questions.Dequeue();
            Description.text = question.Description;
            for (int i = 0; i < question.options.Count; i++)
            {
                if (Buttons.Count > i)
                {
                    QuestionOption option = question.options[i];
                    QuestionElement questionElement = Buttons[i];
                    questionElement.SetUp(option.OptionText, option.IsCorrect);
                    questionElement.onPick.AddListener(() =>
                    {
                        questionElement.AfterPick(true);
                        foreach(QuestionElement element in Buttons)
                        {
                            if (element == questionElement) continue;
                            element.AfterPick(false);
                        }
                        if (option.IsCorrect)
                        {
                            Points += option.PointsGain;
                            _correctPicks++;
                        }
                        _picked = true;
                    });
                }
            }
            Debug.Log("Waiting!");
            while (!_picked)
            {
                yield return null;
            }
            yield return new WaitForSeconds(After_Answer_Wait_Time);
        } while (Questions.Count > 0);
        OnFinish?.Invoke(_correctPicks, _startingCount);
    }
}
[Serializable]
public struct QuestionOption
{
    [SerializeField]
    public string OptionText;
    [SerializeField]
    public bool IsCorrect;
    [SerializeField]
    public float PointsGain;
}
[Serializable]
public struct Question
{
    [SerializeField]
    public string Description;
    [SerializeField]
    public List<QuestionOption> options;
}
