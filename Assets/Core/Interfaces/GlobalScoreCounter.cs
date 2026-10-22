using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GlobalScoreCounter : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler questionsHandler;
    public float TotalScore { get; private set; } = 0;
    public int Wins { get; private set; } = 0;
    [HideInInspector]
    public event Action<int> OnWinsChange;
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        questionsHandler.OnFinish.AddListener(OnEnd);
        questionsHandler.OnDataLoaded += UpdateText;
        UpdateText();
    }
    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        UpdateText();
    }
    void OnEnd(float _1, float _2)
    {
        UpdateText();
    }
    public void UpdateText()
    {
        float total = 0;
        int wins = 0;
        if (DataManager.GData.IsleInformation != null)
        {
            foreach (var values in DataManager.GData.IsleInformation)
            {
                if (values.Value.Item1 >= 0 && values.Value.Item2 >= 2)
                {
                    total += values.Value.Item1;
                    wins++;
                }
            }
            Wins = wins;
            TotalScore = total;
            text.text = total.ToString();
        }
        if (wins == 0 && questionsHandler.Points != null && questionsHandler.Points.Count > 0)
        {
            total = 0;
            wins = 0;
            foreach (var values in questionsHandler.Points)
            {
                if (values.Value >= 0)
                {
                    total += values.Value;
                    wins++;    
                }
            }
        }
        OnWinsChange?.Invoke(wins);
    }
}
