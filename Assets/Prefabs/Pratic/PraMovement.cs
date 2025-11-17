using UnityEngine;
using Fusion;

public class PraMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;
    private float verticalVelocity;

    [SerializeField] private CharacterController characterController;
    private Kien inputActions;

    [Header("Animation")]
    [SerializeField] private NetworkMecanimAnimator networkMecanimAnimator;

    [Header("Audio")]
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            inputActions = new Kien();
            inputActions.Enable();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        MoveMent();
    }

    void MoveMent()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        if (characterController.isGrounded)
        {
            networkMecanimAnimator.Animator.SetBool("Jump", false);
            if (verticalVelocity < 0)
                verticalVelocity = -1f;

            if (inputActions.Player.Jump.WasPerformedThisFrame())
            {
                verticalVelocity = jumpForce;
                networkMecanimAnimator.Animator.SetBool("Jump", true);
            }
        }
        else
        {
            verticalVelocity += gravity * Runner.DeltaTime;
        }

        Vector3 velocityM = moveDir.normalized * moveSpeed;
        velocityM.y = verticalVelocity;

        characterController.Move(velocityM * Runner.DeltaTime);

        networkMecanimAnimator.Animator.SetBool("run", moveDir.sqrMagnitude > 0.01f);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Runner.DeltaTime * rotationSpeed);
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(characterController.center), FootstepAudioVolume);
            }
        }
    }

}
