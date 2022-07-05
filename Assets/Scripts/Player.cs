using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool _freezeRotation = true;
    [SerializeField] private float _mass = 10f;
    [SerializeField] private float _drag = 6f;

    [Space]
    [Header("Controls")]
    [SerializeField] private MouseStateSO _mouseState;
    [Space]
    [SerializeField] private InputActionSO _inputAxisHorizontal;
    [SerializeField] private InputActionSO _inputAxisVertical;
    [SerializeField] private InputActionSO _inputActionJump;
    [SerializeField] private InputActionSO _inputActionShoot;

    // Private fields
    private Movement _movement;
    private Shooter _shooter;

    // Unity components
    private Rigidbody _rb;

    private void OnEnable() => Initialize();

    private void FixedUpdate()
    {
        _movement.Move(
            _inputAxisHorizontal.GetValueNormalized(),
            _inputAxisVertical.GetValueNormalized()
        );

        if(_inputActionJump.WasDown())
            _movement.Jump();

        if (_inputActionShoot.IsDown())
        {
            RotateToMouse();
            _shooter.Shoot();
        }
    }

    private void Initialize()
    {
        _movement = GetComponent<Movement>();
        if(_movement == null)
            Debug.LogError($"Failed to get movement in {gameObject.name}");

        _shooter = GetComponent<Shooter>();
        if(_shooter == null)
            Debug.LogError($"Failed to get shooter in {gameObject.name}");

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
}
