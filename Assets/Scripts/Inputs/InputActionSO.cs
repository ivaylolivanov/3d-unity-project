using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
//[CreateAssetMenu(fileName = "InputAction", menuName = "InputAction/SpawnManagerScriptableObject", order = 1)]
public abstract class InputActionSO : ScriptableObject
{
    public abstract void ReadInput();

    public abstract bool IsDown();
    public abstract bool WasReleased();
    public abstract bool WasDown();

    public abstract float GetValue();
    public abstract float GetValueNormalized();
}
