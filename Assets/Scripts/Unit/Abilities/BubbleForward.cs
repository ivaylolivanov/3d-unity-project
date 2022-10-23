using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Bubble forward")]
public class BubbleForward : Ability
{
    [SerializeField] private Transform _visual;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxDuration;

    private Transform _visualInstance;

    private float _nextAvailableTime = 0f;

    public override void Reset()
    {
        _nextAvailableTime = 0f;
    }

    public override bool CanActivate()
    {
        bool result = false;

        if (Time.time <= _nextAvailableTime) return result;
        result = _data.Button.WasDown();

        return result;
    }

    public override void Activate(GameObject caster)
    {
        Transform visualInstance = Instantiate(
            _visual,
            caster.transform.position + caster.transform.up * 1.4f + caster.transform.forward * 2f,
            Quaternion.identity, caster.transform);

        Rigidbody rb = visualInstance.gameObject.AddComponent(
            typeof(Rigidbody))
            as Rigidbody;
        rb.useGravity = false;
        rb.freezeRotation = true;

        rb.AddForce(
            caster.transform.forward * _movementSpeed,
            ForceMode.Impulse);

        Bubble bubble = visualInstance.GetComponent<Bubble>();
        if (bubble != null)
        {
            bubble.OnTriggerEntered += CaptureTarget;
            bubble.OnTriggerExited += ReleaseTarget;
        }

        _nextAvailableTime = Time.time + _data.Cooldown;
    }

    private void CaptureTarget(GameObject target, GameObject bubble)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null) return;

        Rigidbody bubbleRb = bubble.GetComponent<Rigidbody>();
        if (bubbleRb == null) return;

        targetRb.velocity = bubbleRb.velocity;

        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit != null)
        {
            targetUnit.SetCurrentState(UnitState.InBubble);
        }
    }

    private void ReleaseTarget(GameObject target, GameObject bubble)
    {
        Unit targetUnit = target.GetComponent<Unit>();
        if (targetUnit != null)
        {
            targetUnit.SetCurrentState(UnitState.None);
        }
    }
}
