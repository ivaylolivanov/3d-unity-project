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
        if(_player == null)
            return;

        _navAgent.SetDestination(_player.transform.position);
    }
}
