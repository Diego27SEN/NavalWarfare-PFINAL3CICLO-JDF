using UnityEngine;

public static class SkillFactory
{
    public static void ApplyComplexHability(GameObject shipToUpgrade, string abilityID)
    {
        switch (abilityID)
        {
            case "AgilidadAcuatica":
                // Aumenta la velocidad temporalmente.
                if (shipToUpgrade.GetComponent<AquaticAgilitySkil> () == null)
                {
                    shipToUpgrade.AddComponent<AquaticAgilitySkil>();
                    Debug.Log("Habilidad añadida: Agilidad Acuática");
                }
                break;

            case "BloqueoMaritimo":
                // Spawnea minas alrededor del barco
                if (shipToUpgrade.GetComponent<MaritimeBlockadeSkill>() == null)
                {
                    shipToUpgrade.AddComponent<MaritimeBlockadeSkill>();
                    Debug.Log("Habilidad añadida: Bloqueo Marítimo");
                }
                break;

            case "GatlingGun":
                // Convierte el cañón en ametralladora pesada
                if (shipToUpgrade.GetComponent<GatlingGunSkill>() == null)
                {
                    shipToUpgrade.AddComponent<GatlingGunSkill>();
                    Debug.Log("Habilidad añadida: Gatling Gun");
                }
                break;

            case "Mortero":
                // Convierte el cañón en mortero
                if (shipToUpgrade.GetComponent<MortarSkill>() == null)
                {
                    shipToUpgrade.AddComponent<MortarSkill>();
                    Debug.Log("Habilidad añadida: Mortero");
                }
                break;

            default:
                Debug.LogWarning($"El abilityID '{abilityID}' no existe en la Factory.");
                break;
        }
    }
}
