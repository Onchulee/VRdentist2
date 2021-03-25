using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public Transform target;
    bool hasTarget;

    // Start is called before the first frame update
    void Start()
    {
        hasTarget = target != null;
    }

    public void SetTarget(Transform setTarget) {
        target = setTarget;
        FollowTarget();
    }

    public void RemoveTarget()
    {
        target = null;
    }

    public void SetTransformTo(Transform setTarget) {
        if (setTarget)
        {
            this.transform.position = setTarget.position;
            this.transform.rotation = setTarget.rotation;
        }
    }
    

    private void FollowTarget() {
        if (target)
        {
            this.transform.position = target.position;
            this.transform.rotation = target.rotation;
        }
    }
    

    void Update()
    {
        FollowTarget();
    }
}
