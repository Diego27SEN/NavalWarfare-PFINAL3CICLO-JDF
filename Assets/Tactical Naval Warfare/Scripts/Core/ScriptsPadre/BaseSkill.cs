using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ITemporaryWeapon
{
    [Header("Configuración de Habilidad Base")]
    public int maxBullets;
    public int currentBullets;

    protected virtual void ResetWeapon()
    {
        currentBullets = maxBullets;
        Debug.Log($"[{gameObject.name}] Habilidad sin munición. Desmontando.");
    }

    // Polimorfismo : Obligamos a los hijos a definir CÓMO disparan
    public abstract void FireShot();

    // Herencia: Todos los hijos comparten cómo se destruyen
    public virtual void DisableWeapon()
    {
        Destroy(this);
    }
}
