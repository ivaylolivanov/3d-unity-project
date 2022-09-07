using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    [SerializeField] private int _instantDeathDamage = 99999;

    private void OnCollisionEnter(Collision collision)
    {
        Unit target = collision.transform.GetComponent<Unit>();
        if(target == null)
            return;

        target.TakeDamage(_instantDeathDamage);
        Debug.Break();
    }
}
