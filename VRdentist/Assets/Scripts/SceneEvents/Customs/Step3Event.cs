using com.dgn.SceneEvent;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "Step3Event", menuName = "SceneEvent/TeethRemoval/Step3Event")]
public class Step3Event : SceneEvent
{

    public string collisionTriggerName;
    public string targetItemName;
    public string gumControllerName = "GumController";
    public string guidanceName = "PathGuidance";
    public SceneEvent nextScene;



    private CollisionTrigger trigger;
    private GrabbableEquipmentBehavior targetItem;
    private PathGuidance guidance;
    private UiController ui;
    private UiEquipmentController uiEquipment;
    private GumController gumCtrl;
    private bool isCollided;

    public override void InitEvent()
    {
        base.InitEvent();

        SceneAssetManager.GetAssetComponent("UIController", out ui);
        SceneAssetManager.GetAssetComponent("UIEquipment", out uiEquipment);

        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(targetItemName, out targetItem);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);
        SceneAssetManager.GetAssetComponent<GumController>(gumControllerName, out gumCtrl);

        if (nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        isCollided = false;
        guidance?.SetParent(targetItem.transform);


        Debug.Log(ui);
        ui.UpdateData(1);
        uiEquipment.UpdateData(1);
        if (trigger)
        {
            guidance?.SetTarget(trigger.transform);
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }

        Debug.Log("เริ่ม อีเว้น 3");
    }

    public override void UpdateEvent()
    {
        if (targetItem &&/* targetItem.IsActivate && */isCollided)
        {
            passEventCondition = true;
        }
    }

    public override void StopEvent()
    {
        if (trigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
            trigger.gameObject.SetActive(false);
        }
        guidance?.SetTarget(null);
        guidance?.SetParent(null);
        if (gumCtrl) gumCtrl.SettingGum(gumCtrl.targetGum_parent, gumCtrl.cutGum_parent);
        Debug.Log("Stop event: " + this.name);
    }

    public override SceneEvent NextEvent()
    {
        Debug.Log("Go next scene");
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
        if (collision.gameObject == targetItem.gameObject)
        {
            if (isCollided == false) Debug.Log(targetItem.name + "is Collided");
            isCollided = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetItem.gameObject)
        {
            isCollided = false;
        }
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
