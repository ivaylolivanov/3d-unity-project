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
    [SerializeField] private float _rotateToMouseScale = 3f;

    [Header("Jump")]
    [SerializeField] private KeyCode _jumpKeyCode = KeyCode.Space;
    [SerializeField] private int _groundCheckLayer;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private float _jumpForce = 10f;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _jumpKeyPressed;
    private Vector3 _mousePosition;
    private Vector3 _mouseWorldPosition;

    private Camera mainCamera;

    private Rigidbody _rb;

    void OnEnable() => Initialize();

    void Update() => ReadInputs();

    void FixedUpdate()
    {
        bool isOnGround = IsOnGround();

        RotateToMouse();
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

    private void RotateToMouse()
    {
        Vector3 playerToMouseDirection = _mouseWorldPosition - _rb.position;
        float angle = Vector3.SignedAngle(
            transform.forward,
            playerToMouseDirection,
            transform.up
        );

        Quaternion targetRotation = Quaternion.LookRotation(
            playerToMouseDirection,
            transform.up
        );

        // Do rotation ONLY around the Y axis
        targetRotation = Quaternion.Euler(
            _rb.rotation.eulerAngles.x,
            targetRotation.eulerAngles.y,
            _rb.rotation.eulerAngles.z
        );

        _rb.rotation = Quaternion.Slerp(
            _rb.rotation,
            targetRotation,
            _rotateToMouseScale * Time.fixedDeltaTime
        );
    }

    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = Vector3.down * _fallSpeed;
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

    private bool IsOnGround()
    {
        bool result = Physics.CheckSphere(
            _groundCheckPoint.position,
            _groundCheckRadius,
            _groundCheckLayer
        );

        return result;
    }

    private void ReadInputs()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _jumpKeyPressed = Input.GetKey(_jumpKeyCode);
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 0;

        Ray mouseWorldRay = mainCamera.ScreenPointToRay(_mousePosition);

        if (Physics.Raycast(mouseWorldRay, out RaycastHit raycastHit))
            _mouseWorldPosition = raycastHit.point;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
