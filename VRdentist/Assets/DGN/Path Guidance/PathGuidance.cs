using com.dgn.UnityAttributes;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class PathGuidance : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    public float speed = 1f;
    [SerializeField]
    [Tooltip("Particles will stop when this transform within target range")]
    public float stopRange = 0.2f;


    [SerializeField]
    private LayerMask layerMask;

    private List<Vector3> nodes;

    private Vector3 StartPoint {
        get { return transform.position; }
    }
    private Vector3 EndPoint
    {
        get  { return target.transform.position; }
    }

    [ReadOnly]
    [SerializeField]
    private int maxParticles;

    [ReadOnly]
    [SerializeField]
    private int particleCount;
    
    float totalDist = 0;

    new ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;

    ParticleSystem.MainModule particleSystemMainModule;

    private bool isEmitting;
    
    private void Start()
    {
        nodes = new List<Vector3>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;
    }

    private void OnEnable()
    {
        nodes = new List<Vector3>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;
        isEmitting = particleSystem.isEmitting;
        if(isEmitting)  Emit();
    }

    private void OnDisable()
    {
        Stop();
    }

    void Update()
    {
        if (target && particleSystem.isPlaying) {
            bool isUpdated = TryGeneratePathways();
            CreateBreakPoints();
        }
    }

    void LateUpdate()
    {
        if (particleSystem.isPlaying)
        {
            maxParticles = particleSystemMainModule.maxParticles;
            if (particles == null || particles.Length < maxParticles)
            {
                particles = new ParticleSystem.Particle[maxParticles];
            }

            if (target && totalDist > stopRange) {
                AddForceParticles();
            }
            else
            {
                particleSystem.SetParticles(particles, 0);
            }
        }
    }

    private void AddForceParticles() {
        particleSystem.GetParticles(particles);
        particleCount = particleSystem.particleCount;
        for (int i = 0; i < particleCount; i++)
        {
            int targetNode = GetTargetNode(particles[i]);
            Vector3 fromPos = TransformedPosition(targetNode == Mathf.Clamp(targetNode, 1, nodes.Count) ? nodes[targetNode - 1] : StartPoint);
            Vector3 toPos = TransformedPosition(targetNode < nodes.Count ? nodes[targetNode] : EndPoint);

            Vector3 projPos = fromPos + Vector3.Project(particles[i].position - fromPos, toPos - fromPos);
            float distParticles = Vector3.Distance(projPos, particles[i].position);
            float particleDiameter = particleSystem.shape.radius * 2f;

            Vector3 seekForce = Vector3.zero;

            if (particleDiameter > 0 && distParticles > particleDiameter)
            {
                // particle is far from path
                seekForce = GenerateForce(particles[i].position, toPos, speed);
            }
            else if (Vector3.SqrMagnitude(particles[i].position - fromPos) > Vector3.SqrMagnitude(toPos - fromPos))
            {
                // particle is passed target point, kill this particle
                particles[i].remainingLifetime = 0;
            }
            else
            { 
                // particle is within path
                seekForce = GenerateForce(fromPos, toPos, speed);
                particles[i].remainingLifetime = 1;
            }
            particles[i].velocity = seekForce;
        }
        particleSystem.SetParticles(particles, particleCount);
    }

    public void Emit() {
        particleSystem.Clear(true);
        particleSystem.Play(true);
    }

    public void Stop() {
        particleSystem.Stop(true);
        particleSystem.Clear(true);
    }

    public void SetParent(Transform parent) {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private Vector3 GenerateForce(Vector3 from, Vector3 to, float speed) {
        return Vector3.Normalize(to - from) * speed;
    }
    
    private bool TryGeneratePathways() {
        bool isChanged = false;
        int iter = 0;
        Vector3 traverseNode = StartPoint;
        Vector3 nextPoint = traverseNode;
        HashSet<Transform> checkedHits = new HashSet<Transform>();
        while (traverseNode != EndPoint)
        {
            Vector3 directionToTarget = Vector3.Normalize(EndPoint - traverseNode);
            float dist = Vector3.Distance(EndPoint, traverseNode);
            nextPoint = EndPoint;
            if (Physics.Raycast(traverseNode, directionToTarget, out RaycastHit hit,  dist, layerMask) && !IsTargetOrSelf(hit) && !checkedHits.Contains(hit.transform))
            {
                checkedHits.Add(hit.transform);
                if (!IsInside(hit)) nextPoint = GetAvoidPoint(traverseNode, directionToTarget, hit);
                if (nodes.Count <= iter)
                {
                    nodes.Add(nextPoint);
                    isChanged = true;
                }
                else if (nodes[iter] != nextPoint)
                {
                    nodes.RemoveRange(iter, nodes.Count - iter);
                    nodes.Add(nextPoint);
                    isChanged = true;
                }
            }
            else if (nodes.Count > iter)
            {
                nodes.RemoveRange(iter, nodes.Count - iter);
                isChanged = true;
            }
            traverseNode = nextPoint;
            iter++;
        }
        return isChanged;
    }

    private bool IsInside(RaycastHit hit) {
        bool isInside = hit.collider.bounds.Contains(StartPoint) || hit.collider.ClosestPoint(StartPoint) == StartPoint;
        if (target) isInside = isInside || hit.collider.bounds.Contains(target.position) || hit.collider.ClosestPoint(target.position) == target.position;
        return isInside;
    }

    private void CreateBreakPoints() {
        totalDist = 0;
        Vector3 traverse = StartPoint;
        for (int i = 0; i < nodes.Count; i++)
        {
            totalDist += Vector3.Distance(traverse, nodes[i]);
            traverse = nodes[i];
        }
        totalDist += Vector3.Distance(traverse, EndPoint);
    }

    private Vector3 GetAvoidPoint(Vector3 initPos, Vector3 dirToTarget, RaycastHit hit)
    {
        Vector3 avoidPoint = hit.point;
        Vector3[] validPoints = GetValidBoundPoints(hit);
        if (validPoints.Length > 0) {
            avoidPoint = validPoints[0];
            foreach (Vector3 point in validPoints)
            {
                avoidPoint = GetNearestPoint(initPos, avoidPoint, point);
            }
        }
        return avoidPoint;
    }

    private bool CreatePoint(Vector3 center, Vector3 dir, float distance, Transform self, out Vector3 createdPoint) {
        createdPoint = center + (dir * distance);
        if (Physics.Raycast(createdPoint, Vector3.Normalize(EndPoint - createdPoint), out RaycastHit checkHit, distance) && checkHit.transform == self) {
            return false;
        }
        return true;
    }

    private Vector3[] GetValidBoundPoints(RaycastHit hit)
    {
        List<Vector3> validPoints = new List<Vector3>();
        // we have to create combined bounds
        Bounds bounds = new Bounds();
        bounds.center = hit.transform.position;
        Collider[] colliders = hit.transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }
        // use combined bound to find valid 6 points
        if (CreatePoint(bounds.center, Vector3.right, bounds.extents.x, hit.transform, out Vector3 rPt))
        {
            validPoints.Add(rPt);
        }
        if (CreatePoint(bounds.center, Vector3.left, bounds.extents.x, hit.transform, out Vector3 lPt))
        {
            validPoints.Add(lPt);
        }
        if (CreatePoint(bounds.center, Vector3.up, bounds.extents.y, hit.transform, out Vector3 uPt))
        {
            validPoints.Add(uPt);
        }
        if (CreatePoint(bounds.center, Vector3.down, bounds.extents.y, hit.transform, out Vector3 dPt))
        {
            validPoints.Add(dPt);
        }
        if (CreatePoint(bounds.center, Vector3.forward, bounds.extents.z, hit.transform, out Vector3 fPt))
        {
            validPoints.Add(fPt);
        }
        if (CreatePoint(bounds.center, Vector3.back, bounds.extents.z, hit.transform, out Vector3 bPt))
        {
            validPoints.Add(bPt);
        }
        return validPoints.ToArray();
    }
    
    private Vector3 GetNearestPoint(Vector3 initPos, Vector3 pt1, Vector3 pt2)
    {
        float pDist1 = Vector3.Distance(initPos, pt1);
        float pDist2 = Vector3.Distance(initPos, pt2);
        return pDist1 < pDist2 ? pt1 : pt2;
    }

    private bool IsTargetOrSelf(RaycastHit hit)
    {
        return hit.transform == target || hit.transform == transform;
    }

    private int GetTargetNode(ParticleSystem.Particle particle) {
        if (nodes.Count < 1) { return 1; }
        int targetNode = 0;
        Vector3 projVector = StartPoint + Vector3.Project(particle.position - StartPoint, nodes[targetNode] - StartPoint);
        Vector3 forward = Vector3.Normalize(nodes[targetNode] - projVector);
        Vector3 toOther = Vector3.Normalize(nodes[targetNode] - StartPoint);
        while (Vector3.Dot(forward, toOther) < 1f) {
            targetNode = targetNode + 1;
            if (targetNode >= nodes.Count) break;
            projVector = nodes[targetNode-1] + Vector3.Project(particle.position - nodes[targetNode-1], nodes[targetNode] - nodes[targetNode-1]);
            forward = Vector3.Normalize(nodes[targetNode] - particle.position);
            toOther = Vector3.Normalize(nodes[targetNode] - nodes[targetNode-1]);
        }
        return targetNode;
    }

    private Vector3 TransformedPosition(Vector3 position) {
        Vector3 targetTransformedPosition = position;
        switch (particleSystemMainModule.simulationSpace)
        {
            case ParticleSystemSimulationSpace.Local:
                {
                    targetTransformedPosition = transform.InverseTransformPoint(position);
                    break;
                }
            case ParticleSystemSimulationSpace.Custom:
                {
                    targetTransformedPosition = particleSystemMainModule.customSimulationSpace.InverseTransformPoint(position);
                    break;
                }
            case ParticleSystemSimulationSpace.World:
                {
                    targetTransformedPosition = position;
                    break;
                }
        }
        return targetTransformedPosition;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(StartPoint, 0.01f);
        if (target) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(target.position, 0.01f);
            Gizmos.color = Color.green;
            if (nodes.Count > 0) {
                Vector3 traverse = StartPoint;
                foreach (Vector3 point in nodes) {
                    Gizmos.DrawSphere(point, 0.01f);
                    Gizmos.DrawLine(traverse, point);
                    traverse = point;
                }
                Gizmos.DrawLine(traverse, target.position);
            }
            else {
                Gizmos.DrawLine(StartPoint, target.position);
            }
        }
    }
}
