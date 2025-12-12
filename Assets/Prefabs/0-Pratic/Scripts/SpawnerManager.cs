using UnityEngine;
using Fusion;

public class SpawnerManager : SimulationBehaviour, IPlayerJoined
{
    public GameObject SpawnerObj;
    public void PlayerJoined(PlayerRef player)
    {
        if(player == Runner.LocalPlayer)
        {
            var playerPre = Runner.Spawn(SpawnerObj, Vector3.zero, Quaternion.identity, player);
        }
    }
}
