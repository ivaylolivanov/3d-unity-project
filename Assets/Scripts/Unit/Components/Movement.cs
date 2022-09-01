using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    private Unit _unit;
    private Rigidbody _rb;
    private NavMeshAgent _navAgent;

#region MonoBehaviour methods

    private void OnEnable() => Initialize();

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
            _unit.UnitData.RotationSmoothness * deltaTime
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
            horizontal * _unit.UnitData.MovementSpeed,
            _rb.velocity.y,
            vertical * _unit.UnitData.MovementSpeed
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

        _navAgent.SetDestination(targetDestination);
    }

#endregion

#region Private methods

    private void Initialize()
    {
        _unit = GetComponent<Unit>();
        if (_unit == null)
            Debug.LogError($"Failed to get {_unit.GetType()} in {gameObject.name}");

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");

        _navAgent = GetComponent<NavMeshAgent>();
        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");
    }

#endregion
}
