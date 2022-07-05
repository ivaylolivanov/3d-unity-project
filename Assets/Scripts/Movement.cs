using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _fallSpeed = 10f;
    [SerializeField] private float _movementSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _rotationSmoothness = 10f;

    [Header("Jump")]
    [SerializeField] private int _groundCheckLayer;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.5f;

    private Rigidbody _rb;

#region MonoBehaviour methods
    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");
    }

    private void FixedUpdate()
    {
        HandleRotation(Time.fixedDeltaTime);
        AdjustFallingSpeed();
    }
#endregion

#region Public methods
    public void Move(float horizontal, float vertical)
    {
        Vector3 movementDirection = new Vector3(
            horizontal * _movementSpeed,
            _rb.velocity.y,
            vertical * _movementSpeed
        );

        _rb.velocity = movementDirection;
    }

    public void Jump()
    {
        if (!IsOnGround()) return;

        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpForce;
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
    }
#endregion

#region Private methods
    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y >= -0.1f)
            return;

        _rb.velocity = Vector3.right * _rb.velocity.x
            + Vector3.down * _fallSpeed
            + Vector3.forward * _rb.velocity.z;
    }

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
            _rotationSmoothness * deltaTime
        );

        _rb.rotation = smoothedLookRotation;
    }

    private bool IsOnGround()
    {
        bool result = Physics.CheckSphere(
            _groundCheckPoint.position,
            _groundCheckRadius,
            _groundCheckLayer
        );

        return result;
    }
#endregion
}
