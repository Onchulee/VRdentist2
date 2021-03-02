﻿using com.dgn.SceneEvent;
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
        public string freezeToolsName;


    }

    [System.Serializable]
    public struct Tracking
    {
        public GrabbableEquipmentBehavior equipment;
        public CollisionTrigger trigger;
        public PathGuidance guidance;
        public GameObject freezeTool;
        public bool hold;
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
                    check = false,
                    hold = false
                };
                if (SceneAssetManager.GetAssetComponentInChildren(config.triggerName, out CollisionTrigger i_trigger))
                {
                    newTrack.trigger = i_trigger;
                }

                if (SceneAssetManager.GetAssetComponent(config.guidanceName, out PathGuidance i_guidance))
                {
                    newTrack.guidance = i_guidance;
                }

                if (SceneAssetManager.GetGameObjectAsset(config.freezeToolsName, out GameObject i_freezeTool))
                {
                    newTrack.freezeTool = i_freezeTool;
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

              //  trackedTools[i].freezeTool.SetActive(true);

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

        //    trackedTools[i].freezeTool.SetActive(false);
        }
    }



    


    
    public override void StartEvent()
    {


        Debug.Log(trackedTools.Length);

    
        for (int i = 0; i < trackedTools.Length; i++)
        {

            Debug.Log(trackedTools[i].freezeTool);

            //ทำให้อุปกรณ์ มองไม่เห็น
            trackedTools[i].freezeTool.SetActive(false);
        //    trackedTools[1].freezeTool.SetActive(false);



            trackedTools[i].guidance?.SetTarget(trackedTools[i].trigger.transform);
          

            if (trackedTools[i].trigger)
            {
                
                trackedTools[i].trigger.gameObject.SetActive(true);
                trackedTools[i].trigger.OnCollisionEnterEvent += OnCollisionEnter;
                trackedTools[i].trigger.OnCollisionExitEvent += OnCollisionExit;
                
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
        int triggleToolCount = 0;
        foreach (Tracking tool in trackedTools)
        {
            if (tool.check == true)
            {
                triggleToolCount += 1;
            }
        }

        if (triggleToolCount >= trackedTools.Length)
        {
            Debug.Log("จบ Event step4 แล้วจ้าาาา ");
            passEventCondition = true;  // Uncomment if you want system to done here
        }




    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;


       


        for (int i = 0; i < trackedTools.Length; i++)
        {

            if (collision.gameObject == trackedTools[i].equipment.gameObject)
            {
                Debug.Log("ชนกัน");
                trackedTools[i].freezeTool.SetActive(true);


                trackedTools[i].trigger.gameObject.SetActive(false);
                trackedTools[i].equipment.gameObject.SetActive(false);
                trackedTools[i].check = true;
                break;
            }




        }

    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject == trackedTools[1].equipment.gameObject)
        {
           // trackedTools[1].freezeTool.SetActive(false);
           


        }
    }

    private void OntriggerEnter(Collider other)
    {



    }
}
