using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputSystem : MonoBehaviour
{
    private InputSystem_Actions inputs;

    public Action<Vector2> OnMove;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += MovePerformed;
        inputs.Player.Move.canceled += MoveCanceled;
    }

    private void OnDisable()
    {
        inputs.Player.Move.performed -= MovePerformed;
        inputs.Player.Move.canceled -= MoveCanceled;

        inputs.Disable();
    }

    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void MoveCanceled(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(Vector2.zero);
    }
    
}
