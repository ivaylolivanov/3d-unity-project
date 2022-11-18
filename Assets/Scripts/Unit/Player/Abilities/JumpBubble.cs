using UnityEngine;

using Utils;

public class JumpBubble : MonoBehaviour
{
    [Header("Ability data")]
    [SerializeField] private float _riseSpeed;
    [SerializeField] private float _duration;
    [SerializeField] private float _cooldown;

    [Space]
    [Header("Bubble")]
    [Tooltip("Offset the positions of the bubble, so it will appear around the player")]
    [SerializeField] private Vector3 _bubblePositionOffset;

    // Private fields
    private PlayerData _data;
    private Rigidbody _rb;
    private Jump _jump;
    private Unit _caster;

    private float _nextBubbleAvailableTime;
    private float _endDurationTimePoint;

    private Bubble _visualsBubble;

#region MonoBehaviour methods

    private void OnEnable()
    {
        _nextBubbleAvailableTime = 0f;
        _endDurationTimePoint = 0f;
    }

    private void Update()
    {
        if (_data.InputActionJump.WasDown())
            _jump.DoJump();

        bool canActivateBubble = _data.InputActionJump.IsDown()
            && (_nextBubbleAvailableTime <= Time.time)
            && (_rb.velocity.y < 0f)
            && !_caster.IsInBubble;

        if (!canActivateBubble) return;

        _visualsBubble = ObjectsPools.GetInstance<Bubble>(
            transform.position);

        if (_visualsBubble == null)
            return;

        _caster.IsInBubble = true;
        _visualsBubble.Initialize(
            _duration,
            transform.up,
            () => { return _data.InputActionJump.WasReleased(); });

        _endDurationTimePoint = Time.time + _duration;
        _nextBubbleAvailableTime = _endDurationTimePoint + _cooldown;
    }

#endregion

#region Public methods

    public void Setup(PlayerData data, Rigidbody rb, Jump jump, Unit caster)
    {
        _data = data;
        _rb = rb;
        _jump = jump;
        _caster = caster;
    }

#endregion
}
