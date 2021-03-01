using com.dgn.UnityAttributes;
using System.Collections;
using System.Collections.Generic;
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
    [ReadOnly]
    [SerializeField]
    private bool contactA;
    [ReadOnly]
    [SerializeField]
    private bool contactSurfaceA;
    [ReadOnly]
    [SerializeField]
    private float contactOffsetA;
    [ReadOnly]
    [SerializeField]
    private Vector3 contactPointA;
    [ReadOnly]
    [SerializeField]
    private Vector3 centerA;



    public Collider colB;
    [ReadOnly]
    [SerializeField]
    private bool contactB;
    [ReadOnly]
    [SerializeField]
    private bool contactSurfaceB;
    [ReadOnly]
    [SerializeField]
    private float contactOffsetB;
    [ReadOnly]
    [SerializeField]
    private Vector3 contactPointB;
    [ReadOnly]
    [SerializeField]
    private Vector3 centerB;

    protected override void Start()
    {
        base.Start();
        collision.OnCollisionStayEvent += OnCollisionEnterEvent;
        collision.OnCollisionExitEvent += OnCollisionExitEvent;
        defaultConstraints = rigid.constraints;
        fixedConstraints = RigidbodyConstraints.FreezeAll;
    }

    private void FixedUpdate()
    {
        rigid.constraints = defaultConstraints;
        if (contactA == true && contactSurfaceA == false)
        {
            lockTransform.localRotation = Quaternion.Euler(limitAngles.max);
            rigid.constraints = fixedConstraints;
        }
        if (contactB == true && contactSurfaceB == false)
        {
            lockTransform.localRotation = Quaternion.Euler(limitAngles.min);
            rigid.constraints = fixedConstraints;
        }
        if (contactA && contactB)
        {
            lockTransform.localRotation = Quaternion.Euler((limitAngles.min + limitAngles.max) * 0.5f);
            rigid.constraints = fixedConstraints;
        }
    }

    void LateUpdate()
    {
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

    private bool GetContact(ContactPoint contactPoint, Collider collider) {
        return Vector3.Distance(contactPoint.point, collider.ClosestPoint(contactPoint.point)) < 0.01f;
    }

    private void OnCollisionEnterEvent(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (contactPoint.thisCollider == colA) {
                contactOffsetA = Vector3.Distance(contactPoint.point, colA.ClosestPoint(contactPoint.point));
                contactA = GetContact(contactPoint, colA);
                contactPointA = contactPoint.point;
                centerA = contactPoint.otherCollider.bounds.center;
                contactSurfaceA = contactA && contactPoint.point.y < centerA.y;
            }
            if (contactPoint.thisCollider == colB)
            {
                contactOffsetB = Vector3.Distance(contactPoint.point, colB.ClosestPoint(contactPoint.point));
                contactB = GetContact(contactPoint, colB);
                contactPointB = contactPoint.point;
                centerB = contactPoint.otherCollider.bounds.center;
                contactSurfaceB = contactB && contactPoint.point.y < centerB.y;
            }
        }
    }

    private void OnCollisionExitEvent(Collision collision)
    {
        if (collision.contacts.Length <= 0) {
            contactA = false;
            contactB = false;
        }
    }

    private void OnDestroy()
    {

        collision.OnCollisionEnterEvent -= OnCollisionEnterEvent;
        collision.OnCollisionExitEvent -= OnCollisionExitEvent;
    }

    private void OnDrawGizmos()
    {
        if (contactA)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(centerA, 0.0025f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(contactPointA, 0.0025f);
        }

        if (contactB)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(centerB, 0.0025f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(contactPointB, 0.0025f);
        }
    }
}
