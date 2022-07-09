using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionSO[] _inputActions;

    [SerializeField] private MouseStateSO _mouseState;

    private void Update()
    {
        foreach (var inputAction in _inputActions)
            inputAction.ReadInput();

        _mouseState.Update();
    }
}
