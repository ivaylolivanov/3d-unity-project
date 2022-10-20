using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Bubble to the air")]
public class BubbleToTheAir : Ability
{
    [SerializeField] private Transform _visual;
    [SerializeField] private float _erectionSpeed;
    [SerializeField] private float _maxDuration;

    private Transform _visualInstance;

    private bool _isActive = false;
    private float _lastDeactivationTime = 0f;
    private float _endDurationTime = 0f;

    public override void Reset()
    {
        _isActive = false;
        _lastDeactivationTime = 0f;
        _endDurationTime = 0f;
    }

    public override bool CanActivate()
    {
        bool result = false;

        if (_visualInstance != null)
            _visualInstance.gameObject.SetActive(false);

        // The scriptable object saves data between playsessions. Therefore
        // this snippet reset it, when the spell is blocked because of that.
        if (_lastDeactivationTime >= Time.time)
            _lastDeactivationTime = 0f;

        if ((_data.Button.WasReleased() && _isActive) || (_isActive && (_endDurationTime < Time.time)))
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

    public override void Activate(GameObject caster)
    {
        Rigidbody casterRb = caster.GetComponent<Rigidbody>();
        if (casterRb == null) return;

        if (!_isActive && _visualInstance == null)
        {
            _visualInstance = Instantiate(
                _visual,
                caster.transform.position + new Vector3(0, 1.4f, 0),
                Quaternion.identity, caster.transform);

            Bubble bubble = _visualInstance.GetComponent<Bubble>();
            if (bubble != null)
                bubble.OnTriggerEntered += BubbleTriggerEntered;
        }
        else if (_visualInstance.gameObject.activeSelf == false)
        {
            _visualInstance.gameObject.SetActive(true);
        }

        if (!_isActive) _endDurationTime = Time.time + _maxDuration;

        _isActive = true;

        Vector3 casterVelocity = casterRb.velocity;
        casterVelocity = new Vector3(0f, casterVelocity.y + _erectionSpeed, 0f);
        casterRb.velocity = casterVelocity;
    }

    private void BubbleTriggerEntered(GameObject other, GameObject bubble)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet == null) return;

        _visualInstance.gameObject.SetActive(false);
        _isActive = false;
        _lastDeactivationTime = Time.time;

        ObjectsPools.DisableInstance(bullet);
    }
}
