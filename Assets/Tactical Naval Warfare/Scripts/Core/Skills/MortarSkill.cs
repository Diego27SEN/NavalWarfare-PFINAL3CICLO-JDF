using UnityEngine;

public class MortarSkill : BaseSkill, ITemporaryWeapon
{
    public float damageFixed = 60;

    [Header("Físicas del Mortero")]
    public float upwardForce = 200f; // Fuerza hacia arriba
    public float forwardForce = 150f; // Fuerza hacia adelante

    private ShootController cañonBase;

    protected override void ResetWeapon()
    {
        base.ResetWeapon();

        cañonBase = GetComponent<ShootController>();
        Debug.Log($"[Mortero] Instalado. Municion: {currentBullets} | Daño destructivo: {damageFixed}");
    }

    public override void FireShot()
    {
        if (currentBullets <= 0 || cañonBase == null) return;

        Debug.Log($"¡Booooom! Disparando Mortero. Balas restantes: {currentBullets - 1}");

        // Instanciar la bala
        GameObject bala = PoolManager.Instance?.GetObject(cañonBase.poolId, cañonBase.firePoint.position, cañonBase.firePoint.rotation);
        if (bala == null) return;

        // Asignamos el daño especial
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

        // Restar munición
        currentBullets--;

        // Si nos quedamos sin balas, llamamos al método DisableWeapon() que heredamos del padre
        if (currentBullets <= 0)
        {
            DisableWeapon();
        }
    }
}
