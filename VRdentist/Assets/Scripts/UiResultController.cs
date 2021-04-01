using com.dgn.SceneEvent;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class UiResultController : MonoBehaviour
{
    public Field trackedUi;
    public UIData[] uiResultSetup;


    [System.Serializable]
    public struct UIData
    {
        public GameObject bgObject;
        public Sprite emojiImage;

        public string timeint;
        public string satisfactionText;
    }

    [System.Serializable]
    public struct Field
    {
        public GameObject bg;
        public Image emoji;
        public Text time;
        public Text satisfaction;
    }

    public void UpdateData(int step)
    {
        trackedUi.bg = uiResultSetup[step].bgObject;
        trackedUi.emoji.sprite = uiResultSetup[step].emojiImage;
        trackedUi.time.text = uiResultSetup[step].timeint;
        trackedUi.satisfaction.text = uiResultSetup[step].satisfactionText;


    }

    void Start()
    {
        //UpdateData(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
