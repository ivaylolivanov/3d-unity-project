using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Bubble forward")]
public class BubbleForward : Ability
{
    [SerializeField] private Transform _visual;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxDuration;

    private Transform _visualInstance;

    private bool _isActive = false;
    private float _lastDeactivationTime = 0f;
    private float _endDurationTime = 0f;

    public override bool CanActivate()
    {
        bool result = false;

        // if (_visualInstance != null)
        //     _visualInstance.gameObject.SetActive(false);

        // The scriptable object saves data between playsessions. Therefore
        // this snippet reset it, when the spell is blocked because of that.
        if (_lastDeactivationTime >= Time.time)
            _lastDeactivationTime = 0f;

        if (_isActive && (_endDurationTime < Time.time))
        {
            _lastDeactivationTime = Time.time;
            if (_visualInstance != null)
                Destroy(_visualInstance.gameObject);
            _isActive = false;
            return result;
        }

        bool isOutOfCooldown = (
            Time.time >= (_lastDeactivationTime + _data.Cooldown));
        // Debug.Log($"Time: {Time.time}, Finish: {_endDurationTime}, Last deactivation: {_lastDeactivationTime}, CD: {_data.Cooldown}");

        result = isOutOfCooldown && _data.Button.WasDown();

        return result;
    }

    public override void Activate(GameObject caster)
    {
        if (!_isActive && _visualInstance == null)
        {
            _visualInstance = Instantiate(
                _visual,
                caster.transform.position + caster.transform.up * 1.4f + caster.transform.forward * 2f,
                Quaternion.identity, caster.transform);
            Rigidbody rb = _visualInstance.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.useGravity = false;
            rb.freezeRotation = true;
            rb.AddForce(caster.transform.forward * _movementSpeed, ForceMode.Impulse);
        }

        if (!_isActive) _endDurationTime = Time.time + _maxDuration;

        _isActive = true;

        if (_visualInstance == null) return;

        Bullet bubbleBullet = _visualInstance.GetComponent<Bullet>();
        if (bubbleBullet == null) return;

        bubbleBullet.Reset();
        bubbleBullet.Setup(100f, caster, caster.transform.forward, 0);
    }
}
