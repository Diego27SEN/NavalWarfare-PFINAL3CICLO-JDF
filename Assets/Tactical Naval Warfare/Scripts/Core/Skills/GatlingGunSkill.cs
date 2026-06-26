using UnityEngine;

public class GatlingGunSkill : MonoBehaviour
{
    public int damageExtra = 2;
    public float fireRate = 0.2f;

    void Start()
    {
        Debug.Log($"[Gatling Gun] Instalada. Daño por bala: +{damageExtra} | Cadencia: {fireRate}s");
    }

    public void DisableWeapon()
    {
        Debug.Log("[Gatling Gun] Desmontada. Volviendo al cañon estándar.");
        Destroy(this);
    }
}
