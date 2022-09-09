using UnityEngine;
using UnityEngine.AI;
using Utils;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Unit
{
    public EnemyData EnemyData => (EnemyData)base._unitData;

    private NavMeshAgent _navAgent;
    private Movement _movement;
    private Shooter _shooter;

    private GameObject _target;

#region MonoBehaviour methods

    protected override void OnEnable()
    {
        base.OnEnable();

        Initialize();
    }

    void Update()
    {
        if (_target == null)
            return;

        Vector3 directionToTarget = Helpers.Direction(
            transform.position,
            _target.transform.position
        );
        float distanceToTarget = directionToTarget.sqrMagnitude;

        if (!Helpers.IsInRange(directionToTarget, EnemyData.ViewRadius))
            return;

        if (!IsTargetOnLineOfSight())
            return;

        _movement.HandleRotation(Time.fixedDeltaTime, directionToTarget);
        _movement.Move(_target.transform.position);

        if (!Helpers.IsInRange(directionToTarget, EnemyData.AttackRadius))
            return;

        _shooter.Shoot();
    }

#endregion

#region Private methods

    private bool IsTargetOnLineOfSight()
    {
        bool result = false;

        RaycastHit hitInfo;
        Physics.Raycast(
            transform.position,
            Helpers.Direction(transform.position, _target.transform.position),
            out hitInfo,
            EnemyData.ViewRadius
        );

        if (hitInfo.transform == null || hitInfo.transform?.gameObject == null)
            return result;

        result = Object.ReferenceEquals(hitInfo.transform.gameObject, _target);

        return result;
    }

    private void Initialize()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        if (_navAgent == null)
            Debug.LogError($"Failed to find {_navAgent.GetType()} in enemy - {gameObject.name}.");
        else
            _navAgent.stoppingDistance = EnemyData.AttackRadius;

        _movement = GetComponent<Movement>();
        if (_movement == null)
            Debug.LogError($"Failed to find {_movement.GetType()} in enemy - {gameObject.name}.");

        _shooter = GetComponent<Shooter>();
        if (_shooter == null)
            Debug.LogError($"Failed to find {_shooter.GetType()} in enemy - {gameObject.name}.");

        _target = FindObjectOfType<Player>()?.gameObject;
        if (_target == null)
            Debug.LogError($"Failed to find player in enemy - {gameObject.name}.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, EnemyData.ViewRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyData.AttackRadius);
    }

#endregion

}
