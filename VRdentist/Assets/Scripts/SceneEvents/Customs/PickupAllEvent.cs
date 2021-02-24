using com.dgn.SceneEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "PickupAllEvent", menuName = "SceneEvent/Test02/PickupAllEvent")]
public class PickupAllEvent : SceneEvent
{
   

    private XRGrabInteractable grabInteractable;


    //อันนี้คือถาด
    public string collisionTriggerName;
    public string[] ToolsName;

    public string guidanceName = "PathGuidance";
    
    public SceneEvent nextScene;




    private CollisionTrigger trigger;
    private GrabbableEquipmentBehavior targetItem0;
    private GrabbableEquipmentBehavior targetItem1;
    private GrabbableEquipmentBehavior targetItem2;
    private GrabbableEquipmentBehavior targetItem3;
    private GrabbableEquipmentBehavior targetItem4;

    private GrabbableEquipmentBehavior Toolss;

    private PathGuidance guidance;
    private List<GameObject> tools;
    

    private bool isCollided;


    public override void InitEvent()
    {
        base.InitEvent();
       
        //ถาดโว้ย
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(collisionTriggerName, out trigger);

        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(ToolsName[0], out targetItem0);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(ToolsName[1], out targetItem1);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(ToolsName[2], out targetItem2);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(ToolsName[3], out targetItem3);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(ToolsName[4], out targetItem4);



        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);

     /*   tools = new List<GameObject>();
        foreach (string targetName in ToolsName)
        {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject toolsObject))
            {

               // toolsObject.SetActive(false); // hide at begin
                tools.Add(toolsObject);

            }
        }*/

        // ตอนแรกจะทำ foreach แต่มันไม่สามารถแปลง gameobject เป็น GrabbableEquipmentBehavior ได้






       

          Debug.Log("คู่กันมั้ย ? " + tools[0] + targetItem0);
          Debug.Log("คู่กันมั้ย ? " + tools[1] + targetItem1);
          Debug.Log("คู่กันมั้ย ? " + tools[2] + targetItem2);
          Debug.Log("คู่กันมั้ย ? " + tools[3] + targetItem3);
          Debug.Log("คู่กันมั้ย ? " + tools[4] + targetItem4);
          


    }

    public override void StartEvent()
    {
        isCollided = false;

     //   guidance?.SetParent(targetItem4.transform);
       // guidance?.SetTarget(trigger.transform);

        if (trigger)
        {
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }

       

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetItem0.gameObject)
        {
            Debug.Log("วาง0");
           
        }


        if (collision.gameObject == targetItem1.gameObject)
        {
            Debug.Log("วาง1");
            
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnGrabbed(XRBaseInteractor arg0)
    {
        
            Debug.Log("Grab");
        
      
    }



    private void OnReleased(XRBaseInteractor arg0)
    {
        Debug.Log("OnReleased");
        
    }


    public override SceneEvent NextEvent()
    {


        return nextScene;
    }

    

    public override void StopEvent()
    {
       // grabInteractable.onSelectEntered.RemoveListener(OnGrabbed);
        //grabInteractable.onSelectExited.RemoveListener(OnReleased);
    }

    public override void UpdateEvent()
    {
        if (targetItem0 && targetItem1 /*&& isCollided*/)
        {
            passEventCondition = true;
            Debug.Log("เล่นผ่านแล้วไอสัส");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

       

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
