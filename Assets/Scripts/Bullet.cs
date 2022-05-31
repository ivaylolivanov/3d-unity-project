using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _maxDistance;
    [SerializeField] float _maxTimeActive;

    private Vector3 _initialPoint;
    private float _activationTime;

    private ObjectsPools _objectsPools;
    private Rigidbody _rb;

    private void Awake()
    {
        _objectsPools = FindObjectOfType<ObjectsPools>();
        if(_objectsPools == null)
            Debug.LogError($"Failed to find ObjectsPools in {gameObject.name}");

        _rb = GetComponent<Rigidbody>();
        if(_rb == null)
            Debug.LogError($"Failed to find Rigidbody in {gameObject.name}");
    }

    private void OnEnable()
    {
        _initialPoint = transform.position;
        _activationTime = Time.time;

        _rb.velocity = Vector3.zero;
    }

    private void Update()
    {
        float distance = (transform.position - _initialPoint).sqrMagnitude;
        if(distance >= _maxDistance)
            Destroy();

        if((Time.time - _activationTime) >= _maxTimeActive)
            Destroy();
    }

    private void Destroy()
    {
        if(_objectsPools == null)
            return;

        _objectsPools.DisableInstance(this);
    }
}
