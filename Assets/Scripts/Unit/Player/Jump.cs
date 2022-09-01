using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rb;

#region MonoBehaviour methods

    private void OnEnable()
    {
        _player = GetComponent<Player>();
        if (_player == null)
            Debug.LogError($"Failed to get {_player.GetType()} in {gameObject.name}");

        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
            Debug.LogError($"Failed to get {_rb.GetType()} in {gameObject.name}");
    }

    private void FixedUpdate() => AdjustFallingSpeed();

#endregion

#region Public methods

    public void DoJump()
    {
        if (!IsOnGround()) return;

        Vector3 velocity = _rb.velocity;
        velocity.y = _player.UnitData.JumpForce;

        _rb.velocity = new Vector3(
            _rb.velocity.x,
            _player.UnitData.JumpForce,
            _rb.velocity.z
        );
    }

#endregion

#region Private methods

    private void AdjustFallingSpeed()
    {
        if (_rb.velocity.y >= -0.1f)
            return;

        _rb.velocity = Vector3.right * _rb.velocity.x
            + Vector3.down * _player.PlayerData.FallSpeed
            + Vector3.forward * _rb.velocity.z;
    }

    private bool IsOnGround()
    {
        bool result = Physics.CheckSphere(
            transform.position + _player.PlayerData.GroundCheckOffset,
            _player.PlayerData.GroundCheckRadius,
            _player.PlayerData.GroundCheckLayer
        );

        return result;
    }

#endregion
}
