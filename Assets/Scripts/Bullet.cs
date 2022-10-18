using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _maxTimeActive;

    private int _damage = 10;

    private Vector3 _initialPoint;
    private float _activationTime;

    private Vector3 _direction = Vector3.zero;

    private Rigidbody _rb;

    private GameObject _currentOwner;

#region MonoBehaviour mehtods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if(_rb == null)
            Debug.LogError($"Failed to find Rigidbody in {gameObject.name}");

        _currentOwner = null;
    }

    private void OnEnable() => Reset();

    private void Update()
    {
        // Keep same speed and height
        if (_direction != Vector3.zero)
            _rb.velocity = _direction;

        if (!Helpers.IsInRange(transform.position, _initialPoint, _maxDistance))
            Destroy();

        if ((Time.time - _activationTime) >= _maxTimeActive)
            Destroy();
    }

#endregion

#region Public methods

    public void Reset()
    {
        _currentOwner = null;

        _rb.velocity = Vector3.zero;

        _initialPoint = transform.position;
        _activationTime = Time.time;

        _direction = Vector3.zero;
    }

    public void Setup(float maxDistance, GameObject owner, Vector3 direction,
                      int damage)
    {
        _maxDistance  = maxDistance;
        _currentOwner = owner;
        _direction    = direction;
        _damage       = damage;
    }

    public void SetMaxDistance(float maxDistance)
        => _maxDistance = maxDistance;
    public void SetCurrentOwner(GameObject newOwner)
        => _currentOwner = newOwner;
    public void SetDirection(Vector3 direction)
        => _direction = direction;

#endregion

#region Private methods

    private void OnCollisionEnter(Collision collision)
    {
        if (GameObject.ReferenceEquals(_currentOwner, collision.gameObject))
            return;

        Unit target = collision.gameObject.GetComponent<Unit>();
        if (target == null)
        {
            Destroy();
            return;
        }

        target.TakeDamage(_damage);

        // Push the target with an impulse
        // Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        // if (targetRigidbody != null)
        // {
        //     targetRigidbody.AddForce(_rb.velocity, ForceMode.Impulse);
        // }

        Destroy();
    }

    private void Destroy()
    {
        _currentOwner = null;
        ObjectsPools.DisableInstance(this);
    }

#endregion
}
