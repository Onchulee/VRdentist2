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
    public string freezeGauzeName;
    public string freezeGauzeAtScissorName;
    public string triggerName;
    public string gauzeTriggerName;
    public string guidanceName;
        

    private GrabbableEquipmentBehavior equipment;
    private GameObject gauzeTool;
    private GameObject freezeGauze;
    private GameObject freezeAtScissorGauze;
    private CollisionTrigger trigger;
    private CollisionTrigger gauzeTrigger;
    private PathGuidance guidance;
        
    
    private bool holdingEquipment;
    private bool holdingGauze;
    private bool check;
    


    public SceneEvent nextScene;

    public override void InitEvent()
    {
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(toolName, out equipment);
        SceneAssetManager.GetGameObjectAsset(gauzeToolName, out gauzeTool);
        SceneAssetManager.GetGameObjectAsset(freezeGauzeName, out freezeGauze);
        SceneAssetManager.GetGameObjectAsset(freezeGauzeAtScissorName, out freezeAtScissorGauze);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(triggerName, out trigger);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(gauzeTriggerName, out gauzeTrigger);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);


        XRGrabInteractable interactable = equipment.GetComponent<XRGrabInteractable>();
        interactable.onSelectEntered.AddListener(OnGrabbed);
        interactable.onSelectExited.AddListener(OnReleased);


    }
    public override void StartEvent()
    {
        holdingGauze = false;
        check = false;

        freezeGauze.SetActive(false);
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

        }
        Debug.Log("มาเริ่ม อีเว้นท์ Step4 กันเถอะ");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;
        

        if (collider.attachedRigidbody.gameObject == equipment.gameObject && holdingGauze )
        {
            freezeGauze.SetActive(true);
            gauzeTool.SetActive(false);
            guidance?.SetTarget(null);
            freezeAtScissorGauze.SetActive(false);
            check = true;
            Debug.Log(check);
            Debug.Log("วางแล้ว");
        

    }
    }



    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;



    }

    private void OnGauzeTriggerEnter(Collider gauzeCollider )
    {
        if (gauzeCollider == null) return;
        if (gauzeCollider.attachedRigidbody == null) return;


      
        //if(gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject && !equipment.IsActivate)
        //{
           
        //    Debug.Log("ชนผ้าก๊อชเฉยๆ ไม่ได้คีบ");

        //}

        if (gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject && equipment.IsActivate)
        {

            guidance?.SetTarget(trigger.transform);

            holdingGauze = true;

            freezeAtScissorGauze.SetActive(true);
            gauzeTool.SetActive(false);

           // HoldingGauze();
            

        }

    }



    private void OnGauzeTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        guidance?.SetTarget(gauzeTrigger.transform);
        holdingGauze = false;
        HoldingGauze();
        


    }



    // คีบ
    private void HoldingGauze()
    {
        

        if (holdingGauze && equipment.IsActivate)
        {
            gauzeTool.transform.SetParent(equipment.transform);
            gauzeTool.GetComponent<Rigidbody>().isKinematic = true;
            
           
            Debug.Log("คีบ");


        }


        //if (!holdingGauze)
        //{
        //    gauzeTool.transform.SetParent(null);
        //    gauzeTool.GetComponent<Rigidbody>().isKinematic = false;

          


        //}


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
        

        

       
        if (check == true)
        {

            Debug.Log("ผ่าน Event10 แล้วต้า");
            passEventCondition = true;
            


        }
    }

    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;

        if (interactable.gameObject == equipment.gameObject)
        {
            guidance?.SetParent(equipment.transform);
            holdingEquipment = true;
        }
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        holdingEquipment = false;
        guidance?.SetParent(null);
    }
}
