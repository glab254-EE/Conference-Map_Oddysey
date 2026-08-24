using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoseFrameHandler : MonoBehaviour
{
    [SerializeField]
    private QuestionsHandler _questHandler;
    [SerializeField]
    private List<UnityEvent> TakesCountToText;
    void Start()
    {
        _questHandler.OnLose.AddListener(OnEnd);
    }
    void OnEnd(int takes)
    {
        if (takes < 0 || takes > TakesCountToText.Count)
        {
            return;
        }
        TakesCountToText[takes]?.Invoke();
    }
}
