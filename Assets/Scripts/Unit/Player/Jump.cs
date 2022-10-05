using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    private PlayerData _data;
    private Rigidbody _rb;

#region MonoBehaviour methods

    private void OnEnable()    => Initialize();
    private void FixedUpdate() => AdjustFallingSpeed();

#endregion

#region Public methods

    public void DoJump()
    {
        if (!IsOnGround()) return;

        Vector3 velocity = _rb.velocity;
        velocity.y = _data.JumpForce;

        _rb.velocity = new Vector3(
            _rb.velocity.x,
            _data.JumpForce,
            _rb.velocity.z
        );
    }

    public void Setup(PlayerData data, Rigidbody rigidbody)
    {
        _data = data;
        _rb   = rigidbody;
    }

#endregion

#region Private methods

    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y >= -0.1f)
            return;

        _rb.velocity = Vector3.right * _rb.velocity.x
            + Vector3.down * _data.FallSpeed
            + Vector3.forward * _rb.velocity.z;
    }

    private bool IsOnGround()
    {
        bool result = Physics.CheckSphere(
            transform.position + _data.GroundCheckOffset,
            _data.GroundCheckRadius,
            _data.GroundCheckLayer
        );

        return result;
    }

    private void Initialize()
    {
        Player player = GetComponent<Player>();
        if (player == null)
            Debug.LogError($"Failed to get {player.GetType()} in {gameObject.name}");
        else
            _data = player.PlayerData;

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");
    }

    #endregion
}
