using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Pool Configuration")]
    public string myPoolId = "CannonBall";
    public float lifeTime = 3.0f;

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Water") && !other.CompareTag("Ship")) return;

        // Efecto visual
        string particleId = other.CompareTag("Water") ? "WaterExplosion" : "WoodExplosion";
        PoolManager.Instance?.GetObject(particleId, transform.position, Quaternion.identity);

        // 3. Sistema de Daño
        if (other.CompareTag("Ship"))
        {
            if (other.TryGetComponent<ShipHealth>(out ShipHealth health))
            {
                health.TakeDamage(20.00f);
            }
        }

        ReturnToPool();

    }
    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        if (PoolManager.Instance != null && this.gameObject.activeInHierarchy)
        {
            PoolManager.Instance.ReturnObject(myPoolId, this.gameObject);
        }
    }
    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }
}
