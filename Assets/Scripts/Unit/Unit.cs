using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected UnitData _unitData;

    public UnitData UnitData => _unitData;

    protected Health _health;

    protected virtual void OnEnable()
    {
        _health = GetComponentInChildren<Health>();
    }

    public void TakeDamage(int damage) => _health.TakeDamage(damage);
    public void Heal(int amount)       => _health.Heal(amount);
}
