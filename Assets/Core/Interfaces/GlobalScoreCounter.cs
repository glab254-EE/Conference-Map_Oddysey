using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GlobalScoreCounter : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler QuestionsHandler;
    public float TotalScore { get; private set; } = 0;
    public int Wins { get; private set; } = 0;
    public event Action<int> OnWinsChange;
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        QuestionsHandler.OnFinish.AddListener(OnEnd);
        UpdateText();
    }
    void OnEnd(float _, float _1)
    {
        UpdateText();
    }
    void UpdateText()
    {
        float total = 0;
        int wins = 0;
        foreach (var values in QuestionsHandler.Points)
        {
            if (values.Value > 0)
            {
                total += values.Value;
                wins++;
            }
        }
        Wins = wins;
        TotalScore = total;
        text.text = total.ToString();
        OnWinsChange?.Invoke(wins);
    }
}
