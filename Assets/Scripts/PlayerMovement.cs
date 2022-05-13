using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 8f;
    [SerializeField] private float _drag = 6f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10f;

    // [Header("Rotation")]
    // [SerializeField] private float _rotationSmoothTime = 0.1f;
    // [SerializeField] private float _rotationSpeed = 50f;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpKeyPressed;

    private Camera mainCamera;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");
        }
        else
        {
            _rb.freezeRotation = true;
            _rb.drag = _drag;
        }

        mainCamera = Camera.main;
        _jumpKeyPressed = false;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _jumpKeyPressed = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        Vector3 movementDirection = new Vector3(
            _horizontalInput,
            _rb.velocity.y,
            _verticalInput
        );

        // if(_jumpKeyPressed)
            // movementDirection += Vector3.up * _jumpForce;

        Vector3 movement = movementDirection.normalized
            * _movementSpeed
            * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }
}
