using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float maxMoveDistance = 10f;

    [Header("Turn System")]
    public bool InTurn = false;

    private Vector3 moveDirection;

    private void Update()
    {
        
        if (!InTurn) return;  // Solo se mueve en su turno

        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;         // Rotación

        if (moveDirection != Vector3.zero)        // Rotación
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,rotationSpeed * Time.deltaTime);
        }
    }
    public void StartTurn()
    {
        InTurn = true;

        Debug.Log(name + " START TURN");
    }

    public void EndTurn()
    {
        InTurn = false;

        Debug.Log(name + " END TURN");
    }

}