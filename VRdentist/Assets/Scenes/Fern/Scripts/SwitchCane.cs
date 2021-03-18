using UnityEngine;

public class SwitchCane : SwitchController
{
    public Axis activationAxis;
    public MinMax turnSwitchAVal;
    public MinMax turnSwitchBVal;

    [Header("Switch Control")]
    public Transform lockTransform;
    public Vector3 lockPosition;
    public MinMaxVector3 limitAngles;
    
    protected override void Start()
    {
        base.Start();
    }
    
    void LateUpdate()
    {
        if (lockTransform) {
            LockTransform();
            CheckActivation(lockTransform.localEulerAngles);
        }
    }

    private void LockTransform() {
        lockTransform.localPosition = lockPosition;
        lockTransform.localRotation = Vector3Extensions.RotationClamp(
                                                lockTransform.localEulerAngles, 
                                                limitAngles.min, limitAngles.max);
    }

    private void CheckActivation(Vector3 eulerAngles) {
        switch (activationAxis) {
            case Axis.X:
                CallEvent(eulerAngles, Vector3.right);
                break;
            case Axis.Y:
                CallEvent(eulerAngles, Vector3.up);
                break;
            case Axis.Z:
                CallEvent(eulerAngles, Vector3.forward);
                break;
        }
    }

    private void CallEvent(Vector3 val, Vector3 targetDir) {
        if (val == Vector3Extensions.RotationClamp(val, targetDir * turnSwitchAVal.min, targetDir * turnSwitchAVal.max).eulerAngles)
        {
            ActivateSwitch(Activation.SwitchA);
        }
        else if (val == Vector3Extensions.RotationClamp(val, targetDir * turnSwitchBVal.min, targetDir * turnSwitchBVal.max).eulerAngles)
        {
            ActivateSwitch(Activation.SwitchB);
        }
        else {
            ActivateSwitch(Activation.TurnOff);
        }
    }
}
