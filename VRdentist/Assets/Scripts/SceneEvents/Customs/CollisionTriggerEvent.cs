using com.dgn.SceneEvent;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "CollisionTriggerEvent", menuName = "SceneEvent/TeethRemoval/CollisionTriggerEvent")]
public class CollisionTriggerEvent : SceneEvent
{
    public string uiBoardName;
    public string collisionTriggerName;
    public string targetItemName;
    public string guidanceName= "PathGuidance";
    public SceneEvent nextScene;


    private Text uiBoardText;
    private CollisionTrigger trigger;
    private GrabbableEquipmentBehavior targetItem;
    private PathGuidance guidance;

    private bool isCollided;

    public override void InitEvent()
    {
        base.InitEvent();
        SceneAssetManager.GetAssetComponent(uiBoardName, out uiBoardText);
        SceneAssetManager.GetAssetComponentInChildren<CollisionTrigger>(collisionTriggerName, out trigger);
        SceneAssetManager.GetAssetComponent<GrabbableEquipmentBehavior>(targetItemName, out targetItem);
        SceneAssetManager.GetAssetComponent<PathGuidance>(guidanceName, out guidance);

        if (nextScene) nextScene.InitEvent();
    }

    public override void StartEvent()
    {
        isCollided = false;
        guidance?.SetParent(targetItem.transform);
        uiBoardText.gameObject.SetActive(true);
        if (trigger)
        {
            guidance?.SetTarget(trigger.transform);
            trigger.gameObject.SetActive(true);
            Debug.Log("CollisionTriggerEvent assign events");
            trigger.OnCollisionEnterEvent += OnCollisionEnter;
            trigger.OnCollisionExitEvent += OnCollisionExit;
        }

        Debug.Log("เริ่ม อีเว้น 2");
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
        uiBoardText.gameObject.SetActive(false);

        if (trigger)
        {
            Debug.Log("CollisionTriggerEvent remove events");
            trigger.OnCollisionEnterEvent -= OnCollisionEnter;
            trigger.OnCollisionExitEvent -= OnCollisionExit;
            trigger.gameObject.SetActive(false);
        }
        guidance?.SetTarget(null);
        guidance?.SetParent(null);
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
        if (collision.gameObject == targetItem.gameObject) {
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
