using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("Pool Configuration")]
    public string myPoolId = "CannonBall";

    private void OnTriggerEnter(Collider other)
    {
        // Detecta si chocó contra el agua o contra un barco
        if (other.CompareTag("Water") || other.CompareTag("Ship"))
        {
            // Elegimos qué partícula pedir dependiendo de contra qué chocamos
            string particleId = other.CompareTag("Water") ? "WaterExplosion" : "WoodExplosion";

            if (PoolManager.Instance != null)
            {               
                PoolManager.Instance.GetObject(particleId, transform.position, Quaternion.identity);
                PoolManager.Instance.ReturnObject(myPoolId, this.gameObject);
            }
            else
            {
                Debug.LogWarning("CannonBall de PoolManager no se ha encontrado en la escena.");
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (PoolManager.Instance != null && this.gameObject.activeInHierarchy)
        {
            PoolManager.Instance.ReturnObject(myPoolId, this.gameObject);
        }
    }
}
