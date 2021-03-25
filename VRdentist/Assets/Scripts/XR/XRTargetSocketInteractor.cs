using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTargetSocketInteractor : XRSocketInteractor
{
    public List<XRBaseInteractable> allowInteractables = new List<XRBaseInteractable>();

    public override bool CanHover(XRBaseInteractable interactable) {
        if (selectTarget == interactable) return true;
        if (allowInteractables.Contains(interactable)) {
            return base.CanHover(interactable);
        }
        return false;
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        if (selectTarget == interactable) return true;
        if (allowInteractables.Contains(interactable))
        {
            return base.CanSelect(interactable);
        }
        return false;
    }
}
