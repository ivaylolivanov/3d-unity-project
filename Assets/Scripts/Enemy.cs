using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _attackRadius = 12f;
    [SerializeField] private float _viewRadius = 17f;

    private NavMeshAgent _navAgent;
    private Shooter _shooter;

    private GameObject _target;

    void OnEnable() => Initialize();

    void Update()
    {
        if (_target == null)
            return;

        Vector3 directionToTarget = Helpers.Direction(
            transform.position,
            _target.transform.position
        );
        float distanceToTarget = directionToTarget.sqrMagnitude;

        if (!Helpers.IsInRange(directionToTarget, _viewRadius))
            return;

        if (!IsTargetOnLineOfSight())
            return;

        _navAgent.SetDestination(_target.transform.position);

        if (!Helpers.IsInRange(directionToTarget, _attackRadius))
            return;

        LookInDirection(directionToTarget);

        _shooter.Shoot();
    }

    private bool IsTargetOnLineOfSight()
    {
        bool result = false;

        RaycastHit hitInfo;
        Physics.Raycast(
            transform.position,
            Helpers.Direction(transform.position, _target.transform.position),
            out hitInfo,
            _viewRadius
        );

        if (hitInfo.transform == null || hitInfo.transform?.gameObject == null)
            return result;

        result = Object.ReferenceEquals(hitInfo.transform.gameObject, _target);

        return result;
    }

    private void LookInDirection(Vector3 directionToPlayer)
    {
        Quaternion lookAtTarget = Quaternion.LookRotation(
            directionToPlayer,
            transform.up
        );

        var desiredRotation = new Vector3(
            transform.rotation.x,
            lookAtTarget.eulerAngles.y,
            transform.rotation.z
        );

        transform.rotation = Quaternion.Euler(desiredRotation);
    }

    private void Initialize()
    {
        _navAgent = FindObjectOfType<NavMeshAgent>();
        if (_navAgent == null)
            Debug.LogError($"Failed to find {_navAgent.GetType()} in enemy - {gameObject.name}.");
        else
            _navAgent.stoppingDistance = _attackRadius;

        _shooter = FindObjectOfType<Shooter>();
        if (_shooter == null)
            Debug.LogError($"Failed to find {_shooter.GetType()} in enemy - {gameObject.name}.");

        _target = FindObjectOfType<Player>()?.gameObject;
        if (_target == null)
            Debug.LogError($"Failed to find player in enemy - {gameObject.name}.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }

}
