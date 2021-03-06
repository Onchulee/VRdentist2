﻿using com.dgn.XR.Extensions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class HideHandAfterGrabbed : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private HandPresence handPresence;
    private bool recordShowController;
    private bool recordShowHand;
    
    // Start is called before the first frame update
    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        grabInteractable.onSelectExited.AddListener(OnReleased);
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor)
    {
        HandPresence rHandPresence = rBaseInteractor.attachTransform.GetComponentInChildren<HandPresence>();
        if (rHandPresence) {
            if (handPresence && handPresence!=rHandPresence) {
                handPresence.showController = recordShowController;
                handPresence.showHand = recordShowHand;
            }

            handPresence = rHandPresence;
            recordShowController = handPresence.showController;
            recordShowHand = handPresence.showHand;
            handPresence.showController = false;
            handPresence.showHand = false;
        }
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        if (handPresence)
        {
            handPresence.showController = recordShowController;
            handPresence.showHand = recordShowHand;
        }
        handPresence = null;
    }

    private void OnDestroy()
    {
        if (grabInteractable) {
            grabInteractable.onSelectEntered.RemoveListener(OnGrabbed);
            grabInteractable.onSelectExited.RemoveListener(OnReleased);
        }
    }

}
