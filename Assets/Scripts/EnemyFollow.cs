using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;

    private Player _player;

    void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        if(_player == null)
            Debug.Log($"Failed to find player in {gameObject.name}.");
    }

    // Update is called once per frame
    void Update()
    {
        enemy.SetDestination(Player.position);
    }
}
