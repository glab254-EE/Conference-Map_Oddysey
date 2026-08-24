using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestionsHandler : MonoBehaviour
{
    [SerializeField]
    private float MaxTimeUntilTimeout = 180;
    [SerializeField]
    private int MaxTakes = 2;
    [SerializeField]
    private TMP_Text Description;
    [SerializeField]
    private List<QuestionElement> Buttons;
    [SerializeField]
    private float AfterAnswerWaitTime;
    [SerializeField]
    private string TimerFormat;
    [SerializeField]
    private TMP_Text TimerTextField;
    [HideInInspector]
    public float Timer { get; private set; }
    public UnityEvent<int> OnStart;
    public UnityEvent<float, float> OnFinish;
    public UnityEvent<int> OnLose;
    public UnityEvent<int> OnTimeOut;
    public Dictionary<string, float> Points { get; private set; } = new();
    private Dictionary<string, int> Takes = new();
    private Coroutine mainCoroutineLoop;
    private Queue<Question> Questions;
    private string CurrentIsle;
    void Awake()
    {
        try
        {
            var data = DataManager.GData.IsleInformation;
            foreach (var isleData in data)
            {
                Takes.Add(isleData.Key, isleData.Value.Item2);
                Points.Add(isleData.Key, isleData.Value.Item1);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
    public bool CanStartRun(string isle)
    {

        if ((Questions != null
            && Takes.TryGetValue(isle, out int val1)
            && val1 >= MaxTakes)
            ){
            return false;
        }
        return true;
    }
    public bool TryStartRun(IEnumerable<Question> questions, string isle)
    {
        if (isle == null)
        {
            if (CurrentIsle == null)
            {
                return false;
            } else
            {
                isle = CurrentIsle;
            }
        }
        if ((questions == null && (Questions == null || Questions.Count == 0))
            || (questions != null
            && Takes.TryGetValue(isle, out int val)
            && val >= MaxTakes)
            || (questions == null && Questions != null
            && Takes.TryGetValue(isle, out int val1)
            && val1 >= MaxTakes)
            )
        {
            return false;
        }
        if (questions != null)
            Questions = new(questions);
        if (mainCoroutineLoop != null)
            StopCoroutine(mainCoroutineLoop);
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        if (isle != null)
        {
            CurrentIsle = isle;
        }
        mainCoroutineLoop = StartCoroutine(MainLoop());
        return true;
    }
    private IEnumerator MainLoop()
    {
        if (Questions == null || Questions.Count == 0)
        {
            yield break;
        }
        Timer = MaxTimeUntilTimeout;
        Queue<Question> cloned = new(Questions);
        Time.timeScale = 0;
        if (Points.ContainsKey(CurrentIsle) == false)
        {
            Points.Add(CurrentIsle, 0);
        }
        if (Takes.ContainsKey(CurrentIsle))
        {
            Takes[CurrentIsle] += 1;
        }
        else
        {
            Takes.Add(CurrentIsle, 1);
        }
        OnStart?.Invoke(Takes[CurrentIsle]);
        float _correctPicks = 0;
        float _maxPoints = 0;
        float _currentPoints = 0;
        do
        {
            bool _picked = false;
            Question question = Questions.Dequeue();
            Description.text = question.Description;
            float questionPoints = 0;
            for (int i = 0; i < question.options.Count; i++)
            {
                if (question.options[i].IsCorrect)
                {
                    questionPoints = question.options[i].PointsGain > questionPoints ? question.options[i].PointsGain : questionPoints;
                }
                if (Buttons.Count > i)
                {
                    QuestionOption option = question.options[i];
                    QuestionElement questionElement = Buttons[i];
                    questionElement.SetUp(option.OptionText, option.IsCorrect);
                    questionElement.onPick.AddListener(() =>
                    {
                        if (_picked)
                        {
                            return;
                        }
                        questionElement.AfterPick(true);
                        if (option.IsCorrect)
                        {
                            _currentPoints += option.PointsGain;
                            _correctPicks++;
                        }
                        _picked = true;
                    });
                }
            }
            _maxPoints += questionPoints;
            while (!_picked && Timer > 0)
            {
                if (TimerTextField != null)
                {
                    int seconds = Mathf.RoundToInt(Timer);
                    int Minutes = seconds / 60;
                    seconds %= 60;
                    TimerTextField.text = string.Format(TimerFormat, Minutes, seconds);
                }
                Timer -= 0.1f;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            yield return new WaitForSecondsRealtime(AfterAnswerWaitTime);
        } while (Questions.Count > 0 && Timer > 0);

        Questions = cloned;
        int takes = Takes[CurrentIsle];
        _currentPoints = Mathf.Clamp(_currentPoints, 0, _maxPoints);
        if (_currentPoints != 0)
        {
            _currentPoints = Mathf.Round(_currentPoints / takes * 10) / 10;
        }
        else
        {
            OnLose?.Invoke(takes);
        }
        if (Points.ContainsKey(CurrentIsle))
        {
            Points[CurrentIsle] = _currentPoints;
        } else
        {
            Points.Add(CurrentIsle, _currentPoints);
        }
        if (Timer <= 0)
        {
            OnTimeOut?.Invoke(takes);
        }
        else
        {
            OnFinish?.Invoke(_currentPoints, _maxPoints);
            StopCoroutine(mainCoroutineLoop);
        }
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
