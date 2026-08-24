using UnityEngine;

public abstract class AInterfaceView : MonoBehaviour
{
    [SerializeField] protected GameObject[] Frames;
    protected virtual void Open()
    {
        foreach (GameObject frame in Frames)
        {
            frame?.SetActive(true);
        }
    }
    protected virtual void Close()
    {
        foreach (GameObject frame in Frames)
        {
            frame?.SetActive(false);
        }
    }
    public void ToggleFrames(bool state)
    {
        if (state)  Open();
        else Close();
    }
}
