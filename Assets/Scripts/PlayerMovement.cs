using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool _freezeRotation = true;
    [SerializeField] private float _mass = 10f;
    [SerializeField] private float _drag = 6f;
    [SerializeField] private float _fallSpeed = 10f;

    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 8f;

    [Header("Jump")]
    [SerializeField] private KeyCode _jumpKeyCode = KeyCode.Space;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _jumpForce = 10f;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpKeyPressed;

    private Camera mainCamera;

    private Rigidbody _rb;

    void OnEnable() =>Initialize();

    void Update() => ReadInputs();

    void FixedUpdate()
    {
        bool isOnGround = IsOnGround();

        AdjustFallingSpeed();
        Move();

        if (_jumpKeyPressed && isOnGround)
            Jump();
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");
        }
        else
        {
            _rb.freezeRotation = _freezeRotation;
            _rb.mass = _mass;
            _rb.drag = _drag;
        }

        mainCamera = Camera.main;
        _jumpKeyPressed = false;
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(
            _horizontalInput * _movementSpeed,
            _rb.velocity.y,
            _verticalInput * _movementSpeed
        );

        _rb.velocity = movementDirection;
    }

    private void Jump()
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpForce;
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
    }

    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = Vector3.down * _fallSpeed;
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position, -transform.up, _groundCheckDistance);
    }

    private void ReadInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _jumpKeyPressed = Input.GetKey(_jumpKeyCode);
    }
}
