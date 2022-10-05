using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData UnitData => _unitData;

    public Animator Animator { get; private set; }

    [SerializeField] protected UnitData _unitData;

    protected Rigidbody _rb;

    protected HealthUI _healthUI;

    protected int _health    = 0;
    protected int _maxHealth = 0;

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
        _health    = _unitData.InitialHealth;
        _maxHealth = _unitData.InitialHealth;

        _healthUI = GetComponentInChildren<HealthUI>();
        Animator  = GetComponent<Animator>();
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
