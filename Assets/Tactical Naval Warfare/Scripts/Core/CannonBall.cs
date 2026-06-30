using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;


public class CannonBall : MonoBehaviour
{
    [Header("Pool Configuration")]
    public string myPoolId = "CannonBall";
    public float lifeTime = 3.0f;

    [Header("Damage Settings")]
    public float currentDamage = 20.00f; 

    [Header("Particles")]
    public GameObject woodExplosionVFX;
    public GameObject waterExplosionVFX;


    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }
    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;

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

        // 4. Feedback visual y reciclaje (Esto se ejecuta para Agua, Barco y Tripulación)
        ImpactFeedBackManager.Instance?.PlayImpact(transform.position);
        StartCoroutine(DelayedReturnToPool());
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
    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }
}
