using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _navAgentDisableEdgeDistance = 1f;

    private UnitData _data;
    private Rigidbody _rb;
    private NavMeshAgent _navAgent;

#region MonoBehaviour methods

#endregion

#region Public methods

    public void HandleRotation(float deltaTime, Vector3 lookDirection, bool smooth = true)
    {
        bool shouldNotHandleRotation = (_rb == null)
            || (lookDirection.magnitude <= 0.1f);
        if (shouldNotHandleRotation)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(
            lookDirection,
            transform.up
        );
        lookRotation = Quaternion.Euler(
            _rb.rotation.eulerAngles.x,
            lookRotation.eulerAngles.y,
            _rb.rotation.eulerAngles.z
        );

        Quaternion smoothedLookRotation = Quaternion.Slerp(
            _rb.rotation,
            lookRotation,
            _data.RotationSmoothness * deltaTime
        );

        if (smooth)
            _rb.rotation = smoothedLookRotation;
        else
            _rb.rotation = lookRotation;
    }

    public void Move(float horizontal, float vertical)
    {
        if (_rb == null)
        {
            Debug.LogError($"{gameObject.name} tries to move with {_rb.GetType()}, but none was found.");
            return;
        }

        Vector3 movementDirection = new Vector3(
            horizontal * _data.MovementSpeed,
            _rb.velocity.y,
            vertical * _data.MovementSpeed
        );

        _rb.velocity = movementDirection;
    }

    public void Move(Vector3 targetDestination)
    {
        if (_navAgent == null)
        {
            Debug.LogError($"{gameObject.name} tries to move with {_navAgent.GetType()}, but none was found.");
            return;
        }

        _navAgent.enabled = true;
        NavMeshHit edgeHit;
        bool isEdgeFound = _navAgent.FindClosestEdge(out edgeHit);
        if (isEdgeFound)
        {
            bool targetNavAgentState = edgeHit.distance > _navAgentDisableEdgeDistance;

            _navAgent.enabled = targetNavAgentState;

            if (!targetNavAgentState)
                Debug.Log($"Disable nav agent !!!, distance {edgeHit.distance}");
        }

        _navAgent?.SetDestination(targetDestination);
    }

    public void Setup(UnitData data, Rigidbody rigidbody,
                      NavMeshAgent navMeshAgent = null)
    {
        _data     = data;
        _rb       = rigidbody;
        _navAgent = navMeshAgent;
    }

#endregion

#region Private methods

#endregion
}
