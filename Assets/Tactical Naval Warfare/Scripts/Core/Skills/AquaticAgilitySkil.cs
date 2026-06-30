using UnityEngine;
using System.Collections;

public class AquaticAgilitySkil : MonoBehaviour
{
    private float bonusVelocity = 20f;
    private float timeDuration = 120f;
    private float originalSpeed;
    void Start()
    {
        StartCoroutine(SpeedRoutine());
    }

    private IEnumerator SpeedRoutine()
    {
        ShipController ship = GetComponent<ShipController>();

        // Guardamos el valor original antes de tocarlo
        originalSpeed = ship.moveSpeed;
        Debug.Log($"[Agilidad Acuática] Velocidad original guardada: {originalSpeed}");

        // Aplicamos el buff
        ship.moveSpeed = originalSpeed + bonusVelocity;
        Debug.Log($"[Agilidad Acuática] Activada: +{bonusVelocity}. Velocidad actual: {ship.moveSpeed}");

        yield return new WaitForSeconds(timeDuration);

        // Restauramos el valor exacto original
        ship.moveSpeed = originalSpeed;
        Debug.Log($"[Agilidad Acuática] Terminada. Velocidad restaurada a: {ship.moveSpeed}");

        Destroy(this);
        Debug.Log("[Agilidad Acuática] Componente destruido.");
    }
}
