using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;




[System.Serializable]
[CreateAssetMenu(fileName = "Step12Event", menuName = "SceneEvent/Step12/Step12Event")]
public class Step12Event : SceneEvent
{

   
    public SceneEvent nextScene;


    private UiResultController uiResult;

    public override void InitEvent()
    {
        base.InitEvent();
        
        SceneAssetManager.GetAssetComponent("UiResultController", out uiResult);

        uiResult.gameObject.SetActive(false);
    }



    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void StartEvent()
    {


        uiResult.UpdateData(2);
        Debug.Log("มาเริ่ม อีเว้นท์ Step12 กันเถอะ");
        
    }

    public override void StopEvent()
    {

        Debug.Log("จบ Step12 กันเถอะ");
        
    }

    public override void UpdateEvent()
    {
       
    }
}
