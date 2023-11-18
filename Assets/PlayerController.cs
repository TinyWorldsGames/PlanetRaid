using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Transform collectPoint;


    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.335f;
    public float rotationSmoothTime = 0.12f;
    public float speedChangeRate = 10.0f;
    public float sensitivity = 1f;

    public float jumpHeight = 1.2f;
    public float gravity = -15.0f;
    public float jumpTimeout = 0.50f;
    public float fallTimeout = 0.15f;

    [HideInInspector]
    public bool isGrounded = true;
    float groundedOffset = -0.14f;
    float groundedRadius = 0.28f;
    public LayerMask groundLayers;

    public GameObject cameraTarget;


    public bool lockCameraPosition = false;

    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private float speed;
    private float animationBlend;
    private float targetRotation = 0.0f;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDJump;
    private int animIDFreeFall;
    private int animIDMotionSpeed;

    private Animator animator;
    private CharacterController characterController;
    [SerializeField] PlayerInputHandler playerInput;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] bool rotateOnMove = true;

    private const float threshold = 0.01f;

    private bool hasAnimator;


    private Vector2 movement;


    Vector3 direction;
    [SerializeField]
    Transform[] hitWeapons;

    Transform _weapon;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        hasAnimator = TryGetComponent(out animator);
        characterController = GetComponent<CharacterController>();

        AssignAnimationIDs();

        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    private void Update()
    {
        hasAnimator = TryGetComponent(out animator);

        JumpAndGravity();
        GroundedCheck();
        Move();


    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);

        if (hasAnimator)
        {
            animator.SetBool(animIDGrounded, isGrounded);
        }
    }

    // Build Mode Player Controllers

    private void OnMove(InputValue value)
    {
        if (!playerInput.buildMode)
            return;

        movement = value.Get<Vector2>();

        direction = new Vector3(movement.x, 0f, movement.y);

        if (movement.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }



    public void OnAttackStart()
    {
        moveSpeed = 0f;
    }

    public void OnAttackRelease()
    {
        moveSpeed = 2;
    }







    public void PerformPunch(Enums.Weapons hitWeapon)
    {
        _weapon = hitWeapons[(int)hitWeapon];

        Collider[] hitDamagables = Physics.OverlapSphere(_weapon.position, 0.5f);

        foreach (Collider hit in hitDamagables)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(2);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect(collectPoint);
        }
    }


    //




    private void CameraRotation()
    {
        if (playerInput.buildMode)
            return;

        if (playerInput.look.sqrMagnitude >= threshold && !lockCameraPosition)
        {
            cinemachineTargetYaw += playerInput.look.x * Time.deltaTime * sensitivity;
            cinemachineTargetPitch += playerInput.look.y * Time.deltaTime * sensitivity;
        }

        cameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {
        float targetSpeed = playerInput.sprint ? sprintSpeed : moveSpeed;

        if (playerInput.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = playerInput.analogMovement ? playerInput.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        Vector3 inputDirection = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y).normalized;

        if (playerInput.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);

            if (rotateOnMove && !playerInput.buildMode)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            }
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        if (!playerInput.buildMode)
        {
            characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        }
        else
        {
            characterController.Move(direction * moveSpeed * Time.deltaTime);

        }


        if (hasAnimator)
        {
            animator.SetFloat(animIDSpeed, animationBlend);
            animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (isGrounded)
        {
            fallTimeoutDelta = fallTimeout;

            if (hasAnimator)
            {
                animator.SetBool(animIDJump, false);
                animator.SetBool(animIDFreeFall, false);
            }

            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if (playerInput.jump && jumpTimeoutDelta <= 0.0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

                if (hasAnimator)
                {
                    animator.SetBool(animIDJump, true);
                }
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (hasAnimator)
                {
                    animator.SetBool(animIDFreeFall, true);
                }
            }

            playerInput.jump = false;
        }

        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }



    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMove = newRotateOnMove;
    }
}
