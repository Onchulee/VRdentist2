using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class BoundsDrawer : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmosSelected()
    {
        if (!rb) return;
        Bounds bounds = new Bounds();
        bounds.center = transform.position;
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders) {
            bounds.Encapsulate(col.bounds);
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bounds.center, bounds.extents*2);
    }
}
