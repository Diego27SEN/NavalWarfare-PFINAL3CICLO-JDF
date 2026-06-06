using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRuntimeSetSO", menuName = "TacticalNavalWarfare/PlayerRuntimeSetSO")]
public class PlayerRuntimeSetData : SerializedScriptableObject
{
    [ShowInInspector]
    public Dictionary<string, PlayerGame> PlayersActive = new ();

    // Limpiar la lista al iniciar la escena
    public void Initialize()
    {
        PlayersActive.Clear();
    }

    public void AddPlayer(PlayerGame player)
    {
        if (!PlayersActive.ContainsKey(player.PlayerID))
        {
            PlayersActive.Add(player.PlayerID, player);
        }
    }
}
