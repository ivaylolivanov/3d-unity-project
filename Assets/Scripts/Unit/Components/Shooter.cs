using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootInterval = 0.2f;
    [SerializeField] private float _shootForce = 30f;

    // private fields
    private Rigidbody _rb;

    private ObjectsPools _objectsPools;
    private float _nextShootTime;

    #region MonoBehaviour

    void OnEnable() => Initialize();

    #endregion

    // Public methods
    public void Shoot()
    {
        if (Time.time < _nextShootTime)
            return;

        Bullet bullet = _objectsPools.GetBulletInstance(_shootPoint.position);
        if(bullet == null)
            return;

        bullet.SetCurrentOwner(gameObject);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if(bulletRigidbody == null)
            return;

        bulletRigidbody.AddForce(
            (_rb.rotation * Vector3.forward) * _shootForce,
            ForceMode.Impulse
        );

        _nextShootTime = Time.time + _shootInterval;
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");
        }

        _nextShootTime = 0;

        _objectsPools = FindObjectOfType<ObjectsPools>();
        if (_objectsPools == null)
            Debug.LogError($"Failed to get ObjectsPools in {gameObject.name}");
    }
}
