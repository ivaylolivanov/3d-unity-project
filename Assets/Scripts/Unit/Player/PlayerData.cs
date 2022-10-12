using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "PlayerData", menuName = "UnitData/PlayerData", order = 2)]
public class PlayerData : UnitData
{
    [Header("Controls")]
    [SerializeField] public MouseStateSO MouseState;
    [Space]
    [SerializeField] public InputActionSO InputAxisHorizontal;
    [SerializeField] public InputActionSO InputAxisVertical;
    [SerializeField] public InputActionSO InputActionJump;
    [SerializeField] public InputActionSO InputActionShoot;
    [Space]
    [Header("Jump")]
    [SerializeField] public int GroundCheckLayer;
    [SerializeField] public Vector3 GroundCheckOffset;
    [SerializeField] public float GroundCheckRadius;
    [Space]
    [Header("Abilities")]
    [SerializeField] public Ability[] Abilities;
}
