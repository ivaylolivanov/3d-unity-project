using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData/UnitData", order = 3)]
public class UnitData : ScriptableObject
{
    [Header("Rigidbody data")]
    [SerializeField] public bool  RbFreezeRotation;
    [SerializeField] public float RbMass;
    [SerializeField] public float RbDrag;

    [Space]
    [Header("Movement data")]
    [SerializeField] public float MovementSpeed = 8f;
    [SerializeField] public float RotationSmoothness = 10f;
    [Space]
    [SerializeField] public float FallSpeed = 10f;
    [SerializeField] public float JumpForce = 10f;
}
