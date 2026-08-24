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
    void UpdateText(int _)
    {
        int wins = 0;
        foreach (var values in handler.Points)
        {
            if (values.Value > 0)
            {
                wins++;
            }
        }
        text.text = string.Format(Format, wins);
    }
}
