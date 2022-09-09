using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected UnitData _unitData;

    public UnitData UnitData => _unitData;

    protected HealthUI _healthUI;

    protected int _health = 0;
    protected int _maxHealth = 0;

    protected virtual void OnEnable()
    {
        _health    = _unitData.InitialHealth;
        _maxHealth = _unitData.InitialHealth;

        _healthUI = GetComponentInChildren<HealthUI>();

        if (_healthUI == null)
        {
            Debug.LogError($"Failed to get {_healthUI.GetType()} in {gameObject.name}");
        }
        else
        {
            _healthUI.UpdateCurrentValue(_health);
            _healthUI.UpdateMaxValue(_maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if(damage <= 0)
            return;

        _health -= damage;

        _healthUI.UpdateCurrentValue(_health);

        if(_health <= 0) Die();
    }

    public void Heal(int amount)
    {
        if(amount <= 0)
            return;

        _health += amount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);

        _healthUI.UpdateCurrentValue(_health);
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
