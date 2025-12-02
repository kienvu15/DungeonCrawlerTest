using Fusion;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Timer : SimulationBehaviour, IPlayerJoined
{
    public NetworkPrefabRef timerPrefab;

    private NetworkObject timerInstance;

    public void PlayerJoined(PlayerRef player)
    {
        // Chỉ host spawn 1 lần duy nhất
        if (!Runner.IsServer) return;

        if (timerInstance == null)
        {
            timerInstance = Runner.Spawn(timerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
