using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRuntimeSetSO", menuName = "NavalWarfare/PlayerRuntimeSetSO")]
public class PlayerRuntimeSetSO : ScriptableObject
{
    [ShowInInspector]
    public List<PlayerGame> PlayersActive = new List<PlayerGame>();

    // Limpiar la lista al iniciar la escena
    public void Initialize()
    {
        PlayersActive.Clear();
    }

    public void AddPlayer(PlayerGame player)
    {
        if (!PlayersActive.Contains(player))
        {
            PlayersActive.Add(player);
        }
    }
}
