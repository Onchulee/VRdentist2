using com.dgn.SceneEvent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrayCheck : MonoBehaviour
{
    public string collisionTriggerName;
    public Text MissionClearText;
    public Text numOfTools;

    // public string targetItemName;
    //  public string ScissorsTextName;

    //ลองทำเองเพิ่ม
    public string[] ToolsNames;
    public string[] TextCheckNames;

   

    private List<GameObject> Tools;
    private List<GameObject> Texts;


    private int score;


  //  public string[] ToolsNames02;
   // private bool[] check;

   // List<GameObject> list;





        [System.Serializable]
        public struct Tracking 
        {
        public GameObject tools;
        public bool check;
        }

        private List<Tracking> list02;

  
    void Start()
    {

      /*  list = new List<GameObject>();
        foreach (string targetName in ToolsNames02)
        {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject toolsObject))
            {

                list.Add(toolsObject);
            }
        }


        Debug.Log(list[0]);
        Debug.Log(list[1]);
        Debug.Log(list[2]);*/


       
       


        Debug.Log("ddd");

       /* Debug.Log(ToolsNames02[0]);


        list = new List<GameObject>();
        for (int i = 0; i < ToolsNames02.Length; i++)
                  {

                     SceneAssetManager.GetGameObjectAsset(ToolsNames02[i], out GameObject toolsObject);

                      list.Add(toolsObject);
                


        }

       // ToolsNames02 = list.ToArray();

          check = new bool[ToolsNames02.Length];


        Debug.Log(list[0]);
        Debug.Log(list[1]);
        Debug.Log(list[2]);/*




        //ล้มเหลว
        
        for (int i = 0; i < 5; i++)
        {
            Tracking newTrack = new Tracking
            {
        //    tools = GetGameObjectAsset()
            check = false
            }  ;
            list.Add(newTrack);

        }
        


        //ลองทำเพิ่มเอง
/*
        Tools = new List<GameObject>();
        foreach (string targetName in ToolsNames)
        {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject toolsObject ))
            {
               
                Tools.Add(toolsObject);
            }
        }

        Texts = new List<GameObject>();
        foreach (string targetName in TextCheckNames)
        {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject))
            {
                targetObject.SetActive(false); // hide at begin
                Texts.Add(targetObject);
            }
        }

      


        /*
                Debug.Log("จำนวนเครื่องมือที่เจอ: " + Tools.Count);
                Debug.Log(Tools[0]);
                Debug.Log(Tools[1]);
                Debug.Log(Tools[2]);
                Debug.Log(Tools[3]);
                Debug.Log(Tools[4]);
                Debug.Log("____________________________");


                Debug.Log("จำนวนข้อความที่เจอ: " + Texts.Count);
                Debug.Log(Texts[0]);
                Debug.Log(Texts[1]);
                Debug.Log(Texts[2]);
                Debug.Log(Texts[3]);
                Debug.Log(Texts[4]);
                */

        score = 0;

        
        MissionClearText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

     //   progressText.text = GetProgressString();

        numOfTools.text = score.ToString();


        if(score>= 8)
        {
            Debug.Log("___ภารกิจผ่าน___");

            MissionClearText.gameObject.SetActive(true);

        }

       




    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;

        for (int i =0; i<Tools.Count ; i++)
        {
            if (collision.rigidbody.gameObject == Tools[i].gameObject)
            {
                Texts[i].gameObject.SetActive(true);
                score++;
                break;
            }


        }
    
      
    }

    

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject == Tools[0].gameObject)
        {
            Texts[0].gameObject.SetActive(false);
            score--;
        }
        if (collision.gameObject == Tools[1].gameObject)
        {
            Texts[1].gameObject.SetActive(false);
            score--;
        }
        if (collision.gameObject == Tools[2].gameObject)
        {
            Texts[2].gameObject.SetActive(false);
            score--;
        }
        if (collision.gameObject == Tools[3].gameObject)
        {
            Texts[3].gameObject.SetActive(false);
            score--;
        }
        if (collision.gameObject == Tools[4].gameObject)
        {
            Texts[4].gameObject.SetActive(false);
            score--;
        }

    }



}
