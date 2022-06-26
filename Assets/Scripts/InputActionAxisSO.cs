using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
[CreateAssetMenu(
    fileName = "InputActionButton",
    menuName = "InputActions/Axis")]
public class InputActionAxisSO : InputActionSO
{
    [SerializeField] private string _axis;

    private float _axisValue = 0;

    private bool _isDown      = false;
    private bool _wasDown     = false;
    private bool _wasReleased = false;

    public override void ReadInput()
    {
        _previousAxisValue = _axisValue;
        _axisValue = Input.GetAxis(_axis);

        bool axisIsZero = _axisValue == 0f;

        _isDown      = ! axisIsZero;
        _wasReleased = _wasDown && axisIsZero;
        _wasDown     = ! axisIsZero;
    }

    public override bool IsDown()      => _isDown;
    public override bool WasReleased() => _wasReleased;
    public override bool WasDown()     => _wasDown;

    public override float GetValue()           => _axisValue;

    private float _previousAxisValue;
    public override float GetValueNormalized()
    {
        float result = 0f;

        if(_axisValue < _previousAxisValue && (_axisValue > 0))
            return 0f;

        if(_axisValue >= _previousAxisValue && (_axisValue > 0))
            return 1f;

        if(_axisValue > _previousAxisValue && (_axisValue < 0))
            return 0f;

        if(_axisValue <= _previousAxisValue && (_axisValue < 0))
            return -1f;

        return result;
    }
}
