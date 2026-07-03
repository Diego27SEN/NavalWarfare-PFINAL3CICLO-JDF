using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;


public class CannonBall : BaseProjectile, IPoolable
{
    private float defaultDamage;

    private void Awake()
    {
        defaultDamage = currentDamage;
    }

    public override void OnImpact(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;

        // Crew primero
        if (obj.CompareTag("Crew"))
        {
            obj.transform.SetParent(null);

            // Solo agregar si no tiene ya uno
            Rigidbody crewRb = obj.GetComponent<Rigidbody>();
            if (crewRb == null)crewRb = obj.AddComponent<Rigidbody>();

            crewRb.linearVelocity = Vector3.zero;
            Vector3 impulso = (obj.transform.position - transform.position).normalized;
            impulso.y = 0.5f;
            crewRb.AddForce(impulso * 8f, ForceMode.Impulse);
            ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
            StartCoroutine(DelayedReturnToPool());
            return;
        }

        // Agua
        if (obj.CompareTag("Water"))
        {
            ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
            StartCoroutine(DelayedReturnToPool());
            return;
        }
        // Barco
        ShipHealth health = obj.GetComponentInParent<ShipHealth>();
        if (health != null)
        {
            health.TakeDamage(currentDamage);
            Debug.Log($"Impacto en barco. Daño infligido: {currentDamage}");
            ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
            StartCoroutine(DelayedReturnToPool());
        }
    }

    // Conectamos con nuestro método polimórfico
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Bala tocó: {other.collider.gameObject.name} | Tag: {other.collider.gameObject.tag}");
        OnImpact(other);
    }

    public void OnObjectSpawn()
    {
        // Restauramos el daño por si la ametralladora u otra arma lo había modificado
        currentDamage = defaultDamage;

        // Limpiamos la inercia física del disparo anterior
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Invoke(nameof(ReturnToPool), lifeTime);
    }

    // Implementamos el método de la interfaz
    public void OnObjectDespawn()
    {
        CancelInvoke(nameof(ReturnToPool));
    }

    private void OnEnable()
    {
        OnObjectSpawn();
    }

    private void OnDisable()
    {
        OnObjectDespawn();
    }

    
    private IEnumerator DelayedReturnToPool()
    {
        // Congela la cámara inmediatamente
        if (BulletCameraController.bulletCam != null)
            BulletCameraController.bulletCam.Target.TrackingTarget = null;

        yield return new WaitForSeconds(0.1f);
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
}
