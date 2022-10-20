using UnityEngine;

[System.Serializable]
public struct AbilityData
{
    public string Name;
    public int Cooldown;
    public InputActionButtonSO Button;
}

[System.Serializable]
public class Ability : ScriptableObject
{
    [SerializeField] protected AbilityData _data;

    public virtual void Reset() { }

    public virtual bool CanActivate() => _data.Button.WasDown();

    public virtual void Activate(GameObject caster) { }
}
