using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothness = 1.75f;
    [SerializeField] private Vector3 offset = new Vector3(0, 10, 10);
    [SerializeField] private Vector3 rotationOffset = new Vector3(30, 0, 0);

    [SerializeField] private bool shouldFollowPlayerRotation = false;

    private Transform target;
    private Vector3 initialOffset = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    void OnEnable()
    {
        var player = FindObjectOfType<PlayerMovement>();

        if (player == null)
        {
            Debug.LogError("Failed to find player to follow.");
            return;
        }

        target = player.transform;
        initialOffset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Lost player.");
            return;
        }

        float targetYRotationAngle = target.rotation.eulerAngles.y;
        float targetHeight = target.position.y + offset.y;

        float currentYRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentYRotationAngle = Mathf.LerpAngle (
            currentYRotationAngle,
            targetYRotationAngle,
            smoothness * Time.fixedDeltaTime
        );

        currentHeight = Mathf.Lerp(
            currentHeight,
            targetHeight,
            smoothness * Time.deltaTime
        );

        Quaternion currentRotation = Quaternion.Euler (
            0,
            currentYRotationAngle,
            0
        );

        transform.position = target.position;
        if(shouldFollowPlayerRotation)
            transform.position -= currentRotation * Vector3.forward * offset.z;
        else
            transform.position -= Vector3.forward * offset.z;

        transform.position = new Vector3(
            transform.position.x,
            currentHeight,
            transform.position.z
        );

        transform.LookAt (target);

        transform.rotation = Quaternion.Euler(
            rotationOffset.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );
    }
}
