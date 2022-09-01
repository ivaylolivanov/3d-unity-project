using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EnemyData", menuName = "UnitData/EnemyData", order = 1)]
public class EnemyData : UnitData
{
    [Space]
    [Header("Attack")]
    [SerializeField] public float AttackRadius = 12f;
    [SerializeField] public float ViewRadius = 17f;
    [Space]
    [SerializeField] public float ShootInterval = 0.2f;
    [SerializeField] public float ShootForce = 30f;
}
