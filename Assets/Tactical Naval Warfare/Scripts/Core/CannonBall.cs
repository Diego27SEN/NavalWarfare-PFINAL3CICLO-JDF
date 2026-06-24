using UnityEngine;
using MoreMountains.Feedbacks;


public class CannonBall : MonoBehaviour
{

    [Header("Pool Configuration")]
    public string myPoolId = "CannonBall";
    public float lifeTime = 3.0f;

    [Header("Particles")]
    public GameObject woodExplosionVFX;
    public GameObject waterExplosionVFX;


    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Water") && !other.gameObject.CompareTag("Ship") && !other.gameObject.CompareTag("Crew")) return;

       // string particleId = other.gameObject.CompareTag("Water") ? "WaterExplosion" : "WoodExplosion";
       // PoolManager.Instance?.GetObject(particleId, transform.position, Quaternion.identity);

        if (other.gameObject.CompareTag("Ship"))
        {
            if (other.gameObject.TryGetComponent<ShipHealth>(out ShipHealth health))health.TakeDamage(20.00f);
        }

        if (other.gameObject.CompareTag("Crew"))
        {
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 impulso = (other.transform.position - transform.position).normalized;
                impulso.y = 0.5f;
                rb.AddForce(impulso * 8f, ForceMode.Impulse);
            }
        }
        Debug.Log("Impacto - reproduciendo feedback");
        ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
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
