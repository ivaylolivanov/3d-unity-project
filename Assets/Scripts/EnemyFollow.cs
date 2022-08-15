using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float _attackRadius = 12f;
    [SerializeField] private float _viewRadius = 17f;

    private NavMeshAgent _navAgent;
    private Shooter _shooter;

    private GameObject _target;

    void OnEnable()
    {
        _navAgent = FindObjectOfType<NavMeshAgent>();
        if (_navAgent == null)
            Debug.Log($"Failed to find {_navAgent.GetType()} in enemy - {gameObject.name}.");
        else
            _navAgent.stoppingDistance = _attackRadius;

        _shooter = FindObjectOfType<Shooter>();
        if (_shooter == null)
            Debug.Log($"Failed to find {_shooter.GetType()} in enemy - {gameObject.name}.");

        _target = FindObjectOfType<Player>()?.gameObject;
        if (_target == null)
            Debug.Log($"Failed to find player in enemy - {gameObject.name}.");
    }

    void Update()
    {
        if (_target == null)
            return;

        Vector3 directionToPlayer = _target.transform.position
            - transform.position;
        float distanceToPlayer = directionToPlayer.sqrMagnitude;

        if (distanceToPlayer > (_viewRadius * _viewRadius))
            return;

        if (!IsTargetOnLineOfSight())
            return;

        _navAgent.SetDestination(_target.transform.position);

        if (distanceToPlayer > (_attackRadius * _attackRadius))
            return;

        Quaternion rotateTowardTarget = Quaternion.LookRotation(
            directionToPlayer,
            transform.up
        );

        var targetRotation = new Vector3(
            transform.rotation.x,
            rotateTowardTarget.eulerAngles.y,
            transform.rotation.z
        );

        transform.rotation = Quaternion.Euler(targetRotation);

        _shooter.Shoot();
    }

    private bool IsTargetOnLineOfSight()
    {
        bool result = false;

        RaycastHit hitInfo;
        Physics.Raycast(
            transform.position,
            _target.transform.position - transform.position,
            out hitInfo,
            _viewRadius
        );

        result = Object.ReferenceEquals(hitInfo.transform.gameObject, _target);

        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
