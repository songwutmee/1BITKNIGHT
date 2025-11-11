using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
// LEAD COMMENT: เพิ่ม PlayerStatus เข้าไปใน RequireComponent ด้วย
// เพื่อให้แน่ใจว่าระบบทั้งหมดทำงานร่วมกันได้
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    // --- Dependencies ---
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCameraTransform;
    // --- [เพิ่มใหม่] ---
    // LEAD COMMENT: เพิ่ม reference ไปยัง PlayerStatus เพื่อให้ State ต่างๆ
    // สามารถตรวจสอบและใช้ Stamina ได้ นี่คือการสื่อสารระหว่าง Component โดยตรง
    public PlayerStatus PlayerStatus { get; private set; }

    // --- State Machine ---
    private IPlayerState _currentState;
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    // --- [เพิ่มใหม่] ---
    public PlayerGetHitState GetHitState { get; private set; }


    // --- Input Handling ---
    public Vector2 MoveInput { get; private set; }

    // --- Movement & Rotation Parameters ---
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;

    [Header("Ground Check & Gravity")]
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _verticalVelocity;

    // --- Animator Hashes ---
    public readonly int SpeedHash = Animator.StringToHash("Speed");
    public readonly int LightAttackHash = Animator.StringToHash("LightAttack");
    public readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");
    public readonly int DodgeHash = Animator.StringToHash("Dodge");
    public readonly int JumpHash = Animator.StringToHash("Jump");
    public readonly int GetHitHash = Animator.StringToHash("GetHit");


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mainCameraTransform = Camera.main.transform;
        // --- [เพิ่มใหม่] ---
        PlayerStatus = GetComponent<PlayerStatus>(); // Cache PlayerStatus

        // --- State Machine Initialization ---
        GroundedState = new PlayerGroundedState(this);
        AttackState = new PlayerAttackState(this);
        DodgeState = new PlayerDodgeState(this);
        // --- [เพิ่มใหม่] ---
        GetHitState = new PlayerGetHitState(this);
    }

    private void Start()
    {
        SwitchState(GroundedState);
    }

    private void Update()
    {
        _currentState?.Update();
        HandleGravity();
    }

    public void SwitchState(IPlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    // --- [เพิ่มใหม่] ---
    // LEAD COMMENT: นี่คือฟังก์ชันที่ PlayerStatus จะเรียกใช้
    // มันทำหน้าที่เป็น "สะพาน" ไปยัง GetHitState
    public void PlayGetHitAnimation()
    {
        // เราจะเปลี่ยนไป GetHitState ซึ่งจะจัดการล็อคการควบคุมและเล่น Animation เอง
        SwitchState(GetHitState);
    }

    private void HandleGravity()
    {
        if (_controller.isGrounded && _verticalVelocity.y < 0)
        {
            _verticalVelocity.y = -2f;
        }
        _verticalVelocity.y += _gravity * Time.deltaTime;
        _controller.Move(_verticalVelocity * Time.deltaTime);
    }

    #region Input System Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _currentState?.OnJump();
        }
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _currentState?.OnLightAttack();
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _currentState?.OnHeavyAttack();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _currentState?.OnDash();
        }
    }
    #endregion

    #region Public Utility Functions
    public void MoveCharacter(float speedMultiplier = 1f)
    {
        Vector3 moveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
        moveDirection = _mainCameraTransform.forward * moveDirection.z + _mainCameraTransform.right * moveDirection.x;
        moveDirection.y = 0;

        _controller.Move(moveDirection.normalized * (_moveSpeed * speedMultiplier * Time.deltaTime));

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    public Animator GetAnimator()
    {
        return _animator;
    }
    #endregion
}