using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [Header("Base Settings")]
    public string myPoolId;
    public float lifeTime = 3.0f;
    public float currentDamage = 20.0f;

    public abstract void OnImpact(Collision collision);

    // Resetear el proyectil 
    public virtual void ResetProjectile()
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
