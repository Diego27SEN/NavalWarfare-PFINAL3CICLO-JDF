using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;


public class CannonBall : BaseProjectile, IPoolable
{
    private float defaultDamage;

    [Header("Feedbacks de Impacto")]
    public MMFeedbacks onShipImpact;  // Para el barco (Partículas, shake, freeze)
    public MMFeedbacks onWaterImpact; // Para el agua
    public MMFeedbacks onCrewImpact;  // Para la tripulación
    private void Awake()
    {
        defaultDamage = currentDamage;
    }

    public override void OnImpact(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;
        onShipImpact?.PlayFeedbacks();
        // Control de seguridad físico
        if (collision.contacts.Length == 0) return;

        // Capturamos el punto exacto del impacto ANTES de que la bala rebote
        Vector3 hitPoint = collision.contacts[0].point;
        Vector3 hitNormal = collision.contacts[0].normal;

      
        // De esta forma la cámara se congela en su sitio y graba el impacto de forma estable
        if (BulletCameraController.bulletCam != null)
        {
            BulletCameraController.bulletCam.Target.TrackingTarget = null;
        }

        if (collision.gameObject.CompareTag("Crew") || obj.CompareTag("Crew"))
        {
            obj.transform.SetParent(null);

            Rigidbody crewRb = obj.GetComponent<Rigidbody>();
            if (crewRb == null) crewRb = obj.AddComponent<Rigidbody>();

            crewRb.linearVelocity = Vector3.zero;
            Vector3 impulso = (obj.transform.position - transform.position).normalized;
            impulso.y = 3f;
            crewRb.AddForce(impulso * 12f, ForceMode.Impulse);

            // Activamos las partículas en el punto exacto congelado
            ActivateFeedback("CrewImpactsFeel", hitPoint, hitNormal);

            StartCoroutine(DelayedReturnToPool());
            return;
        }

        // 2. IMPACTO CON AGUA
        if (collision.gameObject.CompareTag("Water") || obj.CompareTag("Water"))
        {
            ActivateFeedback("WaterImpactsFeel", hitPoint, Vector3.up);

            StartCoroutine(DelayedReturnToPool());
            return;
        }

        // 3. IMPACTO CON BARCO
        ShipHealth health = obj.GetComponentInParent<ShipHealth>();
        if (health != null)
        {
            health.TakeDamage(currentDamage);
            Debug.Log($"Impacto en barco. Daño infligido: {currentDamage}");

            ActivateFeedback("ShipImpactsFeel", hitPoint, hitNormal);

            StartCoroutine(DelayedReturnToPool());
        }
    }


  
    private void ActivateFeedback(string feedbackObjectName, Vector3 position, Vector3 normal)
    {
        // Buscamos usando tus nombres exactos de la jerarquía
        GameObject feelObject = GameObject.Find(feedbackObjectName);
        if (feelObject != null)
        {
            feelObject.transform.position = position;
            feelObject.transform.forward = normal;

            if (feelObject.TryGetComponent<MMF_Player>(out MMF_Player feedbacks))
            {
                feedbacks.PlayFeedbacks();
            }
            // O si usas la versión anterior:
            else if (feelObject.TryGetComponent<MMFeedbacks>(out MMFeedbacks oldFeedbacks))
            {
                oldFeedbacks.PlayFeedbacks();
            }
        }
        else
        {
            Debug.LogWarning($"No se encontró el objeto de feedback: {feedbackObjectName}");
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
