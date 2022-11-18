using System;
using UnityEngine;

using Utils;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [Space]
    [Header("Detection")]
    [SerializeField] private LayerMask _detectionLayerMask;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private Vector3 _detectionOffset;

    private float _endDurationTimePoint;
    private Rigidbody _rb;

    private Bullet _capturedBullet;
    private Unit _capturedUnit;

    private Func<bool> _popCondition;

    #region MonoBehaviour methods

    private void OnEnable()
    {
        _endDurationTimePoint = 0f;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        _endDurationTimePoint = 0f;
        _popCondition = null;
    }

    private void Update()
    {
        Detection();

        if (_popCondition != null && _popCondition.Invoke())
            Pop();

        if (_capturedUnit != null)
            _capturedUnit.transform.position = transform.position;

        if (_capturedBullet != null)
            _capturedBullet.transform.position = transform.position;

        if (Time.time >= _endDurationTimePoint) Pop();
    }

    #endregion

    public void Initialize(float duration, Vector3 direction,
                           Func<bool> popCondition = null)
    {
        _rb.velocity = Vector3.zero;

        _endDurationTimePoint = Time.time + duration;
        _popCondition = popCondition;
        _rb.velocity = direction * _movementSpeed;
    }

    private void Detection()
    {
        Collider[] cols = Physics.OverlapSphere(
            transform.position + _detectionOffset,
            _detectionRadius,
            _detectionLayerMask);

        if (cols.Length <= 0)
            return;

        Unit unitTarget = cols[0].transform.GetComponent<Unit>();
        Bullet bulletTarget = cols[0].transform.GetComponent<Bullet>();

        if (unitTarget != null)
        {
            _capturedUnit = unitTarget;
            _capturedUnit.IsInBubble = true;
        }
        else if (bulletTarget != null)
        {
            _capturedBullet = bulletTarget;
            _capturedBullet.IsInBubble = true;
        }
    }

    private void Pop()
    {
        if (_capturedUnit != null)
            _capturedUnit.IsInBubble = false;
        if (_capturedBullet != null)
            _capturedBullet.IsInBubble = false;

        _capturedUnit = null;
        _capturedBullet = null;
        _popCondition = null;
        ObjectsPools.DisableInstance<Bubble>(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position + _detectionOffset,
            _detectionRadius);
    }
}
