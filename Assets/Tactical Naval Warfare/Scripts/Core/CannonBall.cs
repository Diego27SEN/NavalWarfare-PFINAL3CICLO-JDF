using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Pool Configuration")]
    public string myPoolId = "CannonBall";
    public float lifeTime = 3.0f;

    private void OnEnable()
    {
        //Regresa al pool tras X segundos si no colisionó
        Invoke(nameof(ReturnToPool), lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Detecta si choco contra el agua o contra un barco
        if (other.CompareTag("Water") || other.CompareTag("Ship"))
        {
            // Elegimos que partícula pedir dependiendo de contra qué chocamos
            string particleId = other.CompareTag("Water") ? "WaterExplosion" : "WoodExplosion";

            if (PoolManager.Instance != null)
            {               
                PoolManager.Instance.GetObject(particleId, transform.position, Quaternion.identity);
            }

            ReturnToPool();
        }
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
