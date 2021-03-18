using com.dgn.SceneEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "PickupEvent", menuName = "SceneEvent/Test/PickupEvent")]
//[RequireComponent(typeof(XRGrabInteractable))]
public class PickupEvent : SceneEvent
{
    private XRGrabInteractable grabInteractable;

    public string collisionTriggerName;
    public string targetItemName;
    public string guidanceName = "PathGuidance";
    public string progressTextName;
    public SceneEvent nextScene;


    private CollisionTrigger trigger;
    private GrabbableEquipmentBehavior targetItem;
    private PathGuidance guidance;
    private Text progressText;



    private bool isCollided;

    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(targetItemName, out targetItem);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);
        bool foundText = SceneAssetManager.GetAssetComponent<Text>(progressTextName, out progressText);
        if (nextScene) nextScene.InitEvent();





        Debug.Log("Found Asset[" + progressTextName + "]: " + foundText);

    }

    public override void StartEvent()
    {
        isCollided = false;
        guidance?.SetParent(targetItem.transform);
        guidance?.SetTarget(trigger.transform);
        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }



        if (progressText)
        {

            progressText.gameObject.SetActive(false);
          //  Debug.Log("ProgessText");


        }
        grabInteractable = targetItem.GetComponent<XRGrabInteractable>();

        
        grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        grabInteractable.onSelectExited.AddListener(OnReleased);



    }

    private void OnGrabbed(XRBaseInteractor arg0)
    {
        Debug.Log("Grab");
        progressText.gameObject.SetActive(true);
    }



    private void OnReleased(XRBaseInteractor arg0)
    {
        Debug.Log("OnReleased");
        progressText.gameObject.SetActive(false);
    }

    

    public override void UpdateEvent()
    {
        if (targetItem /*&& targetItem.IsActivate */&& isCollided)
        {
            passEventCondition = true;
        }
       
        if (targetItem )
        {
           // progressText.gameObject.SetActive(true);
           
        }


    }

    public override void StopEvent()
    {
        if (trigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
          //  trigger.gameObject.SetActive(false);
        }

        if (progressText)
        {
            progressText.gameObject.SetActive(false);
        }

        guidance?.SetTarget(null);
        guidance?.SetParent(null);
        Debug.Log("Stop event: " + this.name);

        grabInteractable.onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable.onSelectExited.RemoveListener(OnReleased);
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
       
        Debug.Log("CollisionTriggerEvent call: " + collision.gameObject.name);

        //ตรงนี้ไม่ต้อง
        ///collision.gameObject คือ area ที่ต้องเอาของไปวาง
        ///targetItem เครื่องมือแพทย์

        if (collision.gameObject == targetItem.gameObject)
        {
            if (isCollided == false) Debug.Log(targetItem.name + "is Collided");
            isCollided = true;
          //  Debug.Log("ชน");

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetItem.gameObject)
        {
            isCollided = false;
          //  Debug.Log("ไม่ชน");
        }
    }

    public override void OnDestroy()
    {
        if (trigger)
        {
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }
}
