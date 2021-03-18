using com.dgn.UnityAttributes;
using UnityEngine;

public class SwitchClick : SwitchController
{
    public Axis activationAxis;
    public MinMax turnSwitchAVal;
    public MinMax turnSwitchBVal;

    [Header("Switch Control")]
    public Transform lockTransform;
    public Vector3 lockPosition;
    public MinMaxVector3 limitAngles;


    public CollisionTrigger collision;
    public Rigidbody rigid;
    RigidbodyConstraints defaultConstraints;
    RigidbodyConstraints fixedConstraints;

    public Collider colA;
    public Transform baseA;
    [ReadOnly]
    [SerializeField]
    private bool isContactA;
    [ReadOnly]
    [SerializeField]
    private bool isContactSurfaceA;
    [ReadOnly]
    [SerializeField]
    private Vector3 contactPointA;

    public Collider colB;
    public Transform baseB;
    [ReadOnly]
    [SerializeField]
    private bool isContactB;
    [ReadOnly]
    [SerializeField]
    private bool isContactSurfaceB;
    [ReadOnly]
    [SerializeField]
    private Vector3 contactPointB;

    protected override void Start()
    {
        base.Start();
        collision.OnCollisionStayEvent += OnCollisionStayEvent;
        collision.OnCollisionExitEvent += OnCollisionExitEvent;
        defaultConstraints = rigid.constraints;
        fixedConstraints = RigidbodyConstraints.FreezeAll;
    }
    
    void LateUpdate()
    {
        if (rigid)
        {
            rigid.constraints = defaultConstraints;
            if (isContactA == true && isContactSurfaceA == false)
            {
                lockTransform.localRotation = Quaternion.Euler(limitAngles.max);
                rigid.constraints = fixedConstraints;
            }
            if (isContactB == true && isContactSurfaceB == false)
            {
                lockTransform.localRotation = Quaternion.Euler(limitAngles.min);
                rigid.constraints = fixedConstraints;
            }
            if (isContactA && isContactB)
            {
                lockTransform.localRotation = Quaternion.Euler((limitAngles.min + limitAngles.max) * 0.5f);
                rigid.constraints = fixedConstraints;
            }
        }

        if (lockTransform)
        {
            LockTransform();
            CheckActivation(lockTransform.localEulerAngles);
        }
    }

    private void LockTransform()
    {
        lockTransform.localPosition = lockPosition;
        lockTransform.localRotation = Vector3Extensions.RotationClamp(
                                                lockTransform.localEulerAngles,
                                                limitAngles.min, limitAngles.max);
    }

    private void CheckActivation(Vector3 eulerAngles)
    {
        switch (activationAxis)
        {
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

    private void CallEvent(Vector3 val, Vector3 targetDir)
    {
        if (val == Vector3Extensions.RotationClamp(val, targetDir * turnSwitchAVal.min, targetDir * turnSwitchAVal.max).eulerAngles)
        {
            ActivateSwitch(Activation.SwitchA);
        }
        else if (val == Vector3Extensions.RotationClamp(val, targetDir * turnSwitchBVal.min, targetDir * turnSwitchBVal.max).eulerAngles)
        {
            ActivateSwitch(Activation.SwitchB);
        }
        else
        {
            ActivateSwitch(Activation.TurnOff);
        }
    }

    private bool GetContact(ContactPoint contactPoint) {
        bool isIntersect = contactPoint.thisCollider.bounds.Intersects(contactPoint.otherCollider.bounds);
        return isIntersect;
    }

    private void OnCollisionStayEvent(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.thisCollider == colA)
            {
                contactPointA = contactPoint.point;
                isContactA = GetContact(contactPoint);
                isContactSurfaceA = CheckContactSurface(contactPoint.point, colA.transform);
            }
            if (contactPoint.thisCollider == colB)
            {
                contactPointB = contactPoint.point;
                isContactB = GetContact(contactPoint);
                isContactSurfaceB = CheckContactSurface(contactPoint.point, colB.transform);
            }
        }
    }

    private bool CheckContactSurface(Vector3 contactPos, Transform baseTransform) {
        Vector3 heading = contactPos - baseTransform.position;
        float dot = Vector3.Dot(heading, baseTransform.up);
        return dot > 0;
    }

    private void OnCollisionExitEvent(Collision collision)
    {
        if (collision.contacts.Length <= 0) {
            isContactA = false;
            isContactSurfaceA = false;
            isContactB = false;
            isContactSurfaceB = false;
        }
    }

    private void OnDestroy()
    {
        collision.OnCollisionStayEvent -= OnCollisionStayEvent;
        collision.OnCollisionExitEvent -= OnCollisionExitEvent;
    }

    private void OnDrawGizmos()
    {
        if (isContactA)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(colA.bounds.center, 0.0025f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(contactPointA, 0.0025f);
        }

        if (isContactB)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(colB.bounds.center, 0.0025f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(contactPointB, 0.0025f);
        }
    }
}
