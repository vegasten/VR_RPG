using System;
using UnityEngine.XR.Interaction.Toolkit;

public class BehindTheHeadGrabZone : XRBaseInteractable
{
    public Action Selected;
    public Action Released;

    public void WasGrabbed()
    {
        Selected?.Invoke();
    }

    public void WasReleased()
    {
        Released?.Invoke();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Selected?.Invoke();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        Released?.Invoke();
    }
}
