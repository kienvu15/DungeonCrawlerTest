using UnityEngine;
using Fusion;
using Unity.Cinemachine;

public class PraMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -20f;
    private float verticalVelocity;
    bool jumpPressed;

    [SerializeField] private CharacterController characterController;
    private Kien inputActions;

    [Header("Animation")]
    [SerializeField] private NetworkMecanimAnimator networkMecanimAnimator;

    [Header("Audio")]
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    //[Networked, OnChangedRender(nameof(OnchangeScore))] public int score { get; set; }

    void OnchangeScore()
    {
        
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            inputActions = new Kien();
            inputActions.Enable();

            var cineCam = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
            if (cineCam != null)
            {
                var target = cineCam.Target;
                target.TrackingTarget = transform;  
                target.LookAtTarget = transform;    
                cineCam.Target = target;
            }
        }
    }


    bool atk;
    bool specialAtk;
    int attack;
    void Update()
    {
        if (!HasInputAuthority) return;

        if (inputActions.Player.Atk.WasPerformedThisFrame())
            atk = true;

        if(inputActions.Player.SpecialAtk.WasPerformedThisFrame())
        {
            specialAtk = true;
        }

        if (inputActions.Player.Jump.WasPerformedThisFrame())
            jumpPressed = true;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RpcRequestAttack()
    {
        RpcPlayAttack();
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcPlayAttack()
    {
        attack = Random.Range(0, 2);
        networkMecanimAnimator.Animator.SetTrigger("atk");
        networkMecanimAnimator.Animator.SetInteger("attack", attack);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RpcSpecialAttack()
    {
        RpcPlaySpecialAttack();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RpcPlaySpecialAttack()
    {
        networkMecanimAnimator.Animator.SetTrigger("SPatk");
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;
        if (atk)
        {
            SoundManager.Instance.Play("Attack");
            RpcRequestAttack();
            atk = false;
        }
        if(specialAtk)
        {
            RpcSpecialAttack();
            specialAtk = false;
        }

        MoveMent();
    }

    void MoveMent()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        if (characterController.isGrounded)
        {
            networkMecanimAnimator.Animator.SetBool("Jump", false);

            if (jumpPressed)
            {
                verticalVelocity = jumpForce;
                networkMecanimAnimator.Animator.SetBool("Jump", true);
                jumpPressed = false;
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

    private void OnTriggerEnter(Collider collision)
    {
        if (!HasInputAuthority) return;
        if (!collision.CompareTag("Coin")) return;

        PreUIManager.Instance.SetScore(100);
        Destroy(collision.gameObject);
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
