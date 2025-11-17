using UnityEngine;
using Fusion;

public class PraSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefab;
    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            var playerPre = Runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }
}