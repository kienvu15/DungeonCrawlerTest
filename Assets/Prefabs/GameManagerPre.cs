using Fusion;
using TMPro;
using UnityEngine;

public class GameManagerPre : NetworkBehaviour
{
    public NetworkRunner runner;
    public GameObject matchTimePrefab;
    public TextMeshProUGUI ee;

    void Start()
    {
        if (runner.IsServer) // Hoặc HasStateAuthority
        {
            runner.Spawn(matchTimePrefab);
        }
    }
}
