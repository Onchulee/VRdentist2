using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public delegate void CollisionAction(Collision collision);
    public event CollisionAction OnCollisionEnterEvent;
    public event CollisionAction OnCollisionStayEvent;
    public event CollisionAction OnCollisionExitEvent;



    public delegate void TriggerAction(Collider collider);
    public event TriggerAction OnTriggerEnterEvent;
    public event TriggerAction OnTriggerStayEvent;
    public event TriggerAction OnTriggerExitEvent;



    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }




    private void OnTriggerEnter(Collider collider)
    {
        OnTriggerEnterEvent?.Invoke(collider);
    }

    private void OnTriggerStay(Collider collider)
    {
        OnTriggerStayEvent?.Invoke(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        OnTriggerExitEvent?.Invoke(collider);
    }
}
