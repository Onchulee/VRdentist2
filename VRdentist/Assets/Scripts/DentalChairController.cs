using com.dgn.UnityAttributes;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class DentalChairController : MonoBehaviour
{
    private const string ANIM_BLEND = "Blend";
    private const string ANIM_LIFT = "Lift";
    private const string ANIM_MOUTH= "Mouth_Open";

    public Animator animator;

    [ReadOnly]
    public float lift_val = 0;
    public float lift_spd = 1f;
    
    [ReadOnly]
    public float blend_val = 0;
    public float blend_spd = 1f;

    [Header("Light Controller")]
    public XRGrabInteractable lightGrabbable;
    public Transform postTransform;
    private Vector3 oldPostPos;


    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetFloat(ANIM_LIFT, lift_val);
        animator.SetFloat(ANIM_BLEND, blend_val);
        oldPostPos = postTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void LiftUp() {
        lift_val = Mathf.Clamp01(lift_val + lift_spd * Time.deltaTime);
        UpdateLift(lift_val);
    }

    public void LiftDown()
    {
        lift_val = Mathf.Clamp01(lift_val - lift_spd * Time.deltaTime);
        UpdateLift(lift_val);
    }

    private void UpdateLift(float val) {
        animator.SetFloat(ANIM_LIFT, val);
        if (lightGrabbable && postTransform)
        {
            lightGrabbable.transform.position = lightGrabbable.transform.position + (postTransform.position - oldPostPos);
            oldPostPos = postTransform.position;
        }
    }
    
    public void BlendUp()
    {
        blend_val = Mathf.Clamp01(blend_val + blend_spd * Time.deltaTime);
        animator.SetFloat(ANIM_BLEND, blend_val);
    }

    public void BlendDown()
    {
        blend_val = Mathf.Clamp01(blend_val - blend_spd * Time.deltaTime);
        animator.SetFloat(ANIM_BLEND, blend_val);
    }
}
