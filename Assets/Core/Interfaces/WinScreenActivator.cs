using UnityEngine;

public class WinScreenActivator : MonoBehaviour
{
    [SerializeField]
    private GlobalScoreCounter counter;
    [SerializeField]
    private InterfacesHandler interfacesHandler;
    [SerializeField]
    private int TargetCount;
    [SerializeField]
    private int TargetFrame;
    private bool Triggered = false;
    private int currentCount = 0;
    void Start()
    {
        counter.OnWinsChange += UpdateWinCount;
    }
    void UpdateWinCount(int count)
    {
        currentCount = count;
    }
    public void Trigger()
    {
        if (Triggered) return;
        if (currentCount >= TargetCount)
        {
            Triggered = true;
            interfacesHandler.OpenFrameByIndex(TargetFrame);
        }
    }
}
