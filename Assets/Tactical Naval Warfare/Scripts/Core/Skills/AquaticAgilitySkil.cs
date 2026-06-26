using UnityEngine;
using System.Collections;

public class AquaticAgilitySkil : MonoBehaviour
{
    private float bonusVelocity = 20f;
    private float timeDuration = 120f;
    void Start()
    {
        StartCoroutine(SpeedRoutine());
    }

    private IEnumerator SpeedRoutine()
    {
        // Aumentamos al velocidad
        GetComponent<ShipController>().moveSpeed += bonusVelocity;
        Debug.Log($"[Agilidad Acuática] Activada: +{bonusVelocity} de velocidad.");

        yield return new WaitForSeconds(timeDuration);

        // Quitamos la velocidad
        GetComponent<ShipController>().moveSpeed -= bonusVelocity;
        Debug.Log("[Agilidad Acuática] Terminada. Velocidad normal restaurada.");

        // Limpiamos
        Destroy(this);
    }
}
