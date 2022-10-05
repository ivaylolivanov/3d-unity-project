using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : Unit
{
    [SerializeField]
    public PlayerData PlayerData => (PlayerData)_unitData;

    // Private fields
    private Movement _movement;
    private Jump _jump;
    private Shooter _shooter;

    // Unity components
    private Rigidbody _rb;

#region MonoBehaviour methods

    protected override void OnEnable()
    {
        base.OnEnable();

        Initialize();
        Setup();
    }

    private void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;

        _movement.HandleRotation(fixedDeltaTime, _rb.velocity);

        _movement.Move(
            PlayerData.InputAxisHorizontal.GetValueNormalized(),
            PlayerData.InputAxisVertical.GetValueNormalized()
        );

        if(PlayerData.InputActionJump.WasDown())
            _jump.DoJump();

        if (PlayerData.InputActionShoot.IsDown())
        {
            RotateToMouse(fixedDeltaTime);
            _shooter.Shoot();
        }
    }

#endregion

#region Private methods

    private void Initialize()
    {
        _movement = GetComponent<Movement>();
        if(_movement == null)
            Debug.LogError($"Failed to get movement in {gameObject.name}");

        _jump = GetComponent<Jump>();
        if(_jump == null)
            Debug.LogError($"Failed to get {_jump.GetType()} in {gameObject.name}");

        _shooter = GetComponent<Shooter>();
        if(_shooter == null)
            Debug.LogError($"Failed to get shooter in {gameObject.name}");

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get rigidbody in {gameObject.name}");
    }

    private void Setup()
    {
        if (_movement != null) _movement.Setup(_unitData,         _rb);
        if (_jump     != null) _jump.Setup((PlayerData)_unitData, _rb);
        if (_shooter  != null) _shooter.Setup(_unitData,          _rb);
    }

    private void RotateToMouse(float fixedDeltaTime)
    {
        Vector3 playerToMouseDirection =
            PlayerData.MouseState.MouseWorldPosition
            - _rb.position;

        _movement.HandleRotation(fixedDeltaTime, playerToMouseDirection, false);
    }

#endregion
}
