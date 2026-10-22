using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WinsCounterDisplay : MonoBehaviour
{
    [SerializeField]
    private GlobalScoreCounter counter;
    [SerializeField]
    private QuestionsHandler handler;
    [SerializeField]
    private string Format;
    private TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        counter.OnWinsChange += UpdateText;
        UpdateText(0);
    }
    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        UpdateText(0);
    }
    void UpdateText(int _)
    {
        int wins = 0;
        if (DataManager.GData.IsleInformation != null)
        {
            foreach (var values in DataManager.GData.IsleInformation)
            {
                if (values.Value.Item1 >= 0 && values.Value.Item2 >= 2)
                {
                    wins++;
                }
            }
        }
        if (handler.Points != null && handler.Points.Count > 0)
        {
            int newWins = 0;
            foreach (var values in handler.Points)
            {
                if (values.Value >= 0)
                {
                    newWins++;
                }
            }
            if (newWins > wins)
            {
                wins = newWins;
            }
        }
        text.text = string.Format(Format, wins);
    }
}
