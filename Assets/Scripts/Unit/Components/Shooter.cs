using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;

    // private fields
    private UnitData _data;
    private Rigidbody _rb;

    private float _nextShootTime;

#region MonoBehaviour

    void OnEnable() => Initialize();

#endregion

#region Public methods

    public void Shoot()
    {
        if (Time.time < _nextShootTime) return;

        Bullet bullet = ObjectsPools.GetBulletInstance(_shootPoint.position);
        if (bullet == null) return;

        bullet.SetCurrentOwner(gameObject);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody == null) return;

        Vector3 bulletVelocity = new Vector3(
            (_rb.rotation * Vector3.forward).x * _data.AttackForce,
            bulletRigidbody.velocity.y,
            (_rb.rotation * Vector3.forward).z * _data.AttackForce
        );
        bullet.SetDirection(bulletVelocity);

        _nextShootTime = Time.time + _data.AttackInterval;
    }

    public void Setup(UnitData data, Rigidbody rigidbody)
    {
        _data = data;
        _rb = rigidbody;
    }

#endregion

#region Private methods

private void Initialize()
{
    _nextShootTime = 0;
}

#endregion
}
