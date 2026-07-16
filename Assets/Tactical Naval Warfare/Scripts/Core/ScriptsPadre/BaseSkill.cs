using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ITemporaryWeapon
{
    [Header("Configuracion de Habilidad Base")]
    public int maxBullets;
    public int currentBullets;

    protected virtual void ResetWeapon()
    {
        currentBullets = maxBullets;
        Debug.Log($"[{gameObject.name}] Habilidad sin municion. Desmontando.");
    }

    // Polimorfismo : Obligamos a los hijos a definir como disparan
    public abstract void FireShot();

    // Herencia: Todos los hijos comparten como se destruyen
    public virtual void DisableWeapon()
    {
        Destroy(this);
    }
}
