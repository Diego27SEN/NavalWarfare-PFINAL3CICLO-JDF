using UnityEngine;

public class CinematicShips : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform targetPoint;

    [SerializeField] private float speed = 5f;

    [SerializeField] private float rotationSpeed = 2f;

    private bool canMove = true;

    private void Update()
    {
        if (!canMove) return;

        MoveToPosition();
    }

    private void MoveToPosition()
    {
        // Movimiento
        transform.position = Vector3.MoveTowards(transform.position,targetPoint.position,speed * Time.deltaTime);

        Quaternion targetRotation =Quaternion.LookRotation(targetPoint.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);

        // Llegó al destino
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            transform.position = targetPoint.position;

            canMove = false;
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }
}