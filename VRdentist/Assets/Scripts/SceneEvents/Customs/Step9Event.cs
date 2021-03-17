using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;





[System.Serializable]
[CreateAssetMenu(fileName = "Step9Event", menuName = "SceneEvent/Step9/Step9Event")]
public class Step9Event : SceneEvent
{

    public string toolName;
    public string triggerName;
    public string guidanceName;
    public string waterParticleName;
    public string progressTextName;
    public float actionTime;
    


    private GrabbableEquipmentBehavior equipment;
    private CollisionTrigger trigger;
    private PathGuidance guidance;
    private GameObject waterObject;
    private Text progressText;
    

    private bool hold;
    private bool check;
    private float progressTime;
    private float delayEndProgress;
    private bool isCollided;

    public SceneEvent nextScene;

   
     

    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(toolName, out equipment);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(triggerName, out trigger);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);
        SceneAssetManager.GetGameObjectAsset(waterParticleName, out waterObject);
        SceneAssetManager.GetAssetComponent<Text>(progressTextName, out progressText);

       

        if (nextScene) nextScene.InitEvent();

        XRGrabInteractable interactable = equipment.GetComponent<XRGrabInteractable>();
        interactable.onSelectEntered.AddListener(OnGrabbed);
        interactable.onSelectExited.AddListener(OnReleased);
    }

    public override void StartEvent()
    {
        isCollided = false;
        progressTime = 0;
        delayEndProgress = 2f;
        guidance?.SetTarget(trigger.transform);
       
        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
          

            trigger.OnTriggerEnterEvent += OnTriggerEnter;
             trigger.OnTriggerExitEvent += OnTriggerExit;
        }

        Debug.Log("มาเริ่ม อีเว้นท์ Step9 กันเถอะ");


        

    }



    private void OnGrabbed(XRBaseInteractor interactor)
    {
        XRBaseInteractable interactable = interactor.selectTarget;
        if (interactable == null) return;

        if(interactable.gameObject == equipment.gameObject)
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

    

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        if (collider.attachedRigidbody.gameObject == equipment.gameObject)
        {
            // progressText.gameObject.SetActive(true);
            isCollided = true;
            Debug.Log("ชนนะ");

        }
      
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == null) return;
        if (collider.attachedRigidbody == null) return;

        if (collider.attachedRigidbody.gameObject == equipment.gameObject)
        {
            // progressText.gameObject.SetActive(true);
            isCollided = false;
            Debug.Log("ไม่ชน");

        }
        
    }
    

    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

   

    public override void StopEvent()
    {
        guidance?.SetTarget(null);
        guidance?.SetParent(null);

        waterObject.gameObject.SetActive(false);
        if (trigger)
        {
            trigger.OnTriggerEnterEvent -= OnTriggerEnter;
            trigger.OnTriggerExitEvent -= OnTriggerExit;
            trigger.gameObject.SetActive(false);
        }
        if (progressText)
        {
            progressText.gameObject.SetActive(false);
        }
        Debug.Log("Stop event: " + this.name);
    }

    public override void UpdateEvent()
    {
        Injection();
        if (equipment && equipment.IsActivate && isCollided)
        {
            progressText.gameObject.SetActive(true);
            progressTime += Time.deltaTime;
            progressText.text = GetProgressString();

            // ต้องทำพุ่นน้ำออกมา
            Injection();
        }

        if (!isCollided)
        {
            progressText.gameObject.SetActive(false);
            
        }

        if (actionTime < progressTime)
        {
            
            //ผ่านด่าน
            
            
            guidance?.SetParent(null);
            guidance?.SetTarget(null);
            waterObject.gameObject.SetActive(false);
            delayEndProgress -= Time.deltaTime;
            progressText.gameObject.SetActive(false);
            trigger.gameObject.SetActive(false);
           
        }
        passEventCondition = (actionTime < progressTime && delayEndProgress < 0);
        if(passEventCondition == true)
        {
            Debug.Log("จบ Event step9 แล้วจ้าาาา ");

        }
       
        Debug.Log(isCollided);
        
    }

    private string GetProgressString()
    {
        return Mathf.Clamp(Mathf.FloorToInt(progressTime / actionTime * 100f), 0, 100) + " %";
    }

    private void Injection()
    {

       
        if (equipment&&equipment.IsActivate)
        {
            Debug.Log("ฉีดยาจ้าาาาาา");

          
            waterObject.gameObject.SetActive(true);

        }
        else
        {
             waterObject.gameObject.SetActive(false);
         
        }

    }
}
