using UnityEngine;

public class MortarSkill : MonoBehaviour, ITemporaryWeapon
{
    public float damageFixed = 60;
    public int maxBullets = 3;
    public int currentBullets;

    [Header("Físicas del Mortero")]
    public float upwardForce = 200f; // Fuerza hacia arriba
    public float forwardForce = 150f; // Fuerza hacia adelante

    private ShootController cañonBase;
    void Start()
    {
        currentBullets = maxBullets;
        cañonBase = GetComponent<ShootController>();
        Debug.Log($"[Mortero] Instalado. Municion: {currentBullets} | Daño destructivo: {damageFixed}");
    }

    public void DisableWeapon()
    {
        Debug.Log("[Mortero] Desmontado.");
        Destroy(this);
    }
    public void FireShot()
    {
        // Guardado
        if (currentBullets <= 0 || cañonBase == null) return;

        Debug.Log($"¡Booooom! Disparando Mortero. Balas restantes: {currentBullets - 1}");

        // Instanciar la bala
        GameObject bala = PoolManager.Instance?.GetObject(cañonBase.poolId, cañonBase.firePoint.position, cañonBase.firePoint.rotation);
        if (bala == null) return;

        // Asignamos el daño especial solo si encontramos el componente
        if (bala.TryGetComponent<CannonBall>(out CannonBall scriptBala))
        {
            scriptBala.currentDamage = damageFixed;
        }

        // Aplicamos las físicas de forma directa
        if (bala.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 direccionMortero = (cañonBase.firePoint.forward * forwardForce) + (Vector3.up * upwardForce);
            rb.AddForce(direccionMortero, ForceMode.Impulse);
        }

        //Restar munición y comprobar autodestrucción
        currentBullets--;
        if (currentBullets <= 0) DisableWeapon();
    }
}
