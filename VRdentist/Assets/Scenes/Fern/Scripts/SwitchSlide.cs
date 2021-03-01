using com.dgn.UnityAttributes;
using UnityEngine;

public class SwitchSlide : SwitchController
{
    [ReadOnly]
    [SerializeField]
    private float velocity;
    [ReadOnly]
    [SerializeField]
    private float direction;
    
    public Axis activationAxis;
    public MinMax turnSwitchAVal;
    public MinMax turnSwitchBVal;

    [Header("Switch Control")]
    public Rigidbody lockBody;
    public Vector3 defaultPosition;
    public MinMaxVector3 limitPositions;

    protected override void Start()
    {
        base.Start();
    }

    void LateUpdate()
    {
        if (lockBody)
        {
            LockTransform();
            CheckActivation(lockBody.transform.localPosition);
        }
    }

    private void CheckActivation(Vector3 position)
    {
        switch (activationAxis)
        {
            case Axis.X:
                CallEvent(position.x);
                break;
            case Axis.Y:
                CallEvent(position.y);
                break;
            case Axis.Z:
                CallEvent(position.z);
                break;
        }
    }

    private void CallEvent(float val)
    {
        if (val == Mathf.Clamp(val, turnSwitchAVal.min, turnSwitchAVal.max))
        {
            ActivateSwitch(Activation.SwitchA);
        }
        else if (val == Mathf.Clamp(val, turnSwitchBVal.min, turnSwitchBVal.max))
        {
            ActivateSwitch(Activation.SwitchB);
        }
        else
        {
            ActivateSwitch(Activation.TurnOff);
        }
    }

    private void LockTransform()
    {
        lockBody.transform.localPosition = Vector3Extensions.PositionClamp(lockBody.transform.localPosition, limitPositions.min, limitPositions.max);
        // limit velocity
        float limitV = Mathf.Clamp01(lockBody.velocity.magnitude);
        lockBody.velocity = lockBody.velocity.normalized * limitV;
        // no spring bounce
        velocity = GetVelocity();
        direction = GetDirection();
        if (velocity > 0 && direction > 0)
        {
            lockBody.velocity = Vector3.zero;
            lockBody.transform.localPosition = defaultPosition;
        }
        if (velocity < 0 && direction < 0)
        {
            lockBody.velocity = Vector3.zero;
            lockBody.transform.localPosition = defaultPosition;
        }
    }

    private float GetVelocity() {
        if (lockBody) {
            switch (activationAxis)
            {
                case Axis.X:
                    return lockBody.velocity.x;
                case Axis.Y:
                    return lockBody.velocity.y;
                case Axis.Z:
                    return lockBody.velocity.z;
            }
        }
        return 0f;
    }

    private float GetDirection()
    {
        if (lockBody)
        {
            switch (activationAxis)
            {
                case Axis.X:
                    return lockBody.transform.localPosition.x - defaultPosition.x;
                case Axis.Y:
                    return lockBody.transform.localPosition.y - defaultPosition.y;
                case Axis.Z:
                    return lockBody.transform.localPosition.z - defaultPosition.z;
            }
        }
        return 0f;
    }

}
