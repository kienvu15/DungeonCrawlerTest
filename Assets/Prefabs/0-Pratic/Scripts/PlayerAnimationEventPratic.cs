using UnityEngine;

public class PlayerAnimationEventPratic : MonoBehaviour
{
    public PlayerWeaponPratic playerWeaponPratic;

    public void StartAttack() => playerWeaponPratic.StartAttack();
    public void EndAttack() => playerWeaponPratic.EndAttack();
}
