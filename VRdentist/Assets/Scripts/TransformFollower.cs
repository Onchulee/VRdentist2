using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public Transform target;
    bool hasTarget;
    public Renderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        hasTarget = target != null;
        SetActiveRenderers(hasTarget);
    }

    public void SetTarget(Transform setTarget) {
        target = setTarget;
        FollowTarget();
        UpdateRenderers();
    }

    public void RemoveTarget()
    {
        target = null;
        UpdateRenderers();
    }

    public void SetTransformTo(Transform setTarget) {
        if (setTarget)
        {
            this.transform.position = setTarget.position;
            this.transform.rotation = setTarget.rotation;
        }
    }

    private void SetActiveRenderers(bool isActive) {
        foreach (Renderer rend in renderers) {
            rend.enabled = isActive;
        }
    }

    private void FollowTarget() {
        if (target)
        {
            this.transform.position = target.position;
            this.transform.rotation = target.rotation;
        }
    }

    private void UpdateRenderers() {
        if (hasTarget == true && target == null)
        {
            hasTarget = false;
            SetActiveRenderers(hasTarget);
        }
        else if (hasTarget == false && target != null)
        {
            hasTarget = true;
            SetActiveRenderers(hasTarget);
        }
    }

    void Update()
    {
        FollowTarget();
        UpdateRenderers();
    }
}
