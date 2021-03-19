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

    public string uiBoardName;

    private Text uiBoardText;

    public SceneEvent nextScene;




    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponent(uiBoardName, out uiBoardText);
    }



    public override SceneEvent NextEvent()
    {
        return nextScene;
    }

    public override void StartEvent()
    {
        Debug.Log("มาเริ่ม อีเว้นท์ Step12 กันเถอะ");
        uiBoardText.gameObject.SetActive(true);
    }

    public override void StopEvent()
    {

        Debug.Log("จบ Step12 กันเถอะ");
        uiBoardText.gameObject.SetActive(false);
    }

    public override void UpdateEvent()
    {
       
    }
}
