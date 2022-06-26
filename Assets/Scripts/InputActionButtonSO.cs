using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
[CreateAssetMenu(
    fileName = "InputActionButton",
    menuName = "InputActions/Button",
    order = 1)]
public class InputActionButtonSO : InputActionSO
{
    [SerializeField] private KeyCode _key;

    private bool _isDown      = false;
    private bool _wasDown     = false;
    private bool _wasReleased = false;

    public override void ReadInput()
    {
        _isDown      = Input.GetKey(_key);
        _wasReleased = Input.GetKeyUp(_key);
        _wasDown     = Input.GetKeyDown(_key);
    }

    public override bool IsDown()      => _isDown;
    public override bool WasReleased() => _wasReleased;
    public override bool WasDown()     => _wasDown;

    public override float GetValue()           => _isDown ? 1f : 0f;
    public override float GetValueNormalized() => GetValue();
}
