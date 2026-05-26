using UnityEngine;

public class targetShips : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [SerializeField] private float rotationSpeed = 2f;

    private int currentWaypoint = 0;

    private bool canMove = true;

    private void Update()
    {
        if (!canMove) return;

        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypoint];

        // Movimiento
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Rotación suave
        Vector3 direction = target.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Llegó al waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;

            // Terminó todos los puntos
            if (currentWaypoint >= waypoints.Length)
            {
                canMove = false;
            }
        }
    }

    public bool HasFinished()
    {
        return !canMove;
    }
}