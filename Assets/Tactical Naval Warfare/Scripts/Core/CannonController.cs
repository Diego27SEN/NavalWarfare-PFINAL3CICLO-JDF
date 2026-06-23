using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CannonInputs))]
public class CannonController : MonoBehaviour
{
    [Header("Referencies de la Torreta")]
    public Transform baseCannon;
    public Transform pivotCannon;

    [Header("Configuracion de Sensibilidad")]
    public float mouseSensitivity = 0.1f;

    [Header("Limites de Rotacion Vertical")]
    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 20f;
    public bool invertVerticalAim = false;

    private bool isCannonActive = false;
    private Vector2 currentLookInput = Vector2.zero;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private Quaternion initialBaseRotation;
    private Quaternion initialPivotRotation;

    private CannonInputs inputsSystem;

    private void Awake()
    {
        inputsSystem = GetComponent<CannonInputs>();
        inputsSystem.enabled = false; // bloqueado por defecto
    }

    private void OnEnable()
    {
        if (inputsSystem != null)
        {
            inputsSystem.OnLook += UpdateLookInput;
            inputsSystem.OnAimStateChanged += UpdateAimState;
        }
    }

    private void OnDisable()
    {
        if (inputsSystem != null)
        {
            inputsSystem.OnLook -= UpdateLookInput;
            inputsSystem.OnAimStateChanged -= UpdateAimState;
        }
    }

    void Start()
    {
        if (baseCannon != null) initialBaseRotation = baseCannon.localRotation;
        if (pivotCannon != null) initialPivotRotation = pivotCannon.localRotation;
        UpdateMouseVisibility();
    }

    void Update()
    {
        HandleRotation();
    }

    #region Recibir Eventos
    private void UpdateLookInput(Vector2 lookData)
    {
        currentLookInput = lookData;
    }

    private void UpdateAimState(bool isActive)
    {
        isCannonActive = isActive;
        UpdateMouseVisibility();
    }
    #endregion

    #region Rotacion y Visbilidad
    public void HandleRotation()
    {
        if (!isCannonActive || baseCannon == null || pivotCannon == null) return;

        float mouseX = currentLookInput.x * mouseSensitivity;
        float mouseY = currentLookInput.y * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY * (invertVerticalAim ? -1f : 1f);

        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        baseCannon.localRotation = initialBaseRotation * Quaternion.Euler(0f, rotationY, 0f);
        pivotCannon.localRotation = initialPivotRotation * Quaternion.Euler(rotationX, 0f, 0f);
    }

    public void UpdateMouseVisibility()
    {
        Cursor.lockState = isCannonActive ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isCannonActive;
    }
    #endregion

    #region Control de Turno
    public void EnableCannon()
    {
        inputsSystem.enabled = true;
        isCannonActive = false; // no activa el modo apuntado automáticamente
        UpdateMouseVisibility(); // cursor libre para la UI
    }

    public void DisableCannon()
    {
        inputsSystem.enabled = false;
        isCannonActive = false;
        UpdateMouseVisibility();
    }
    #endregion
}
