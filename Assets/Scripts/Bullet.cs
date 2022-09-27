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

    private ObjectsPools _objectsPools;
    private Rigidbody _rb;

    private GameObject _currentOwner;

#region MonoBehaviour mehtods

    private void Awake()
    {
        _objectsPools = FindObjectOfType<ObjectsPools>();
        if(_objectsPools == null)
            Debug.LogError($"Failed to find ObjectsPools in {gameObject.name}");

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
        Destroy();
    }

    private void Destroy()
    {
        if (_objectsPools == null)
        {
            Destroy(this.gameObject);
            return;
        }

        _currentOwner = null;
        _objectsPools.DisableInstance(this);
    }

#endregion
}
