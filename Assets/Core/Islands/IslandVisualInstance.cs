using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IslandVisualInstance : MonoBehaviour
{
    [SerializeField]
    private Transform Player_Tansform;
    [field:SerializeField]
    private List<Question> Questions;
    [SerializeField]
    private QuestionsHandler Questions_Handler;
    [SerializeField]
    private InterfacesHandler Interface_Handler;
    [SerializeField]
    private int Target_Interface_ID;
    [SerializeField]
    private string Unlock_Key = "TEST_KEY";
    [SerializeField]
    private Transform Target_Position;
    [SerializeField]
    private SpriteRenderer Front_Icon_Reference;
    [SerializeField]
    private SpriteRenderer Preview_Icon_Reference;
    [SerializeField]
    private Sprite Unlocked_Front_Icon;
    [SerializeField]
    private Color Unlocked_Color;
    [SerializeField]
    private Sprite Completed_Front_Icon;
    [SerializeField]
    private Color Completed_Color;
    [SerializeField]
    private Sprite Unlocked_Preview_Icon;
    [SerializeField]
    private UnityEvent startEvent;
    public event Action OnWinEvent;
    public string Island_ID = "Island_1";
    private bool AwaitingVisit = false;
    private bool Available = true;
#if UNITY_EDITOR
    [SerializeField]
    private bool Debug = false;
    void Start()
    {
        if (Debug == true)
        {
            TriggerStart();
        }
    }
#endif
    public void UnlockVisual(bool won)
    {
        if (Front_Icon_Reference.sprite != Unlocked_Front_Icon && Front_Icon_Reference.sprite != Completed_Front_Icon)
        {
            Front_Icon_Reference.sprite = Unlocked_Front_Icon;
            Front_Icon_Reference.color = Unlocked_Color;
        }
        if (Preview_Icon_Reference.sprite != Unlocked_Preview_Icon)
        {
            Preview_Icon_Reference.sprite = Unlocked_Preview_Icon;
        }
        if (won)
        {
            Front_Icon_Reference.sprite = Completed_Front_Icon;
            Front_Icon_Reference.color = Completed_Color;
        }
    }
    public bool TryUnlock(string Key, out Transform pos)
    {
        pos = null;
        if (Unlock_Key == Key && Questions_Handler.CanStartRun(Island_ID))
        {
            if (Front_Icon_Reference.sprite != Unlocked_Front_Icon && Front_Icon_Reference.sprite != Completed_Front_Icon)
            {
                Front_Icon_Reference.sprite = Unlocked_Front_Icon;
                Front_Icon_Reference.color = Unlocked_Color;
            }
            if (Preview_Icon_Reference.sprite != Unlocked_Preview_Icon)
            {
                Preview_Icon_Reference.sprite = Unlocked_Preview_Icon;
            }
            pos = Target_Position;
            AwaitingVisit = Available;
            return true;
        }
        AwaitingVisit = false;
        return false;
    }
    void OnWin(float min, float max)
    {
        AwaitingVisit = false;
        Available = true;
        Front_Icon_Reference.sprite = Completed_Front_Icon;
        Front_Icon_Reference.color = Completed_Color;
        OnWinEvent?.Invoke();
        OnWinEvent = null;
    }
    void TriggerStart()
    {
        AwaitingVisit = false;
        Available = false;
        if (Questions_Handler.TryStartRun(Questions,Island_ID))
        {
            startEvent?.Invoke();
            Interface_Handler.OpenFrameByIndex(Target_Interface_ID);
            Questions_Handler.OnFinish.AddListener(OnWin);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (Available && AwaitingVisit && other != null && other.transform == Player_Tansform)
        {
            TriggerStart();
        }
    }
}
