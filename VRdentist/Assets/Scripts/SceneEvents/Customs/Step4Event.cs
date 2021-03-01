using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "Step4Event", menuName = "SceneEvent/Step4/Step4Event")]
[RequireComponent(typeof(XRGrabInteractable))]
public class Step4Event : SceneEvent
{
    [System.Serializable]
    public struct ToolSetup
    {
        public string toolName;
        public string triggerName;
        public string guidanceName;
        

    }

    [System.Serializable]
    public struct Tracking
    {
        public GrabbableEquipmentBehavior equipment;
        public CollisionTrigger trigger;
        public PathGuidance guidance;
        public bool check;
        
    }

   




    public ToolSetup[] toolSetup;
  
   
    public SceneEvent nextScene;



    private Tracking[] trackedTools;
   
    private List<XRGrabInteractable> grabInteractables = new List<XRGrabInteractable>();

    public override void InitEvent()
    {
        base.InitEvent();
     

        List<Tracking> trackedList = new List<Tracking>();
        foreach (ToolSetup config in toolSetup)
        {
            if (SceneAssetManager.GetAssetComponent(config.toolName,
                out GrabbableEquipmentBehavior targetObject))
            {
                Tracking newTrack = new Tracking
                {
                    equipment = targetObject,
                    check = false
                };
                if (SceneAssetManager.GetAssetComponentInChildren(config.triggerName, out CollisionTrigger i_trigger))
                {
                    newTrack.trigger = i_trigger;
                }

                if (SceneAssetManager.GetAssetComponent(config.guidanceName, out PathGuidance i_guidance))
                {
                    newTrack.guidance = i_guidance;
                }



                trackedList.Add(newTrack);

                XRGrabInteractable interactable = targetObject.GetComponent<XRGrabInteractable>();
                interactable.onSelectEntered.AddListener(OnGrabbed);
                interactable.onSelectExited.AddListener(OnReleased);
                grabInteractables.Add(interactable);
            }
        }
        trackedTools = trackedList.ToArray();

       
    }

    
    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;




        for (int i = 0; i < trackedTools.Length; i++)
        {

            if (interactable.gameObject == trackedTools[i].equipment.gameObject)
            {
                Debug.Log("จับ " + i);

               trackedTools[i].guidance?.SetParent(trackedTools[i].equipment.transform);

                break;
            }
        }
    }


    private void OnReleased(XRBaseInteractor interactor)
    {

        foreach (Tracking trackedTool in trackedTools)
        {
          // 
        }

        for (int i = 0; i < trackedTools.Length; i++)
        {
            trackedTools[i].guidance?.SetParent(null);
        }
    }



    


    
    public override void StartEvent()
    {


        Debug.Log(trackedTools.Length);

    
        for (int i = 0; i < trackedTools.Length; i++)
        {
            
           trackedTools[i].guidance?.SetTarget(trackedTools[i].trigger.transform);
          

            if (trackedTools[i].trigger)
            {
                
                trackedTools[i].trigger.gameObject.SetActive(true);
              //  trackedTools[i].trigger.OnCollisionEnterEvent += OnCollisionEnter;
             //   trackedTools[i].trigger.OnCollisionExitEvent += OnCollisionExit;
                
            }
            
        }
        Debug.Log("มาเริ่ม อีเว้นท์ Step4 กันเถอะ");

    }

    public override void StopEvent()
    {
        foreach (XRGrabInteractable interactable in grabInteractables)
        {
            interactable.onSelectEntered.RemoveListener(OnGrabbed);
            interactable.onSelectExited.RemoveListener(OnReleased);
        }
        grabInteractables.Clear();

        for (int i = 0; i < trackedTools.Length; i++)
        {
            trackedTools[i].guidance?.SetTarget(null);
            trackedTools[i].guidance?.SetParent(null);
        }
    }

    public override void UpdateEvent()
    {
       
    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

}
