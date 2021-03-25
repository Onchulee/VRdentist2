using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;




[System.Serializable]
[CreateAssetMenu(fileName = "Step8_Event", menuName = "SceneEvent/Step8/Step8_Event")]
[RequireComponent(typeof(XRGrabInteractable))]
public class Step8Event : SceneEvent
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

        public GameObject a;
        public GameObject d;
        //เพิ่ม Bool ของวัตุถุที่วางไว้เช็คว่าเรากำลังถืออุปกรณ์ไหนอยู่บ้าง แล้วเมื่อมัน false ก็ให้ kenimatic เป็น false

    }
   
    public ToolSetup[] toolSetup;
    public SceneEvent nextScene;


    private Tracking[] trackedTools;
    private UiController ui;
    private List<XRGrabInteractable> grabInteractables = new List<XRGrabInteractable>();

    public override void InitEvent()
    {
        base.InitEvent();

        SceneAssetManager.GetAssetComponent("UIController", out ui);
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
                trackedTools[i].guidance?.SetParent(trackedTools[i].equipment.transform);
                trackedTools[i].hold = true;
                break;
            }
        }
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;

        for (int i = 0; i < trackedTools.Length; i++)
        {

            if (interactable.gameObject == trackedTools[i].equipment.gameObject && trackedTools[i].hold)
            {
                Debug.Log("ปล่อย " + i);
                trackedTools[i].equipment.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                trackedTools[i].guidance?.SetParent(null);
                trackedTools[i].hold = false;
            }

        }

        //for (int i = 0; i < trackedTools.Length; i++)
        //{

        //    if (interactable.gameObject == trackedTools[i].equipment.gameObject)
        //    {
        //        Debug.Log("ปล่อย " + i);
        //        trackedTools[i].equipment.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //        trackedTools[i].guidance?.SetParent(null);
        //        trackedTools[i].hold = false;
        //    }

        //}
    }



    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void StartEvent()
    {
        ui.UpdateData(6);
        for (int i = 0; i < trackedTools.Length; i++)
        {

            trackedTools[i].equipment.gameObject.SetActive(true);
            trackedTools[i].hold = false;
            trackedTools[i].freezeTool.gameObject.SetActive(false);
            trackedTools[i].equipment.gameObject.GetComponent<Rigidbody>().isKinematic = true;


            trackedTools[i].guidance?.SetParent(trackedTools[i].equipment.transform);
            trackedTools[i].guidance?.SetTarget(trackedTools[i].trigger.transform);


            if (trackedTools[i].trigger)
            {

                trackedTools[i].trigger.gameObject.SetActive(true);

                //trackedTools[0].trigger.OnTriggerEnterEvent += OnMoltEnter;
                //trackedTools[1].trigger.OnTriggerEnterEvent += OnSeldinEnter;
                //trackedTools[i].trigger.OnCollisionEnterEvent -= OnCollisionEnter;
                //trackedTools[i].trigger.OnCollisionExitEvent -= OnCollisionExit;

                trackedTools[i].trigger.OnTriggerEnterEvent += OnTriggerEnter;
                trackedTools[i].trigger.OnTriggerExitEvent += OnTriggerExit;
            }

        }
        Debug.Log("มาเริ่ม อีเว้นท์ Step8 กันเถอะ");

    }

    public override void StopEvent()
    {
        

        for (int i = 0; i < trackedTools.Length; i++)
        {
            trackedTools[i].guidance?.SetTarget(null);
            trackedTools[i].guidance?.SetParent(null);
            trackedTools[i].hold = false;

            trackedTools[i].trigger.gameObject.SetActive(false);
            trackedTools[i].trigger.OnTriggerEnterEvent -= OnTriggerEnter;
            trackedTools[i].trigger.OnTriggerExitEvent -= OnTriggerExit;
            

        }
        

        grabInteractables.Clear();
    }

    public override void UpdateEvent()
    {
        int placedToolCount = 0;

        if (trackedTools[0].hold)
        {
            trackedTools[0].equipment.gameObject.GetComponent<Rigidbody>().isKinematic = false;


        }

        if (trackedTools[1].hold)
        {
            trackedTools[1].equipment.gameObject.GetComponent<Rigidbody>().isKinematic = false;


        }

        foreach (Tracking tool in trackedTools)
        {
            if (tool.check == true)
            {
                placedToolCount += 1;
            }
        }
        if (placedToolCount >= trackedTools.Length)
        {
            Debug.Log("จบ Event step8 แล้วจ้าาาา ");
            passEventCondition = true;  // Uncomment if you want system to done here
        }


    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        for (int i = 0; i < trackedTools.Length; i++)
        {
            if (collider.attachedRigidbody.gameObject == trackedTools[i].equipment.gameObject)
            {
                Debug.Log("วาง");
                trackedTools[i].check = true;
                break;
            }
            trackedTools[i].guidance?.SetParent(null);
        }


    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        for (int i = 0; i < trackedTools.Length; i++)
        {
            if (collider.attachedRigidbody.gameObject == trackedTools[i].equipment.gameObject)
            {
                Debug.Log("ยก");
                trackedTools[i].check = false;
                break;
            }
        }
    }

}
