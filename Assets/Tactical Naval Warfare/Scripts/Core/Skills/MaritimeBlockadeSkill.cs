using UnityEngine;

public class MaritimeBlockadeSkill : MonoBehaviour
{
    private int marineBombs = 3;
    private float radioAppearance = 5f;

    void Start()
    {
        ExecuteLock();
    }

    public void ExecuteLock()
    {
        // Prefab de la mina
        GameObject minaPrefab = Resources.Load<GameObject>("MinaMarina");

        if (minaPrefab == null)
        {
            Debug.LogWarning("No se encontro el prefab 'MinaMarina' en la carpeta Resources.");
            return;
        }

        // Buscamos a los barcos enemigos por su Tag
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemigo in enemigos)
        {
            for (int i = 0; i < marineBombs; i++)
            {

                Vector3 posicionAleatoria = enemigo.transform.position + (Random.insideUnitSphere * radioAppearance);
                posicionAleatoria.y = 0; 

                Instantiate(minaPrefab, posicionAleatoria, Quaternion.identity);
            }
        }

        Debug.Log("[Bloqueo Marítimo] Minas desplegadas con exito.");
        Destroy(this);
    }
}
