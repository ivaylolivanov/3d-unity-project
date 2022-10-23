using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData UnitData => _unitData;

    [SerializeField] protected UnitData _unitData;

    protected Rigidbody _rb;
    protected Animator  _animator;

    protected HealthUI _healthUI;

    protected int _health    = 0;
    protected int _maxHealth = 0;

    protected UnitState _currentState = UnitState.None;

#region MonoBehaviour

    protected virtual void OnEnable()
    {
        Initialize();
        Setup();
    }

#endregion

#region Public methods

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

    public void SetCurrentState(UnitState state)
        => _currentState = state;

#endregion

#region Protected methods

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

#endregion

#region Private methods

    private void Initialize()
    {
        _currentState = UnitState.None;

        _health    = _unitData.InitialHealth;
        _maxHealth = _unitData.InitialHealth;

        _healthUI = GetComponentInChildren<HealthUI>();
        _animator = GetComponent<Animator>();
        _rb       = GetComponent<Rigidbody>();

        if (_healthUI == null)
            Debug.LogError($"Failed to get {_healthUI.GetType()} in {gameObject.name}");

        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");
    }

    private void Setup()
    {
        if (_healthUI != null)
        {
            _healthUI.UpdateCurrentValue(_health);
            _healthUI.UpdateMaxValue(_maxHealth);
        }

        if (_rb != null)
        {
            _rb.freezeRotation = _unitData.RbFreezeRotation;
            _rb.mass           = _unitData.RbMass;
            _rb.drag           = _unitData.RbDrag;
        }
    }

#endregion
}
