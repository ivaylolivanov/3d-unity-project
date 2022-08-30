using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Unit _unit;
    private Rigidbody _rb;

#region MonoBehaviour methods
    private void OnEnable()    => Initialize();

    private void FixedUpdate() => HandleRotation(Time.fixedDeltaTime);

#endregion

#region Public methods

    public void Move(float horizontal, float vertical)
    {
        Vector3 movementDirection = new Vector3(
            horizontal * _unit.UnitData.MovementSpeed,
            _rb.velocity.y,
            vertical * _unit.UnitData.MovementSpeed
        );

        _rb.velocity = movementDirection;
    }

#endregion

#region Private methods
    private void HandleRotation(float deltaTime)
    {
        if(_rb.velocity.x == 0 && _rb.velocity.z == 0)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(
            _rb.velocity,
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

        _rb.rotation = smoothedLookRotation;
    }

    private void Initialize()
    {
        _unit = GetComponent<Unit>();
        if (_unit == null)
            Debug.LogError($"Failed to get {_unit.GetType()} in {gameObject.name}");

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");
    }

#endregion
}
