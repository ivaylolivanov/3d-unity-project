using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _smoothness = 1.75f;
    [SerializeField] private Vector3 _offset = new Vector3(0, 10, 10);
    [SerializeField] private Vector3 _rotationOffset = new Vector3(30, 0, 0);

    [SerializeField] private bool _shouldFollowPlayerRotation = false;

    private Transform _target;
    private Vector3 _initialOffset = Vector3.zero;
    private Vector3 _smoothVelocity = Vector3.zero;

    void OnEnable()
    {
        var player = FindObjectOfType<PlayerMovement>();

        if (player == null)
        {
            Debug.LogError("Failed to find player to follow.");
            return;
        }

        _target = player.transform;
        _initialOffset = transform.position - _target.position;
    }

    void FixedUpdate()
    {
        if (_target == null)
        {
            Debug.LogError("Lost player.");
            return;
        }

        float targetYRotationAngle = _target.rotation.eulerAngles.y;
        float targetHeight = _target.position.y + _offset.y;

        float currentYRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentYRotationAngle = Mathf.LerpAngle (
            currentYRotationAngle,
            targetYRotationAngle,
            _smoothness * Time.fixedDeltaTime
        );

        currentHeight = Mathf.Lerp(
            currentHeight,
            targetHeight,
            _smoothness * Time.deltaTime
        );

        Quaternion currentRotation = Quaternion.Euler (
            0,
            currentYRotationAngle,
            0
        );

        transform.position = _target.position;
        if(_shouldFollowPlayerRotation)
            transform.position -= currentRotation * Vector3.forward * _offset.z;
        else
            transform.position -= Vector3.forward * _offset.z;

        transform.position = new Vector3(
            transform.position.x,
            currentHeight,
            transform.position.z
        );

        transform.LookAt (_target);

        transform.rotation = Quaternion.Euler(
            _rotationOffset.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );
    }
}
