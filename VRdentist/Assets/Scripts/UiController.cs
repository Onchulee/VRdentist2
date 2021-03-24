using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UiController : MonoBehaviour
{
    public ToolSetup[] uiBoardSetup;
    private Tracking[] trackedUi;

    [System.Serializable]
    public struct ToolSetup
    {
        public string step;
        public string picName;
        public string detailName;
       


    }

    [System.Serializable]
    public struct Tracking
    {
        public GameObject uiStep;
        public GameObject pic;
        public GameObject detail;
      
        public bool hold;
        public bool check;

    }

    



    // Start is called before the first frame update
    void Start()
    {
      //  SceneAssetManager.GetAssetComponent(uiBoardName, out uiBoardText);

        List<Tracking> trackedList = new List<Tracking>();
        foreach (ToolSetup config in uiBoardSetup)
        {
            if (SceneAssetManager.GetAssetComponent(config.step,
                out GameObject targetObject))
            {
                Tracking newTrack = new Tracking
                {
                    uiStep = targetObject,
                    check = false,
                    hold = false
                };
                if (SceneAssetManager.GetAssetComponentInChildren(config.picName, out GameObject i_pic))
                {
                    newTrack.pic = i_pic;
                }

                if (SceneAssetManager.GetAssetComponent(config.detailName, out GameObject i_detail))
                {
                    newTrack.detail = i_detail;
                }
                
                trackedList.Add(newTrack);

              


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(trackedUi[0].uiStep.gameObject);
    }
}
