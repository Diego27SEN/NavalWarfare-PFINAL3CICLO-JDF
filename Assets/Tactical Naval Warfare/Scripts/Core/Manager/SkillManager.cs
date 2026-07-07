using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void EquiparHabilidadAlBarco(ShipController barco, string tipoHabilidad)
    {
        if (barco == null)
        {
            Debug.LogWarning("No hay barco para equipar.");
            return;
        }

        GameObject barcoActivo = barco.gameObject;

        if (barcoActivo.TryGetComponent<ITemporaryWeapon>(out ITemporaryWeapon armaPrevia))
        {
            armaPrevia.DisableWeapon();
        }

        switch (tipoHabilidad)
        {
            case "Gatling":
                GatlingGunSkill gatling = barcoActivo.AddComponent<GatlingGunSkill>();
                gatling.damageFixed = 2f;
                gatling.fireRate = 0.2f;
                gatling.balasPorRafaga = 4;
                break;

            case "Mortar":
                MortarSkill mortero = barcoActivo.AddComponent<MortarSkill>();
                mortero.damageFixed = 60f;
                mortero.maxBullets = 3;
                break;

            default:
                Debug.LogWarning($"[SkillManager] La habilidad '{tipoHabilidad}' no existe.");
                break;
        }
    }
}
