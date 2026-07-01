using UnityEngine;
using System.Collections;

public class GatlingGunSkill : BaseSkill
{
    public float damageFixed = 2f;
    public float fireRate = 0.2f;
    public int balasPorRafaga = 4;
    public float impulseForce = 150f;

    private bool isShooting = false;
    private ShootController cañonBase;
    protected override void ResetWeapon()
    {
        base.ResetWeapon();

        cañonBase = GetComponent<ShootController>();
        Debug.Log("[Gatling Gun] Ametralladora lista para desatar el caos.");
    }

    public override void FireShot()
    {
        // Si no hay munición, no hay cañón, o ya está disparando, ignoramos
        if (currentBullets <= 0 || cañonBase == null || isShooting) return;

        Debug.Log($"¡Ratatata! Disparando ráfaga. Ráfagas restantes: {currentBullets - 1}");

        // Iniciamos la ráfaga
        StartCoroutine(MachineGunBurst());
    }
    private IEnumerator MachineGunBurst()
    {
        isShooting = true;

        for (int i = 0; i < balasPorRafaga; i++)
        {
            GameObject bala = PoolManager.Instance?.GetObject(cañonBase.poolId, cañonBase.firePoint.position, cañonBase.firePoint.rotation);

            // Si no hay bala en el Pool saltamos.
            if (bala == null) continue;

            // Asignamos el daño de la Gatling en una sola línea
            if (bala.TryGetComponent<CannonBall>(out CannonBall scriptBala))
            {
                scriptBala.currentDamage = damageFixed;
            }

            // Aplicamos las físicas y la dispersión directamente
            if (bala.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Calculamos la dispersión 
                Vector3 dispersion = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0);
                Vector3 direccionDisparo = (cañonBase.firePoint.forward + dispersion).normalized;

                rb.AddForce(direccionDisparo * impulseForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(fireRate);
        }

       // Restar munición (1 ráfaga completa consume 1 de munición)
        currentBullets--;
        isShooting = false;

        // Si nos quedamos sin balas, llamamos al método DisableWeapon() del padre
        if (currentBullets <= 0) 
        {
            DisableWeapon();
        }
    }
}
