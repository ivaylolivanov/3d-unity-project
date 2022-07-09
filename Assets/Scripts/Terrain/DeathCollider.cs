using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    [SerializeField] private int _instantDeathDamage = 99999;

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.transform.GetComponent<Health>();
        if(health == null)
            return;

        health.TakeDamage(_instantDeathDamage);
        Debug.Break();
    }
}
