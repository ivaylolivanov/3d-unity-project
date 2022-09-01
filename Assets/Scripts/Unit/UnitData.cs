using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData/UnitData", order = 3)]
public class UnitData : ScriptableObject
{
    [Header("Unit data")]
    [SerializeField] public int InitialHealth;

    [Header("Rigidbody data")]
    [SerializeField] public bool  RbFreezeRotation;
    [SerializeField] public float RbMass;
    [SerializeField] public float RbDrag;

    [Space]
    [Header("Movement data")]
    [SerializeField] public float MovementSpeed;
    [SerializeField] public float RotationSmoothness;
    [Space]
    [SerializeField] public float FallSpeed;
    [SerializeField] public float JumpForce;
}
