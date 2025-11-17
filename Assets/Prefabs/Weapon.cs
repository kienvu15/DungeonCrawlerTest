using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : NetworkBehaviour
{

    HashSet<NetworkObject> hitObjects;
    bool atk;

    private void Awake()
    {
        hitObjects = new HashSet<NetworkObject>();
    }

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
            hitObjects.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Object.HasStateAuthority == false) return;
        if(!atk) return;
        if (!other.CompareTag("Player")) return;

        if (!other.TryGetComponent<NetworkObject>(out NetworkObject netObj)) return;
        if(netObj == Object) return;
        if (!hitObjects.Add(netObj)) return;

        Debug.Log("Hit Player");
        var otherAtributes = other.GetComponent<PlayerAtributes>();
        otherAtributes.RpcApplyDamage(10f, Object.InputAuthority);
    }

}
