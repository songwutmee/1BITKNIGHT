using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStatus))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCameraTransform;
    public PlayerStatus PlayerStatus { get; private set; }

    private IPlayerState _currentState;
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
   
    public PlayerGetHitState GetHitState { get; private set; }


    public Vector2 MoveInput { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _rotationSpeed = 15.0f;

    [Header("Ground Check & Gravity")]
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _verticalVelocity;
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
   
        PlayerStatus = GetComponent<PlayerStatus>(); 

        GroundedState = new PlayerGroundedState(this);
        AttackState = new PlayerAttackState(this);
        DodgeState = new PlayerDodgeState(this);
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

    public void PlayGetHitAnimation()
    {
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