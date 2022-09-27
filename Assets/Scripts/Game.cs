using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public static UnityAction<int> OnUpdateScores;

    private Player _player;

    private int _playerScore = 0;

    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
            Debug.LogError($"Failed to find {_player.GetType()}");

        Enemy.OnEnemyDead += EnemyDied;
    }

    private void EnemyDied()
    {
        ++_playerScore;
        OnUpdateScores?.Invoke(_playerScore);
    }
}
