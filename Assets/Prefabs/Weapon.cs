using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{
    HashSet<NetworkObject> hitObjects = new HashSet<NetworkObject>();
    bool atk;

    public NetworkObject OwnerPlayer;
    public PlayerStas ownerStats;


    public void StartAtk()
    {
        if (HasStateAuthority)
        {
            atk = true;
            hitObjects.Clear();
        }
    }

    public void EndAtk()
    {
        if (HasStateAuthority)
        {
            atk = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        if (!atk) return;


        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<NetworkObject>(out NetworkObject target)) return;

        if (target == OwnerPlayer) return;

        if (!hitObjects.Add(target)) return;

        var stats = other.GetComponent<PlayerStas>();
        if (stats == null) return;

        stats.RpcTakeDamage(10, Object.InputAuthority);
        if(stats.Health <= 0)
        {
            ownerStats.AddScore(100);
        }
    }
}
