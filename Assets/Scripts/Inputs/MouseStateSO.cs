using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
[CreateAssetMenu(
    fileName = "MouseState",
    menuName = "InputActions/MouseState",
    order = 1)]
public class MouseStateSO : ScriptableObject
{
    [SerializeField] private InputActionButtonSO _leftButton;
    [SerializeField] private InputActionButtonSO _middleButton;
    [SerializeField] private InputActionButtonSO _rightButton;

    private Camera _mainCamera;
    private InputActionButtonSO[] _mouseButtons;

    private void OnEnable()
    {
        _mainCamera = Camera.main;
        _mouseButtons = new InputActionButtonSO[3]
        {
            _leftButton,
            _middleButton,
            _rightButton
        };
    }

    private Vector2 _scroll;
    public Vector2 Scroll
    {
        get => _scroll;
        private set { }
    }

    private Vector3 _mousePosition = Vector3.zero;
    public Vector3 MousePosition
    {
        get => _mousePosition;
        private set { }
    }

    private Vector3 _mouseWorldPosition = Vector3.zero;
    public Vector3 MouseWorldPosition
    {
        get => _mouseWorldPosition;
        private set { }
    }

    public void Update()
    {
        foreach(var button in _mouseButtons)
            button.ReadInput();

        _scroll = Input.mouseScrollDelta;

        _mousePosition = Input.mousePosition;
        _mousePosition.z = 0;

        Ray mouseWorldRay = _mainCamera.ScreenPointToRay(_mousePosition);
        if(Physics.Raycast(mouseWorldRay, out RaycastHit raycastHit))
            _mouseWorldPosition = raycastHit.point;
    }
}
