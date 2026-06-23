using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CannonInputs : MonoBehaviour
{
    public event Action<Vector2> OnLook;
    public event Action<bool> OnAimStateChanged;

    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }

    #region InputsActions
    private void OnEnable()
    {
        inputs.Enable();
        inputs.Player.Look.performed += OnLookPerformed;
        inputs.Player.Look.canceled += OnLookCanceled;
        inputs.Player.ToggleMouse.started += OnAimPerformed;   // al presionar
        inputs.Player.ToggleMouse.canceled += OnAimCanceled;   // al soltar
    }

    private void OnDisable()
    {
        inputs.Player.Look.performed -= OnLookPerformed;
        inputs.Player.Look.canceled -= OnLookCanceled;
        inputs.Player.ToggleMouse.started -= OnAimPerformed;
        inputs.Player.ToggleMouse.canceled -= OnAimCanceled;
        inputs.Disable();
    }
    #endregion

    #region Callback de Inputs
    private void OnLookPerformed(InputAction.CallbackContext ctx) => OnLook?.Invoke(ctx.ReadValue<Vector2>());

    private void OnLookCanceled(InputAction.CallbackContext ctx) => OnLook?.Invoke(Vector2.zero);

    private void OnAimPerformed(InputAction.CallbackContext ctx) => OnAimStateChanged?.Invoke(true);

    private void OnAimCanceled(InputAction.CallbackContext ctx) => OnAimStateChanged?.Invoke(false);
    #endregion
}
