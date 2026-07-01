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

    // POLIMORFISMO (Override)
    public override void OnImpact(Collision collision)
    {
        GameObject obj = collision.gameObject;

        // Si no es nada de lo que nos importa, ignoramos la colisión
        if (!obj.gameObject.CompareTag("Water") && !obj.gameObject.CompareTag("Ship") && !obj.gameObject.CompareTag("Crew")) return;

        // Unimos el Tag y el GetComponent 
        if (obj.CompareTag("Ship") && obj.TryGetComponent<ShipHealth>(out ShipHealth health))
        {
            health.TakeDamage(currentDamage);
            Debug.Log($"Impacto en barco. Daño infligido: {currentDamage}");
        }

        else if (obj.CompareTag("Crew"))
        {
            // Sacar de la jerarquía del barco
            obj.transform.SetParent(null);

            // Agregar Rigidbody en el momento del impacto
            Rigidbody crewRb = obj.AddComponent<Rigidbody>();
            crewRb.linearVelocity = Vector3.zero;

            Vector3 impulso = (obj.transform.position - transform.position).normalized;
            impulso.y = 0.5f;
            crewRb.AddForce(impulso * 8f, ForceMode.Impulse);
        }

        // Feedback visual y reciclaje (Esto se ejecuta para Agua, Barco y Tripulación)
        ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
        StartCoroutine(DelayedReturnToPool());
    }

    // Conectamos con nuestro método polimórfico
    private void OnCollisionEnter(Collision other)
    {
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
