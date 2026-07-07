using UnityEngine;

public class MortarSkill : BaseSkill, ITemporaryWeapon
{
    public float damageFixed = 60;

    [Header("Fisicas del Mortero")]
    public float upwardForce = 200f; // Fuerza hacia arriba
    public float forwardForce = 150f; // Fuerza hacia adelante

    private ShootController canonBase; // Cambiado a 'canonBase'

    protected override void ResetWeapon()
    {
        base.ResetWeapon();

        canonBase = GetComponent<ShootController>();
        Debug.Log($"[Mortero] Instalado. Municion: {currentBullets} | Dano destructivo: {damageFixed}");
    }

    public override void FireShot()
    {
        if (currentBullets <= 0 || canonBase == null) return;

        Debug.Log($"Boom! Disparando Mortero. Balas restantes: {currentBullets - 1}");

        // Instanciar la bala
        GameObject bala = PoolManager.Instance?.GetObject(canonBase.poolId, canonBase.firePoint.position, canonBase.firePoint.rotation);
        if (bala == null) return;

        // Asignamos el dano especial
        if (bala.TryGetComponent<CannonBall>(out CannonBall scriptBala))
        {
            scriptBala.currentDamage = damageFixed;
        }

        // Aplicamos las fisicas de forma directa
        if (bala.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 direccionMortero = (canonBase.firePoint.forward * forwardForce) + (Vector3.up * upwardForce);
            rb.AddForce(direccionMortero, ForceMode.Impulse);
        }

        // Restar municion
        currentBullets--;

        // Si nos quedamos sin balas, llamamos al metodo DisableWeapon() que heredamos del padre
        if (currentBullets <= 0)
        {
            DisableWeapon();
        }
    }
}