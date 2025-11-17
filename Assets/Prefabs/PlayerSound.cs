using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioS;

    public List<AudioClip> clips;

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void PlaySoundAttack()
    {
        audioS.PlayOneShot(clips[0]);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void PlayerSoundStep()
    {
        audioS.PlayOneShot(clips[1]);
    }

}
