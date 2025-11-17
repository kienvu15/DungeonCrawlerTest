using UnityEngine;
using Fusion;
using Unity.Cinemachine;
using System.Collections.Generic;

public class MoveTest : NetworkBehaviour
{

    

    [Header("Move")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    private bool jumpPressed = false;
    private Vector3 velocity = Vector3.zero;

    NetworkMecanimAnimator networkMecanimAnimator;
    private Animator anim;

    [Networked, OnChangedRender(nameof(OnSpeedChange))]
    public bool run { get; set; }

    [Networked]
    public bool atk { get; set; }

    [Networked]
    public bool jump { get; set; }

    
    public RectTransform fill;

    [Networked, OnChangedRender(nameof(OnchangeScore))] public int score { get; set; }

    private CharacterController characterController;

    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    void OnchangeScore()
    {
        List<LeaderBoardRowInfo> getlist = UIManager.Instance.gameManager.leaderBoard();
        UIManager.Instance.SetText(getlist);
    }

    public override void Spawned()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        networkMecanimAnimator = GetComponent<NetworkMecanimAnimator>();

        GameObject cinemachineCamera = GameObject.Find("PlayerFollowCamera");
        cinemachineCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;
    }

    

    private Kien inputActions;
    public void OnEnable()
    {
        inputActions = new Kien();
        inputActions.Enable();

        inputActions.Player.Atk.started += Atk_start;
        inputActions.Player.Jump.started += Jump_start;
    }

    

    void Atk_start(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        atk = true;
    }

    void Jump_start(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        jump = true;
    }

    


    void Update()
    {
        if (!Object.HasInputAuthority) return;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority) return;

        if (characterController.isGrounded)
            velocity.y = -1f;
        else
            velocity.y += Physics.gravity.y * Runner.DeltaTime;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(horizontal, 0, vertical).normalized;

        if (jumpPressed && characterController.isGrounded)
        {
            velocity.y = jumpForce;
            anim.SetBool("Jump", true);
        jumpPressed = false;
        }


        if(characterController.isGrounded && velocity.y < 0)
        {
            anim.SetBool("Jump", false);
        }

        Vector3 moveVector = (moveInput * speed) + velocity;

        characterController.Move(moveVector * Runner.DeltaTime);

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }

        if (moveInput.magnitude > 0)
        {
            run = true;
        }
        else
        {
            run = false;

        }

        if (atk)
        {
            atk = false;
            int rnd = Random.Range(0, 2);
            anim.SetInteger("attack", rnd);
            networkMecanimAnimator.SetTrigger("atk");
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            atk = true;
        }
    }


    void OnSpeedChange()
    {
        anim.SetBool("run", run);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (!HasInputAuthority) return;
        if (!collision.CompareTag("Coin")) return;

        score++;
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
