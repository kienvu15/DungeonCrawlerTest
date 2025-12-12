using UnityEngine;
using Fusion;
using Unity.Cinemachine;
using DG.Tweening.CustomPlugins;

public class PlayerPratic : NetworkBehaviour
{
    [Header("Ref")]
    public CharacterController characterController;
    public NetworkMecanimAnimator playerMecanimAnimator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Condition")]
    public bool atk;

    [Header("Audio")]
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    public Kien inputActions;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            inputActions = new Kien();
            inputActions.Enable();

            var cinecam = FindFirstObjectByType<CinemachineCamera>();
            if (cinecam != null)
            {
                var target = cinecam.Target;
                target.TrackingTarget = transform;
                target.LookAtTarget = transform;
                cinecam.Target = target;
            }
        }
    }

    public void Update()
    {
        if (!Object.HasInputAuthority) return;

        if (inputActions.Player.Atk.WasPerformedThisFrame())
        {
            atk = true;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RpcAttackTrigger()
    {
        RpcPerformAttack();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcPerformAttack()
    {
        int attack = Random.Range(0, 2);
        playerMecanimAnimator.Animator.SetTrigger("atk");
        playerMecanimAnimator.Animator.SetInteger("attack", attack);
    }

    public override void FixedUpdateNetwork()
    {
        if (atk) 
        {
            RpcAttackTrigger(); 
            atk = false;
        }

        Movement();
    }

    public void Movement()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        Vector3 moveVelocity = moveDir.normalized * moveSpeed;
        
        characterController.Move(moveVelocity * Runner.DeltaTime);
        playerMecanimAnimator.Animator.SetBool("run", moveDir.sqrMagnitude > 0);

        if (moveDir != Vector3.zero)
        { 
            Quaternion playerRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, Runner.DeltaTime*rotationSpeed);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            RpcPlayFootStep();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RpcPlayFootStep()
    {
        if (FootstepAudioClips.Length > 0)
        {
            var index = Random.Range(0, FootstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(characterController.center), FootstepAudioVolume);
        }
    }
}
