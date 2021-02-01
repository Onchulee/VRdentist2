using com.dgn.UnityAttributes;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabbableEquipmentBehavior : MonoBehaviour
{
    private static readonly string p_grabbed = "grabbed";
    private static readonly string p_activate= "activate";

    enum ActivateType { Switch, Hold }

    private XRGrabInteractable grabInteractable;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ActivateType activateType = ActivateType.Switch;

    [SerializeField]
    private bool alwaysActivateOnGrabbed = false;

    [SerializeField]
    private Collider[] physicsColliders;
    [SerializeField]
    private bool disabledPhysicsOnGrabbed = true;

    [SerializeField]
    private Collider[] triggerColliders;
    [SerializeField]
    private bool alwaysTriggerOnGrabbed = true;

    [SerializeField]
    [ReadOnly]
    [ModifiableProperty]
    private bool isGrabbed;
    [SerializeField]
    [ReadOnly]
    [ModifiableProperty]
    private bool isActivate;
    public bool IsActivate { get { return isActivate; } }

    private void Awake()
    {
        isGrabbed = false;
        isActivate = false;
    }

    void Start()
    {
        grabInteractable = this.GetComponent<XRGrabInteractable>();
        if (!animator) {
            animator = this.GetComponentInChildren<Animator>();
        }
        grabInteractable.onSelectEntered.AddListener(OnGrabbed);
        grabInteractable.onSelectExited.AddListener(OnReleased);
        grabInteractable.onActivate.AddListener(OnActivate);
        grabInteractable.onDeactivate.AddListener(OnDeactivate);
        SetAnimation();
        SetPhysicsColliders();
        SetActivateColliders();
    }

    void OnGrabbed(XRBaseInteractor rBaseInteractor)
    {
        isGrabbed = true;
        isActivate = alwaysActivateOnGrabbed;
        SetAnimation();
        SetPhysicsColliders();
        SetActivateColliders();
    }

    void OnReleased(XRBaseInteractor rBaseInteractor)
    {
        isGrabbed = false;
        isActivate = false;
        SetAnimation();
        SetPhysicsColliders();
        SetActivateColliders();
    }

    void OnActivate(XRBaseInteractor rBaseInteractor)
    {
        switch (activateType)
        {
            case ActivateType.Switch:
                isActivate = !isActivate;
                break;
            case ActivateType.Hold:
                isActivate = true;
                break;
        }
        isActivate = isActivate || alwaysActivateOnGrabbed;
        SetAnimation();
        SetActivateColliders();
    }

    void OnDeactivate(XRBaseInteractor rBaseInteractor)
    {
        switch (activateType)
        {
            case ActivateType.Switch:
                break;
            case ActivateType.Hold:
                isActivate = false;
                break;
        }
        isActivate = isActivate || alwaysActivateOnGrabbed;
        SetAnimation();
        SetActivateColliders();
    }

    private void SetAnimation() {
        if (animator) {
            animator.SetBool(p_grabbed, isGrabbed);
            animator.SetBool(p_activate, isGrabbed && isActivate);
        }
    }

    private void SetPhysicsColliders()
    {
        foreach (Collider col in physicsColliders) {
            if (col)
            {
                col.isTrigger = isGrabbed && disabledPhysicsOnGrabbed;
            }
        }
    }

    private void SetActivateColliders() {
        foreach (Collider col in triggerColliders)
        {
            if (col)
            {
                col.enabled = isGrabbed && (isActivate || alwaysTriggerOnGrabbed);
            }
        }
    }

    private void OnDestroy()
    {
        grabInteractable.onSelectEntered.RemoveListener(OnGrabbed);
        grabInteractable.onSelectExited.RemoveListener(OnReleased);
        grabInteractable.onActivate.RemoveListener(OnActivate);
        grabInteractable.onDeactivate.RemoveListener(OnDeactivate);
    }
}
