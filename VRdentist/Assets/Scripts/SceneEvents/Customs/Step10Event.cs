using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "Step10Event", menuName = "SceneEvent/Step10/Step10Event")]
public class Step10Event : SceneEvent
{
    
        public string toolName;
        public string gauzeToolName;
        public string triggerName;
        public string gauzeTriggerName;
        public string guidanceName;
        

        private GrabbableEquipmentBehavior equipment;
        private GameObject gauzeTool;
        private CollisionTrigger trigger;
        private CollisionTrigger gauzeTrigger;
        private PathGuidance guidance;
        
    //ต้องมี bool ที่เอาไว้เช็คว่าคีบผ้าก๊อซด้วย
        private bool hold;
        private bool check;
    


    public SceneEvent nextScene;

    public override void InitEvent()
    {
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(toolName, out equipment);
        SceneAssetManager.GetGameObjectAsset(gauzeToolName, out gauzeTool);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(triggerName, out trigger);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(gauzeTriggerName, out gauzeTrigger);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);


        XRGrabInteractable interactable = equipment.GetComponent<XRGrabInteractable>();
        interactable.onSelectEntered.AddListener(OnGrabbed);
        interactable.onSelectExited.AddListener(OnReleased);


    }
    public override void StartEvent()
    {
       
        guidance?.SetTarget(gauzeTrigger.transform);
        guidance?.SetParent(equipment.transform);
        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            trigger.OnTriggerEnterEvent += OnTriggerEnter;
            trigger.OnTriggerExitEvent += OnTriggerExit;
        }


        if (gauzeTrigger)
        {
            gauzeTrigger.gameObject.SetActive(true);
            gauzeTrigger.OnTriggerEnterEvent += OnGauzeTriggerEnter;
            gauzeTrigger.OnTriggerExitEvent += OnGauzeTriggerExit;
            Debug.Log(gauzeTrigger);
        }
        Debug.Log("มาเริ่ม อีเว้นท์ Step4 กันเถอะ");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;


        if (collider.attachedRigidbody.gameObject == equipment.gameObject /*คีบ == true*/)
        {
            //อุปกรณ?+ผ้าก๊อช ชนกับจุดเป้าหมาย
            Debug.Log("");

        }
    }



    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;



    }

    private void OnGauzeTriggerEnter(Collider gauzeCollider)
    {
        if (gauzeCollider == null) return;
        if (gauzeCollider.attachedRigidbody == null) return;


      
        if(gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject && !equipment.IsActivate)
        {
           
            Debug.Log("ชนผ้าก๊อชเฉยๆ ไม่ได้คีบ");

        }

        if (gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject && equipment.IsActivate)
        {
            guidance?.SetTarget(trigger.transform);
            Debug.Log("คีบ");
            // ฟังชั่นคีบ ทำงาน
            // คีบ == true
        }

    }



    private void OnGauzeTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        

    }


























    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;

        if (interactable.gameObject == equipment.gameObject)
        {
            guidance?.SetParent(equipment.transform);
            hold = true;
        }
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        hold = false;
        guidance?.SetParent(null);
    }

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    
    public override void StopEvent()
    {
       
    }

    public override void UpdateEvent()
    {
       
    }

    // คีบ
   private void HoldingGauze()
    {



    }
}
