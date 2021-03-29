using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "Step11_Event", menuName = "SceneEvent/Step11/Step11_Event")]
public class Step11Event : SceneEvent
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
    private bool gauzeCollided;
    private bool check;
    private UiController ui;

    private bool toolActivated;
    private bool bb;


    public SceneEvent nextScene;

    public override void InitEvent()
    {
        SceneAssetManager.GetAssetComponent("UIController", out ui);
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
        ui.UpdateData(9);
        holdingGauze = false;
        check = false;
        freezeGauze.SetActive(false);

        gauzeCollided = false;
        bb = false;

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

            //gauzeTrigger.OnCollisionEnterEvent += OnGauzeCollisionEnter;
            //gauzeTrigger.OnCollisionExitEvent += OnGauzeCollisionExit;

        }
        Debug.Log("มาเริ่ม อีเว้นท์ Step11 กันเถอะ");
       

    }

  

    public override void UpdateEvent()
    {

        if (equipment.IsActivate) toolActivated = true;
        if (!equipment.IsActivate) toolActivated = false;
        Debug.Log("เปิดใช้ อุปกรณ์ " + toolActivated);





        if (check == true)
        {

            Debug.Log("ผ่าน Event11 แล้วต้า");
               passEventCondition = true;

        }

        

        if (gauzeCollided && !toolActivated)
        {
            Debug.Log("ชนผ้าก๊อช อ้าปากด้วย และกำลังคีบ");
            guidance?.SetTarget(trigger.transform);
            holdingGauze = true;
            freezeAtScissorGauze.SetActive(true);
            gauzeTool.SetActive(false);

        }




    }
    //ชนกับฟัน
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
            trigger.gameObject.SetActive(false);
          

            check = true;

            Debug.Log("วางแล้ว");
        

    }
    }



    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;



    }

    //private void OnGauzeCollisionEnter(Collision gauzeCollision)
    //{
    //    if (gauzeCollision == null) return;



    //    Debug.Log("ซ้อนผ้าก๊อชเฉยๆ ไม่ได้คีบ");

    //    if (aa)
    //    {
    //        bb = true;
    //        Debug.Log("ซ้อนผ้าก๊อชเฉยๆ ไม่ได้คีบ");

    //    }



    //}

    //private void OnGauzeCollisionExit(Collision gauzeCollision)
    //{

    //}






    //ชนผ้าก๊อซ
    private void OnGauzeTriggerEnter(Collider gauzeCollider)
    {
        if (gauzeCollider == null) return;
        if (gauzeCollider.attachedRigidbody == null) return;

       

        if (gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject)
        {
            gauzeCollided = true;

            Debug.Log("ชนผ้าก๊อช");

        }


        

    }

    private void OnGauzeTriggerExit(Collider gauzeCollider)
    {
        if (gauzeCollider == null) return;
        if (gauzeCollider.attachedRigidbody == null) return;

        if (gauzeCollider.attachedRigidbody.gameObject == equipment.gameObject && !equipment.IsActivate)
        {
            gauzeCollided = false;

            Debug.Log("ไม่ชน");

        }

        gauzeCollided = false;

        guidance?.SetTarget(gauzeTrigger.transform);
        holdingGauze = false;

    }

    private void Update()
    {

      
    }



    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    
    public override void StopEvent()
    {
       // uiBoardText.gameObject.SetActive(false);

        //guidance?.SetTarget(null);
        //guidance?.SetParent(null);

       


    }


    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;

        if (interactable.gameObject == equipment.gameObject)
        {
        //    guidance?.SetParent(equipment.transform);
            holdingEquipment = true;
        }
    }

    private void OnReleased(XRBaseInteractor interactor)
    {
        holdingEquipment = false;
     //   guidance?.SetParent(null);
    }
}
