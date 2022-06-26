using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool _freezeRotation = true;
    [SerializeField] private float _mass = 10f;
    [SerializeField] private float _drag = 6f;
    [SerializeField] private float _fallSpeed = 10f;

    [Space]
    [Header("MouseState")]
    [SerializeField] private MouseStateSO _mouseState;

    [Header("Movement")]
    [SerializeField] private InputActionSO _inputAxisHorizontal;
    [SerializeField] private InputActionSO _inputAxisVertical;
    [SerializeField] private float _rotationSmoothness = 10f;
    [SerializeField] private float _movementSpeed = 8f;

    [Header("Jump")]
    [SerializeField] private InputActionSO _inputActionJump;
    [SerializeField] private int _groundCheckLayer;
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("Shooting")]
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootInterval = 0.2f;
    [SerializeField] private InputActionSO  _inputActionShoot;
    [SerializeField] private float _shootForce = 30f;

    // Shooting
    private float _nextShootTime;

    private ObjectsPools _objectsPools;

    private Rigidbody _rb;

    void OnEnable() => Initialize();
    void Update() => HandleRotation(Time.deltaTime);

    void FixedUpdate()
    {
        bool isOnGround = IsOnGround();

        AdjustFallingSpeed();
        Move();

        if(_inputActionJump.WasDown() && isOnGround)
            Jump();

        if (_inputActionShoot.IsDown() && (Time.time >= _nextShootTime))
        {
            RotateToMouse();
            Shoot();
        }
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

        _nextShootTime = 0;

        _objectsPools = FindObjectOfType<ObjectsPools>();
        if (_objectsPools == null)
            Debug.LogError($"Failed to get ObjectsPools in {gameObject.name}");
    }

    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y < 0)
            _rb.velocity = Vector3.down * _fallSpeed;
    }

    private void HandleRotation(float deltaTime)
    {
        bool noDataFromInputAxes =
            _inputAxisHorizontal.GetValueNormalized() == 0
            && _inputAxisVertical.GetValueNormalized() == 0;

        if (noDataFromInputAxes) return;

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

    private void Move()
    {
        Vector3 movementDirection = new Vector3(
            _inputAxisHorizontal.GetValueNormalized()  * _movementSpeed,
            _rb.velocity.y,
            _inputAxisVertical.GetValueNormalized() * _movementSpeed
        );

        _rb.velocity = movementDirection;
    }

    private void Jump()
    {
        Vector3 velocity = _rb.velocity;
        velocity.y = _jumpForce;
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
    }

    private void Shoot()
    {
        Bullet bullet = _objectsPools.GetBulletInstance(_shootPoint.position);
        if(bullet == null)
            return;

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if(bulletRigidbody == null)
            return;

        bulletRigidbody.AddForce(
            _shootPoint.forward * _shootForce,
            ForceMode.Impulse
        );

        _nextShootTime = Time.time + _shootInterval;
    }

    private void RotateToMouse()
    {
        Vector3 playerToMouseDirection = _mouseState.MouseWorldPosition - _rb.position;
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

        _rb.rotation = targetRotation;
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}
