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
    void Start()
    {
        counter.OnWinsChange += Trigger;
    }
    void Trigger(int count)
    {
        if (Triggered) return;
        if (count >= TargetCount)
        {
            interfacesHandler.OpenFrameByIndex(TargetFrame);
        }
    }
}
