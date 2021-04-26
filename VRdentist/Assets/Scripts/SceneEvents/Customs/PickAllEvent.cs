using com.dgn.SceneEvent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "PickAllEvent", menuName = "SceneEvent/Test02/PickAllEvent")]
[RequireComponent(typeof(XRGrabInteractable))]
public class PickAllEvent : SceneEvent
{
    [System.Serializable]
    public struct ToolSetup
    {
        public string toolName;
        public string detailTextR;
        public string detailTextL;
        public string checkText;
       
    }


    [System.Serializable]
    public struct Tracking
    {
        public GrabbableEquipmentBehavior equipment;
        public GameObject detailWindowRight;
        public GameObject detailWindowLeft;
        public GameObject checkText;
        public bool check;
        

    }

    

    public string collisionTriggerName;
    public ToolSetup[] toolSetup;
    public string numOfToolsTextName;
    public string missionClearTextName;
    public string guidanceRightName = "PathGuidance";
    public string guidanceLeftName = "PathGuidance2";
    public SceneEvent nextScene;

    private Tracking[] trackedTools;
    private Text numOfTools;
    private GameObject missionClearText;
    private CollisionTrigger trigger;
    private PathGuidance guidance_left;
    private PathGuidance guidance_right;


    private List<XRGrabInteractable> grabInteractables = new List<XRGrabInteractable>();

    


    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponentInChildren(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent(guidanceRightName, out guidance_right);
        SceneAssetManager.GetAssetComponent(guidanceLeftName, out guidance_left);
        SceneAssetManager.GetAssetComponent(numOfToolsTextName, out numOfTools);
        SceneAssetManager.GetGameObjectAsset(missionClearTextName, out missionClearText);

        List<Tracking>  trackedList = new List<Tracking>();
        foreach (ToolSetup config in toolSetup)
        {
            if (SceneAssetManager.GetAssetComponent(config.toolName, out GrabbableEquipmentBehavior targetObject))
            {
                Tracking newTrack = new Tracking
                {
                    equipment = targetObject,
                    check = false
                };


                if (SceneAssetManager.GetGameObjectAsset(config.detailTextR, out GameObject i_detailWindowRight))
                {
                    newTrack.detailWindowRight = i_detailWindowRight;
                }
                if (SceneAssetManager.GetGameObjectAsset(config.detailTextL, out GameObject i_detailWindowLeft))
                {
                    newTrack.detailWindowLeft = i_detailWindowLeft;
                }



                if (SceneAssetManager.GetGameObjectAsset(config.checkText, out GameObject i_checkWindow))
                {
                    newTrack.checkText = i_checkWindow;
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

    public override void StartEvent()
    {
        guidance_right?.SetTarget(trigger.transform);
        guidance_left?.SetTarget(trigger.transform);
        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log("มาเริ่ม อีเว้นท์ PickAll กันเถอะ");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }
    }

    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        XRController controller = interactor.GetComponent<XRController>();

        if (interactable == null) return;
       

        //มือขวา
        if (controller.controllerNode == UnityEngine.XR.XRNode.RightHand)
        {
            Debug.Log("ขวา");
            for (int i = 0; i < trackedTools.Length; i++)
            {

                if (interactable.gameObject == trackedTools[i].equipment.gameObject)
                {
                    Debug.Log("จับขวา " + i);
                    trackedTools[i].detailWindowRight.SetActive(true);
                    guidance_right?.SetParent(trackedTools[i].equipment.transform);
                    break;
                }
            }
        }


        //มือซ้าย
        if (controller.controllerNode == UnityEngine.XR.XRNode.LeftHand)
        {
            Debug.Log("ซ้าย");
            for (int i = 0; i < trackedTools.Length; i++)
            {
                if (interactable.gameObject == trackedTools[i].equipment.gameObject)
                {
                    Debug.Log("จับซ้าย " + i);
                    trackedTools[i].detailWindowLeft.SetActive(true);
                    guidance_left?.SetParent(trackedTools[i].equipment.transform);
                    break;
                }
            }
        }
        
        //for (int i=0; i<trackedTools.Length; i++) {

        //    if (interactable.gameObject == trackedTools[i].equipment.gameObject)
        //    {
        //        Debug.Log("จับ "+i);
        //        trackedTools[i].detailWindow.SetActive(true);
        //        guidance?.SetParent(trackedTools[i].equipment.transform);
        //        break;
        //    }
        //}
    }



    private void OnReleased(XRBaseInteractor interactor)
    {
        XRController controller = interactor.GetComponent<XRController>();


        if (controller.controllerNode == UnityEngine.XR.XRNode.RightHand)
        {
            foreach (Tracking trackedTool in trackedTools)
            {
                trackedTool.detailWindowRight.SetActive(false);
            }
            guidance_right?.SetParent(null);
        }


        if (controller.controllerNode == UnityEngine.XR.XRNode.LeftHand)
        {
            foreach (Tracking trackedTool in trackedTools)
            {
                trackedTool.detailWindowLeft.SetActive(false);
            }
            guidance_left?.SetParent(null);
        }
        
    }
    
    public override void UpdateEvent()
    {
        int placedToolCount = 0;
        foreach(Tracking tool in trackedTools)
        {
            if (tool.check == true) {
                placedToolCount += 1;
            }
        }
        numOfTools.text = placedToolCount.ToString();
        if (placedToolCount >= trackedTools.Length)
        {
            missionClearText.gameObject.SetActive(true);
            passEventCondition = true;  // Uncomment if you want system to done here
        }
        else
        {
            missionClearText.gameObject.SetActive(false);
        }
    }

    public override void StopEvent()
    {
        foreach (XRGrabInteractable interactable in grabInteractables) {
            if (interactable)
            {
                interactable.onSelectEntered.RemoveListener(OnGrabbed);
                interactable.onSelectExited.RemoveListener(OnReleased);
            }
        }
        grabInteractables.Clear();

        guidance_right?.SetTarget(null);
        guidance_left?.SetParent(null);

        if (trigger)
        {
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void Pause()
    {

    }

    public override void UnPause()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;

        for (int i = 0; i < trackedTools.Length; i++)
        {
            if (collision.rigidbody.gameObject == trackedTools[i].equipment.gameObject)
            {
                trackedTools[i].checkText.gameObject.SetActive(true);
                trackedTools[i].check = true;
                break;
            }
        }

        guidance_right?.SetParent(null);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody == null) return;
        
        for (int i = 0; i < trackedTools.Length; i++){
            if (collision.rigidbody.gameObject == trackedTools[i].equipment.gameObject)
            {
                trackedTools[i].checkText.gameObject.SetActive(false);
                trackedTools[i].check = false;
                break;
            }
        }
    }
    

    public override void OnDestroy()
    {
        foreach (XRGrabInteractable interactable in grabInteractables)
        {
            interactable.onSelectEntered.RemoveListener(OnGrabbed);
            interactable.onSelectExited.RemoveListener(OnReleased);
        }
        grabInteractables.Clear();

        if (trigger)
        {
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }
}
