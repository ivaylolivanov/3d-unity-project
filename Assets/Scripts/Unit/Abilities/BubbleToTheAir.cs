using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Buble to the air")]
public class BubbleToTheAir : Ability
{
    public override bool CanActivate()
    {
        bool result = false;

        if (_data.Button.WasReleased() && _isActive)
        {
            _lastDeactivationTime = Time.time;
            _isActive = false;
            return result;
        }

        bool isOutOfCooldown = (
            Time.time >= (_lastDeactivationTime + _data.Cooldown));

        result = isOutOfCooldown && _data.Button.IsDown();

        return result;
    }

    private bool _isActive = false;
    private float _lastDeactivationTime = 0f;

    public override void Activate(GameObject caster)
    {
        _isActive = true;

        Rigidbody casterRb = caster.GetComponent<Rigidbody>();
        if (casterRb == null) return;

        Vector3 casterVelocity = casterRb.velocity;
        casterVelocity.y += 1f;
        casterRb.velocity = casterVelocity;
    }
}
