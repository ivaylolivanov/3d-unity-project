using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 8f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSmoothTime = 0.1f;
    [SerializeField] private float _rotationSpeed = 50f;

    private float _horizontalInput;
    private float _verticalInput;

    private Camera mainCamera;

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");

        mainCamera = Camera.main;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        if(_horizontalInput == 0 && _verticalInput == 0)
            return;

        Vector3 movement = new Vector3(_horizontalInput, 0f, _verticalInput)
            * _movementSpeed
            * Time.fixedDeltaTime;

        float targetAngle = Mathf.Atan2(movement.x, movement.z)
            * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float refRotationSmoothVelocity = _rotationSmoothTime;
        float angle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref refRotationSmoothVelocity,
            _rotationSmoothTime
        );

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        _rb.MovePosition(_rb.position + moveDirection.normalized * _movementSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
    }
}
