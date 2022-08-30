using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected UnitData _unitData;

    public UnitData UnitData => _unitData;
}
