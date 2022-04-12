using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothness = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(-5, 10, 0);

    private Transform target;
    private Vector3 smoothVelocity = Vector3.zero;

    void Awake()
    {
        var player = FindObjectOfType<PlayerMovement>();

        if (player == null)
        {
            Debug.LogError("Failed to find player to follow.");
            return;
        }

        target = player.transform;
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Lost player.");
            return;
        }

        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref smoothVelocity,
            smoothness
        );

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
