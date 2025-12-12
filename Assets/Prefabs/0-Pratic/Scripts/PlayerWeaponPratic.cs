using Fusion;
using UnityEngine;

public class PlayerWeaponPratic : NetworkBehaviour
{
    private bool isAttacking = false;
    public NetworkObject owner;
    public PlayerStasPratic stas;

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;

        NetworkObject targetObj = other.GetComponent<NetworkObject>();
        if (targetObj == null) return;
        if (targetObj == owner) return;

        var targetStas = other.GetComponent<PlayerStasPratic>();

        if(targetStas != null)
        {
            targetStas.RpcTakeDamage(stas.damage);
        }
    }

    public void StartAttack()
    {
        if (Object.HasInputAuthority)
        {
            isAttacking = true;
        }
    }

    public void EndAttack()
    {
        if (Object.HasInputAuthority)
        {
            isAttacking = false;
        }
    }
}
