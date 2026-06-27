using UnityEngine;

public class GatlingGunSkill : MonoBehaviour, ITemporaryWeapon
{
    public int damageExtra = 2;
    public float fireRate = 0.2f;

    void Start()
    {
        Debug.Log($"[Gatling Gun] Instalada. Daño por bala: +{damageExtra} | Cadencia: {fireRate}s");
    }

    public void DisableWeapon()
    {
        Debug.Log("[Gatling Gun] Desmontada.");
        Destroy(this);
    }

    public void FireShot()
    {
        // Aquí pones la lógica rápida de la ametralladora (+2 daño)
        Debug.Log("¡Ratatatata! Disparando Gatling.");
    }
}
