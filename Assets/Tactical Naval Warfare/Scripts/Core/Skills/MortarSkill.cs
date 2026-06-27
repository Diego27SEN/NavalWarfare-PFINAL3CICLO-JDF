using UnityEngine;

public class MortarSkill : MonoBehaviour, ITemporaryWeapon
{
    public int damageFixed = 60;
    public int maxBullets = 3;
    public int currentBullets;

    void Start()
    {
        currentBullets = maxBullets;
        Debug.Log($"[Mortero] Instalado. Municion: {currentBullets} | Daño destructivo: {damageFixed}");
    }

    public void RegisterShot()
    {
        if (currentBullets > 0)
        {
            currentBullets--;
            Debug.Log($"[Mortero] ¡Fuego! Balas restantes: {currentBullets}");

            if (currentBullets <= 0)
            {
                DisableWeapon();
            }
        }
    }

    public void DisableWeapon()
    {
        Debug.Log("[Mortero] Desmontado.");
        Destroy(this);
    }
    public void FireShot()
    {
        // Aquí pones la lógica del misilazo (60 daño, resta balas)
        Debug.Log("¡Booooom! Disparando Mortero.");
        currentBullets--;
    }
}
