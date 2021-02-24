using com.dgn.SceneEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
[CreateAssetMenu(fileName = "PickAllEvent", menuName = "SceneEvent/Test02/PickAllEvent")]
[RequireComponent(typeof(XRGrabInteractable))]
public class PickAllEvent : SceneEvent
{
    public string collisionTriggerName;
    public string[] ToolsNames;
    public string[] ToolsDetailText;
    public string guidanceName = "PathGuidance";
    public SceneEvent nextScene;



   // private List<GameObject> toolsName;
    private List<GrabbableEquipmentBehavior> tools;
    private List<GameObject> texts;
    private CollisionTrigger trigger;
    private PathGuidance guidance;


    private List<XRGrabInteractable> grabInteractable;

    private XRGrabInteractable grabInteractable0;
    private XRGrabInteractable grabInteractable1;


    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);


        /* toolsName = new List<GameObject>();
          foreach (string targetName in ToolsNames)
          {
              if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject))
              {
                  targetObject.SetActive(false); // hide at begin
                  toolsName.Add(targetObject);
              }
          }*/

        tools = new List<GrabbableEquipmentBehavior>();
        foreach (string targetName in ToolsNames)
        {
            if (SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(targetName, out GrabbableEquipmentBehavior targetObject))
            {
                //targetObject.SetActive(false); // hide at begin
                tools.Add(targetObject);
            }
        }

        grabInteractable = new List<XRGrabInteractable>();
        foreach (string targetName in ToolsNames)
        {
            if (SceneAssetManager.GetAssetComponent<XRGrabInteractable>(targetName, out XRGrabInteractable targetObject))
            {

                grabInteractable.Add(targetObject);

            }
        }

        grabInteractable[0] = tools[0].GetComponent<XRGrabInteractable>();
        grabInteractable[0].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[0].onSelectExited.AddListener(OnReleased);

        grabInteractable[1] = tools[1].GetComponent<XRGrabInteractable>();
        grabInteractable[1].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[1].onSelectExited.AddListener(OnReleased);

        grabInteractable[2] = tools[2].GetComponent<XRGrabInteractable>();
        grabInteractable[2].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[2].onSelectExited.AddListener(OnReleased);

        grabInteractable[2] = tools[1].GetComponent<XRGrabInteractable>();
        grabInteractable[2].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[2].onSelectExited.AddListener(OnReleased);

        grabInteractable[3] = tools[3].GetComponent<XRGrabInteractable>();
        grabInteractable[3].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[3].onSelectExited.AddListener(OnReleased);

        grabInteractable[4] = tools[4].GetComponent<XRGrabInteractable>();
        grabInteractable[4].onSelectEntered.AddListener(OnGrabbed);
        grabInteractable[4].onSelectExited.AddListener(OnReleased);

        texts = new List<GameObject>();
        foreach (string targetName in ToolsDetailText)
        {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject))
            {
                targetObject.SetActive(false); // hide at begin
                texts.Add(targetObject);
            }
        }

    

       

      /*  Debug.Log("Found Asset[Tools]: " + (tools.Count > 0));

        Debug.Log("อุปกรณ์ที่ถือ" + tools[0]);
        Debug.Log("อุปกรณ์ที่ถือ" + tools[1]);
        Debug.Log("อุปกรณ์ที่ถือ" + tools[2]);
        Debug.Log("อุปกรณ์ที่ถือ" + tools[3]);
        Debug.Log("อุปกรณ์ที่ถือ" + tools[4]);



        Debug.Log("Found Asset[Text]: " + (texts.Count > 0));
        Debug.Log("Found Asset[Text]: " + texts.Count);
        Debug.Log("Textที่เจอ" + texts[0]);
        Debug.Log("Textที่เจอ" + texts[1]);
        Debug.Log("Textที่เจอ" + texts[2]);
        Debug.Log("Textที่เจอ" + texts[3]);
        Debug.Log("Textที่เจอ" + texts[4]);

        Debug.Log("Found Asset[GrabInteractable]: " + (grabInteractable.Count > 0));
        Debug.Log("Found Asset[GrabInteractable]: " + grabInteractable.Count);
        Debug.Log("GrabInteractableที่เจอ" + grabInteractable[0]);
        Debug.Log("GrabInteractableเจอ" + grabInteractable[1]);
        Debug.Log("GrabInteractable" + grabInteractable[2]);
        Debug.Log("GrabInteractableที่เจอ" + grabInteractable[3]);
        Debug.Log("GrabInteractableที่เจอ" + grabInteractable[4]);*/
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

        


        if (interactable.gameObject == tools[0].gameObject)
        {
           
            Debug.Log("จับ0");
            
            texts[0].gameObject.SetActive(true);
            guidance?.SetParent(tools[0].transform);
        }


        if (interactable.gameObject == tools[1].gameObject)
        {

            Debug.Log("จับ1");
           
            texts[1].gameObject.SetActive(true);
            guidance?.SetParent(tools[1].transform);
        }

        if (interactable.gameObject == tools[2].gameObject)
        {

            Debug.Log("จับ2");
            texts[2].gameObject.SetActive(true);
            guidance?.SetParent(tools[2].transform);
        }

        if (interactable.gameObject == tools[3].gameObject)
        {

            Debug.Log("จับ3");
            texts[3].gameObject.SetActive(true);
            guidance?.SetParent(tools[3].transform);
        }

        if (interactable.gameObject == tools[4].gameObject)
        {

            Debug.Log("จับ4");
            texts[4].gameObject.SetActive(true);
            guidance?.SetParent(tools[4].transform);
        }


        
        
    }



    private void OnReleased(XRBaseInteractor interactor)
    {
          XRBaseInteractable interactable = interactor.selectTarget;
        //  if (interactable == null) return;

        if (interactor == null) return;



        texts[0].gameObject.SetActive(false);
        texts[1].gameObject.SetActive(false);
        texts[2].gameObject.SetActive(false);
        texts[3].gameObject.SetActive(false);
        texts[4].gameObject.SetActive(false);

        guidance?.SetParent(null);

        /*  if (interactor.selectTarget == tools[0].gameObject  ) return;
          {

              Debug.Log(")ปล่อย0");

              texts[0].gameObject.SetActive(false);

          }



          if (interactor == tools[1].gameObject) return;
          {

              Debug.Log(")ปล่อย1");

              texts[1].gameObject.SetActive(false);

          }
          if (interactor.selectTarget == tools[2].gameObject) return;
          {

              Debug.Log(")ปล่อย2");

              texts[2].gameObject.SetActive(false);

          }

          */
    }



    public override void UpdateEvent()
    {
        

    }

    public override void StopEvent()
    {


        grabInteractable[0].onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable[0].onSelectExited.RemoveListener(OnReleased);

        grabInteractable[1].onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable[1].onSelectExited.RemoveListener(OnReleased);

        grabInteractable[2].onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable[2].onSelectExited.RemoveListener(OnReleased);

        grabInteractable[3].onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable[3].onSelectExited.RemoveListener(OnReleased);

        grabInteractable[3].onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable[3].onSelectExited.RemoveListener(OnReleased);


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
       /* if ()
        {
            passEventCondition = true;
        }*/

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


        guidance?.SetParent(null);
    }

    private void OnCollisionExit(Collision collision)
    {



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
