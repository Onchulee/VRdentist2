using com.dgn.SceneEvent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "Step6_Event", menuName = "SceneEvent/TeethRemoval/Step6_Event")]
public class Step6Event : SceneEvent
{
  
    public string wisdomTeethName;
    public string wisdomTeethTriggerName;
    public string[] fragmentTeethNames;
    public string toolName;
    public string progressTextName;
    public float actionTime;
    public string guidanceName = "PathGuidance";
    public SceneEvent nextScene;

    private GameObject wisdomTeeth;
    private CollisionTrigger wisdomTeethTrigger;
    private List<GameObject> fragmentTooth;


    private GrabbableEquipmentBehavior tool;
    private Text progressText;
    private PathGuidance guidance;

    private bool isCollided;
    private float progressTime;
    private float delayEndProgress;
    private UiController ui;


    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponent("UIController", out ui);
        bool foundTeeth = SceneAssetManager.GetGameObjectAsset(wisdomTeethName, out wisdomTeeth);
        bool foundTrigger = SceneAssetManager.GetAssetComponent<CollisionTrigger>(wisdomTeethTriggerName, out wisdomTeethTrigger);
        bool foundItem = SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(toolName, out tool);
        bool foundText = SceneAssetManager.GetAssetComponent<Text>(progressTextName, out progressText);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);

        fragmentTooth = new List<GameObject>();
        foreach (string targetName in fragmentTeethNames) {
            if (SceneAssetManager.GetGameObjectAsset(targetName, out GameObject targetObject)) {
                targetObject.SetActive(false); // hide at begin
                fragmentTooth.Add(targetObject);
            }
        }



        Debug.Log("Found Asset[" + wisdomTeethName + "]: " + foundTeeth);
        Debug.Log("Found Asset[" + wisdomTeethTriggerName + "]: " + foundTrigger);
        Debug.Log("Found Asset[fragment Teeth]: " + (fragmentTooth.Count>0));
        Debug.Log("Found Asset[" + toolName + "]: " + foundItem);
        Debug.Log("Found Asset[" + progressTextName + "]: " + foundText);

        if (wisdomTeeth) wisdomTeeth.SetActive(true);

        if (nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        isCollided = false;
        progressTime = 0;
        delayEndProgress = 1f;
        ui.UpdateData(4);

        if (wisdomTeeth) {
            wisdomTeeth.SetActive(true);
        }
        foreach (GameObject fragment in fragmentTooth)
        {
            fragment.SetActive(false);
        }
        if (wisdomTeethTrigger)
        {
            wisdomTeethTrigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            wisdomTeethTrigger.OnCollisionEnterEvent += OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent += OnCollisionExit;
        }
        if (progressText) {
            progressText.text = GetProgressString();
            progressText.gameObject.SetActive(true);
        }
        guidance?.SetParent(tool.transform);
        guidance?.SetTarget(wisdomTeeth.transform);
    }

    public override void UpdateEvent()
    {
        if (tool && tool.IsActivate && isCollided)
        {
            progressTime += Time.deltaTime;
            progressText.text = GetProgressString();
        }
        if (actionTime < progressTime) {
            if (wisdomTeeth && wisdomTeeth.activeInHierarchy) {
                BreakTeeth();
            }
            delayEndProgress -= Time.deltaTime;
        }
        passEventCondition = (actionTime < progressTime && delayEndProgress < 0);
    }

    public override void StopEvent()
    {
        

        guidance?.SetTarget(null);
        guidance?.SetParent(null);
        if (wisdomTeethTrigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            wisdomTeethTrigger.OnCollisionEnterEvent -= OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent -= OnCollisionExit;
            wisdomTeethTrigger.gameObject.SetActive(false);
        }
        if (progressText)
        {
            progressText.gameObject.SetActive(false);
        }
        BreakTeeth();
        Debug.Log("Stop event: " + this.name);
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
        if (collision.gameObject == tool.gameObject)
        {
            if (isCollided == false) Debug.Log(tool.name + "is Collided");
            isCollided = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == tool.gameObject)
        {
            isCollided = false;
        }
    }

    private string GetProgressString() {
        return Mathf.Clamp(Mathf.FloorToInt(progressTime/actionTime*100f), 0, 100) +" %";
    }

    private void BreakTeeth()
    {
        wisdomTeeth?.SetActive(false);
        foreach (GameObject fragment in fragmentTooth)
        {
            fragment.SetActive(true);
        }
    }

    public override void OnDestroy()
    {
        if (wisdomTeethTrigger)
        {
            wisdomTeethTrigger.OnCollisionEnterEvent -= OnCollisionEnter;
            wisdomTeethTrigger.OnCollisionExitEvent -= OnCollisionExit;
        }
    }
}
