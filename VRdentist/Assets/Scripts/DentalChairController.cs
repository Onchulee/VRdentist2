using com.dgn.UnityAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DentalChairController : MonoBehaviour
{
    private const string ANIM_BLEND = "Blend";
    private const string ANIM_LIFT = "Lift";
    private const string ANIM_MOUTH= "Mouth_Open";

    public Animator animator;


    SwitchController.Activation lift_mode = SwitchController.Activation.TurnOff;
    [ReadOnly]
    public float lift_val = 0;
    public float lift_spd = 1f;

    SwitchController.Activation blend_mode = SwitchController.Activation.TurnOff;
    [ReadOnly]
    public float blend_val = 0;
    public float blend_spd = 1f;


    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetFloat(ANIM_LIFT, lift_val);
        animator.SetFloat(ANIM_BLEND, blend_val);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLift();
        UpdateBlend();
    }

    private void UpdateLift() {
        if (lift_mode == SwitchController.Activation.SwitchA)
        {
            lift_val = Mathf.Clamp01(lift_val + lift_spd * Time.deltaTime);
        }
        else if (lift_mode == SwitchController.Activation.SwitchB)
        {
            lift_val = Mathf.Clamp01(lift_val - lift_spd * Time.deltaTime);
        }
        if (lift_mode != SwitchController.Activation.TurnOff) {
            animator.SetFloat(ANIM_LIFT, lift_val);
        }
    }

    public void LiftUp() {
        lift_mode = SwitchController.Activation.SwitchA;
    }

    public void LiftDown()
    {
        lift_mode = SwitchController.Activation.SwitchB;
    }

    public void TurnLiftOff()
    {
        lift_mode = SwitchController.Activation.TurnOff;
    }

    private void UpdateBlend()
    {
        if (blend_mode == SwitchController.Activation.SwitchA)
        {
            blend_val = Mathf.Clamp01(blend_val + blend_spd * Time.deltaTime);
        }
        else if (blend_mode == SwitchController.Activation.SwitchB)
        {
            blend_val = Mathf.Clamp01(blend_val - blend_spd * Time.deltaTime);
        }
        if (blend_mode != SwitchController.Activation.TurnOff)
        {
            animator.SetFloat(ANIM_BLEND, blend_val);
        }
    }

    public void BlendUp()
    {
        blend_mode = SwitchController.Activation.SwitchA;
    }

    public void BlendDown()
    {
        blend_mode = SwitchController.Activation.SwitchB;
    }

    public void TurnBlendOff()
    {
        blend_mode = SwitchController.Activation.TurnOff;
    }
}
