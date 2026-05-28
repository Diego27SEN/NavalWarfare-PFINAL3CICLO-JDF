using UnityEngine;
using UnityEngine.InputSystem;

public class CannonController : MonoBehaviour
{
    [Header("Referencias de la Torreta")]
    public Transform baseCannon;
    public Transform pivotCannon;

    [Header("Configuracion de Sensibilidad")]
    public float mouseSensitivity = 0.1f;

    [Header("Límites de Rotacion Vertical")]
    public float minVerticalAngle = -10f;
    public float maxVerticalAngle = 45f;

    private bool isCannonActive = false;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector2 currentLookInput = Vector2.zero;

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

        inputs.Player.ToggleMouse.performed += OnAimPerformed;
        inputs.Player.ToggleMouse.canceled += OnAimCanceled;
    }

    private void OnDisable()
    {
        inputs.Player.Look.performed -= OnLookPerformed;
        inputs.Player.Look.canceled -= OnLookCanceled;

        inputs.Player.ToggleMouse.performed -= OnAimPerformed;
        inputs.Player.ToggleMouse.canceled -= OnAimCanceled;

        inputs.Disable();
    }
    #endregion
    void Start()
    {
        UpdateMouseVisibility();
    }
    void Update()
    {
        HandleRotation();
    }
    #region Funcion de la Rotacion y la visibilidad del Mouse
    public void HandleRotation()
    {
        if (!isCannonActive) return;

        float mouseX = currentLookInput.x * mouseSensitivity;
        float mouseY = currentLookInput.y * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        if (baseCannon != null)
        {
            baseCannon.localRotation = Quaternion.Euler(0f, rotationY, 0f);
        }

        if (pivotCannon != null)
        {
            pivotCannon.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }
    public void UpdateMouseVisibility()
    {
        Cursor.lockState = isCannonActive ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isCannonActive;
    }
    #endregion

    #region Callback de Inputs
    private void OnLookPerformed(InputAction.CallbackContext ctx) => currentLookInput = ctx.ReadValue<Vector2>();
    private void OnLookCanceled(InputAction.CallbackContext ctx) => currentLookInput = Vector2.zero;

    private void OnAimPerformed(InputAction.CallbackContext ctx)
    {
        isCannonActive = true;
        UpdateMouseVisibility();
    }

    private void OnAimCanceled(InputAction.CallbackContext ctx)
    {
        isCannonActive = false;
        currentLookInput = Vector2.zero;
        UpdateMouseVisibility();
    }
    #endregion
}
