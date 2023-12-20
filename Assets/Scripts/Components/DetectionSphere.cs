using System;
using UnityEngine;
using UnityEngine.Events;

using Utils;

public class DetectionSphere : MonoBehaviour
{
    [SerializeField] private LayerMask _detectionLayerMask;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private Vector3 _detectionOffset;

    public UnityAction<Collider> Detected;

    private void Update() => Detection();

    private void Detection()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(
            transform.position + _detectionOffset,
            _detectionRadius,
            _detectionLayerMask);

        if (detectedColliders.Length <= 0)
            return;

        Collider closestCollider = detectedColliders[0];
        Detected?.Invoke(closestCollider);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + _detectionOffset,
            _detectionRadius);
    }
}
