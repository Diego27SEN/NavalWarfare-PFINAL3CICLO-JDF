using UnityEngine;
using System.Collections;

public class GatlingGunSkill : MonoBehaviour, ITemporaryWeapon
{
    public float damageFixed = 2f;
    public float fireRate = 0.2f;
    public int balasPorRafaga = 4;
    public float impulseForce = 150f;

    private bool isShooting = false;
    private ShootController cañonBase; 
    public void Start()
    {
        cañonBase = GetComponent<ShootController>();
        Debug.Log("[Gatling Gun] Ametralladora lista para desatar el caos.");
    }

    public void DisableWeapon()
    {
        Debug.Log("[Gatling Gun] Desmontada.");
        Destroy(this);
    }

    public void FireShot()
    {
        // Si pasa algo que nos impida disparar, salimos de inmediato.
        if (isShooting) return;
        if (cañonBase == null) return;

        StartCoroutine(MachineGunBurst());
        Debug.Log("¡Ratatatata! Disparando Gatling.");
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

        yield return new WaitForSeconds(0.3f);
        isShooting = false;
    }
}
