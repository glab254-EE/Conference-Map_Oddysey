using System;
using System.Collections.Generic;
using UnityEngine;

public class InterfacesHandler : MonoBehaviour
{
    [SerializeField] private List<AInterfaceView> frames;
    [SerializeField] public AInterfaceView currentFrame;
    public void OpenFrameByIndex(int index)
    {
        if (index >= frames.Count || index < 0) return;
        currentFrame?.ToggleFrames(false);
        currentFrame = frames[index];
        currentFrame.ToggleFrames(true);
    }
}
