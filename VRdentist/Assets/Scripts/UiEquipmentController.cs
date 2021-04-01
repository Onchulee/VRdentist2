﻿using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UiEquipmentController : MonoBehaviour
{
    public Field trackedUi;
    public UIData[] uiBoardSetup;


    [System.Serializable]
    public struct UIData
    {
        public GameObject bgObject;
        public string step;
        public Sprite excempleImage;
      //  public Sprite excempleImage2;
        [TextArea]
        public string textDetail;
    }

    [System.Serializable]
    public struct Field
    {
        public GameObject bg;
        public Text uiStep;
        public Image pic;
      //  public Image pic2;
        public Text detail;
    }

    public void UpdateData(int step)
    {
        trackedUi.bg = uiBoardSetup[step].bgObject;
        trackedUi.uiStep.text = uiBoardSetup[step].step;
        trackedUi.pic.sprite = uiBoardSetup[step].excempleImage;
       // trackedUi.pic2.sprite = uiBoardSetup[step].excempleImage2;
        trackedUi.detail.text = uiBoardSetup[step].textDetail;


    }



    // Start is called before the first frame update
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        //  Debug.Log(trackedUi[0].uiStep.gameObject);
    }
}