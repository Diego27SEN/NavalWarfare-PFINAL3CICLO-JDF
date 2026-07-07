using UnityEngine;
using System.Collections;

public class GatlingGunSkill : BaseSkill
{
    public float damageFixed = 2f;
    public float fireRate = 0.2f;
    public int balasPorRafaga = 4;
    public float impulseForce = 150f;

    private bool isShooting = false;
    private ShootController canonBase; // Cambiado de cañonBase a canonBase

    protected override void ResetWeapon()
    {
        base.ResetWeapon();

        canonBase = GetComponent<ShootController>();
        Debug.Log("[Gatling Gun] Ametralladora lista para desatar el caos.");
    }

    public override void FireShot()
    {
        // Si no hay municion, no hay canon, o ya esta disparando, ignoramos
        if (currentBullets <= 0 || canonBase == null || isShooting) return;

        Debug.Log($"Ratatata! Disparando rafaga. Rafagas restantes: {currentBullets - 1}");

        // Iniciamos la rafaga
        StartCoroutine(MachineGunBurst());
    }

    private IEnumerator MachineGunBurst()
    {
        isShooting = true;

        for (int i = 0; i < balasPorRafaga; i++)
        {
            GameObject bala = PoolManager.Instance?.GetObject(canonBase.poolId, canonBase.firePoint.position, canonBase.firePoint.rotation);

            // Si no hay bala en el Pool saltamos.
            if (bala == null) continue;

            // Asignamos el dano de la Gatling
            if (bala.TryGetComponent<CannonBall>(out CannonBall scriptBala))
            {
                scriptBala.currentDamage = damageFixed;
            }

            // Aplicamos las fisicas y la dispersion directamente
            if (bala.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Calculamos la dispersion 
                Vector3 dispersion = new Vector3(Random.Range(-0.02f, 0.02f), Random.Range(-0.02f, 0.02f), 0);
                Vector3 direccionDisparo = (canonBase.firePoint.forward + dispersion).normalized;

                rb.AddForce(direccionDisparo * impulseForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(fireRate);
        }

        // Restar municion (1 rafaga completa consume 1 de municion)
        currentBullets--;
        isShooting = false;

        // Si nos quedamos sin balas, llamamos al metodo DisableWeapon() del padre
        if (currentBullets <= 0)
        {
            DisableWeapon();
        }
    }
}