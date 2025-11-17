using UnityEngine;
using System.Collections.Generic;

public class PlayerAnimationEvent : MonoBehaviour
{
    public Weapon weapon;
    
    public void Start_Atk() => weapon.StartAtk();
    public void End_Atk() => weapon.EndAtk();


    public void AtkSound()
    {
        var playerSound = GetComponent<PlayerSound>();
        playerSound.PlaySoundAttack();
    }

    public void StepSound()
    {
        var playerSound = GetComponent<PlayerSound>();
        playerSound.PlayerSoundStep();
    }

}
