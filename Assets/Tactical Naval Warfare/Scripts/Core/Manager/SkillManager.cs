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
    public void EquiparHabilidadAlBarcoActivo(string tipoHabilidad)
    {
        // Buscamos el TurnManager en la escena
        TurnManager turnManager = FindAnyObjectByType<TurnManager>();

        // Pedimos el barco 
        ShipController barco = turnManager.GetBarcoActual();

        // Verificamos
        if (barco == null)
        {
            Debug.LogWarning("No hay barco activo para equipar.");
            return;
        }

        GameObject barcoActivo = barco.gameObject;

        // Si el barco ya tiene un arma especial, la quitamos
        if (barcoActivo.TryGetComponent<ITemporaryWeapon>(out ITemporaryWeapon armaPrevia))
        {
            armaPrevia.DisableWeapon();
        }

        // Añadimos el nuevo componente
        switch (tipoHabilidad)
        {
            case "Gatling":
                GatlingGunSkill gatling = barcoActivo.AddComponent<GatlingGunSkill>();
                gatling.damageFixed = 2f;
                gatling.fireRate = 0.2f;
                gatling.balasPorRafaga = 4;
                Debug.Log($"[TurnManager] Gatling equipada en {barcoActivo.name}");
                break;

            case "Mortar":
                MortarSkill mortero = barcoActivo.AddComponent<MortarSkill>();
                mortero.damageFixed = 60f; 
                mortero.maxBullets = 3;
                Debug.Log($"[TurnManager] Mortero equipado en {barcoActivo.name}");
                break;

            default:
                Debug.LogWarning($"[TurnManager] La habilidad '{tipoHabilidad}' no existe.");
                break;
        }
    }
}
