using UnityEngine;

using Utils;

public class ShootBubbles : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _cooldown;

    [Space]
    [Header("Bubble")]
    [Tooltip("Offset the positions of the bubble, so it will appear around the player")]
    [SerializeField] private Vector3 _bubblePositionOffset;

    private PlayerData _data;
    private Rigidbody _rb;

    private Vector3 _shootPoint;

    private float _endCooldownTimePoint;

#region MonoBehaviour methods

    private void OnEnable()
    {
        _endCooldownTimePoint = 0f;
    }

    private void Update()
    {
        bool CanShootBubble = _data.InputActionShoot.WasDown()
            && Time.time >= _endCooldownTimePoint;
        if (!CanShootBubble) return;

        Vector3 bubblePosition = transform.TransformPoint(
            _shootPoint - _bubblePositionOffset);
        Bubble bubble = ObjectsPools.GetInstance<Bubble>(bubblePosition);
        bubble.Initialize(_duration, transform.forward);

        _endCooldownTimePoint = Time.time + _cooldown;
    }

#endregion

#region Public methods

    public void Setup(PlayerData data, Rigidbody rb, Vector3 shootPoint)
    {
        _rb = rb;
        _data = data;
        _shootPoint = shootPoint;
    }

#endregion
}
