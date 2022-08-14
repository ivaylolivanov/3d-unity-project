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

    private Player _player;

    void OnEnable()
    {
        _navAgent = FindObjectOfType<NavMeshAgent>();
        if (_navAgent == null)
            Debug.Log($"Failed to find {_navAgent.GetType()} in {gameObject.name}.");
        else
            _navAgent.stoppingDistance = _attackRadius;

        _player = FindObjectOfType<Player>();
        if (_player == null)
            Debug.Log($"Failed to find player in {gameObject.name}.");
    }

    void Update()
    {
        if (_player == null)
            return;

        float distanceToPlayer = (transform.position - _player.transform.position)
            .sqrMagnitude;

        if (distanceToPlayer > (_viewRadius * _viewRadius))
            return;

        if (!IsTargetOnLineOfSight())
            return;

        _navAgent.SetDestination(_player.transform.position);
    }

    private bool IsTargetOnLineOfSight()
    {
        bool result = false;

        RaycastHit hitInfo;
        Physics.Raycast(
            transform.position,
            _player.transform.position - transform.position,
            out hitInfo,
            _viewRadius
        );

        result = Object.ReferenceEquals(hitInfo.transform.gameObject, _player.gameObject);

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
