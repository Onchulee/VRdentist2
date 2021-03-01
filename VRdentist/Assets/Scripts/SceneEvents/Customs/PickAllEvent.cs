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
        public string detailText;
        public string checkText;
    }


    [System.Serializable]
    public struct Tracking
    {
        public GrabbableEquipmentBehavior equipment;
        public GameObject detailWindow;
        public GameObject checkText;
        public bool check;
    }

    

    public string collisionTriggerName;

    //public string[] ToolsNames;
    //public string[] ToolsDetailText;

    public ToolSetup[] toolSetup;
    public string numOfToolsTextName;
    public string missionClearTextName;
    public string guidanceName = "PathGuidance";
    public SceneEvent nextScene;

    private Tracking[] trackedTools;
    private Text numOfTools;
    private Text missionClearText;

    //private List<Tracking> trackedTools;
    //private List<GameObject> texts;
    private CollisionTrigger trigger;
    private PathGuidance guidance;


    private List<XRGrabInteractable> grabInteractables = new List<XRGrabInteractable>();

    


    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponentInChildren(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent(guidanceName, out guidance);

        SceneAssetManager.GetAssetComponent(numOfToolsTextName, out numOfTools);
        SceneAssetManager.GetAssetComponent(missionClearTextName, out missionClearText);

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
                if (SceneAssetManager.GetGameObjectAsset(config.detailText, out GameObject i_detailWindow))
                {
                    newTrack.detailWindow = i_detailWindow;
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

        //texts = new List<GameObject>();
        //foreach (string targetName in ToolsDetailText)
        //{
        //    if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject))
        //    {
        //        targetObject.SetActive(false); // hide at begin
        //        texts.Add(targetObject);
        //    }
        //}


        /*  Debug.Log("Found Asset[Tools]: " + (tools.Count > 0));

         */
    }

    public override void StartEvent()
    {
        guidance?.SetTarget(trigger.transform);
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
        if (interactable == null) return;

        for (int i=0; i<trackedTools.Length; i++) {

            if (interactable.gameObject == trackedTools[i].equipment.gameObject)
            {
                Debug.Log("จับ "+i);
                trackedTools[i].detailWindow.SetActive(true);
                guidance?.SetParent(trackedTools[i].equipment.transform);
                break;
            }
        }
    }



    private void OnReleased(XRBaseInteractor interactor)
    {
        //XRBaseInteractable interactable = interactor.selectTarget;
        //  if (interactable == null) return;
        //if (interactor == null) return;

        foreach (Tracking trackedTool in trackedTools) {
            trackedTool.detailWindow.SetActive(false);
        }

        

        guidance?.SetParent(null);
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
            //passEventCondition = true;  // Uncomment if you want system to done here
        }
        else
        {
            missionClearText.gameObject.SetActive(false);
        }
    }

    public override void StopEvent()
    {
        foreach (XRGrabInteractable interactable in grabInteractables) {
            interactable.onSelectEntered.RemoveListener(OnGrabbed);
            interactable.onSelectExited.RemoveListener(OnReleased);
        }
        grabInteractables.Clear();

        guidance?.SetTarget(null);
        guidance?.SetParent(null);

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

        guidance?.SetParent(null);
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
