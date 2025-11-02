using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // อย่าลืมติดตั้งและตั้งค่า Unity's New Input System

// [Header("...")] เป็น Attribute ที่ช่วยจัดระเบียบ Inspector ให้ดูง่าย
// [RequireComponent(...)] บังคับให้ GameObject นี้ต้องมี Component ที่ระบุเสมอ ป้องกัน Human Error

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // --- Dependencies ---
    // LEAD COMMENT: เราจะเก็บ reference ของ component ที่ต้องใช้บ่อยๆ ไว้ในตัวแปร private
    // เพื่อลดการเรียก GetComponent() ซึ่งมี cost เล็กน้อย และทำให้โค้ดอ่านง่ายขึ้น
    // การเติม _ หน้าชื่อตัวแปร private เป็น convention ที่นิยมกัน
    private CharacterController _controller;
    private Animator _animator;
    private Transform _mainCameraTransform;

    // --- State Machine ---
    // LEAD COMMENT: หัวใจของ Controller ของเรา ตัวแปรนี้จะเก็บ State ปัจจุบันที่ผู้เล่นเป็นอยู่
    // Controller ไม่ได้ทำ Logic เอง แต่จะมอบหมายให้ State ปัจจุบันเป็นคนทำในแต่ละเฟรม
    private IPlayerState _currentState;

    // เราจะสร้าง property ของ State ต่างๆ เพื่อให้เข้าถึงได้จากภายนอกถ้าจำเป็น
    // และเพื่อให้ State หนึ่งสามารถส่งต่อไปยังอีก State หนึ่งได้
    public PlayerGroundedState GroundedState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    // อนาคตจะมี PlayerJumpingState, PlayerSkillCastState ฯลฯ

    // --- Input Handling ---
    // LEAD COMMENT: เราใช้ Vector2 เพื่อเก็บค่าที่อ่านได้จาก Input (WASD, Gamepad Stick)
    // การแยกตัวแปรเก็บ Input ทำให้ State ต่างๆ สามารถดึงไปใช้ได้โดยไม่ต้องผูกติดกับ Input System โดยตรง
    public Vector2 MoveInput { get; private set; }
    public bool IsDashPressed { get; private set; }

    // --- Movement & Rotation Parameters ---
    [Header("Movement Settings")]
    [Tooltip("ความเร็วในการเดินปกติของตัวละคร")]
    [SerializeField] private float _moveSpeed = 5.0f;
    [Tooltip("ความเร็วในการหมุนตัวละครให้หันตามทิศทางที่เดิน")]
    [SerializeField] private float _rotationSpeed = 15.0f;

    [Header("Ground Check & Gravity")]
    [SerializeField] private float _gravity = -9.81f;
    private Vector3 _verticalVelocity;

    // --- Animator Hashes ---
    // LEAD COMMENT: Best Practice! การใช้ Animator.Set...("string") ใน Update() ทุกเฟรมนั้นช้า
    // เราจะแปลง string parameter เป็น integer hash ตอนเริ่มต้น (Awake/Start) และใช้ hash แทน
    // ซึ่งให้ Performance ที่ดีกว่าอย่างมีนัยสำคัญ
    public readonly int SpeedHash = Animator.StringToHash("Speed");
    public readonly int LightAttackHash = Animator.StringToHash("LightAttack");
    public readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");
    public readonly int DodgeHash = Animator.StringToHash("Dodge");
    public readonly int JumpHash = Animator.StringToHash("Jump");


    private void Awake()
    {
        Debug.Log("--- PlayerController Awake ---"); // <-- สายลับ #1
        _controller = GetComponent<CharacterController>();
        if (_controller == null) Debug.LogError("CharacterController NOT FOUND!"); // <-- เช็ค

        _animator = GetComponent<Animator>();
        if (_animator == null) Debug.LogError("Animator NOT FOUND!"); // <-- เช็้ค

        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
            Debug.Log("Main Camera Found!"); // <-- เช็ค
        }
        else
        {
            Debug.LogError("MAIN CAMERA NOT FOUND! Please check the camera's TAG."); // <-- เช็ค
        }

        GroundedState = new PlayerGroundedState(this);
        AttackState = new PlayerAttackState(this);
        DodgeState = new PlayerDodgeState(this);
    }

    private void Start()
    {
        // LEAD COMMENT: กำหนด State เริ่มต้นใน Start() หลังจากที่ State ทั้งหมดถูกสร้างแล้วใน Awake()
        // โดยปกติแล้วผู้เล่นจะเริ่มต้นในสถานะ "อยู่บนพื้น"
        SwitchState(GroundedState);
    }

    private void Update()
    {
        // --- State Delegation & Gravity ---
        // LEAD COMMENT: Update() ของ Controller จะเรียบง่ายเสมอ
        // แค่สั่งให้ State ปัจจุบันทำงาน และจัดการ Gravity ซึ่งเป็น Logic ที่เกิดขึ้นในทุก State
        _currentState?.Update();
        HandleGravity();
    }

    public void SwitchState(IPlayerState newState)
    {
        // LEAD COMMENT: ฟังก์ชันสำหรับเปลี่ยน State จัดการการ Exit จาก State เก่า
        // และ Enter เข้า State ใหม่อย่างเป็นระบบ
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void HandleGravity()
    {
        // LEAD COMMENT: CharacterController ของ Unity ไม่ได้จัดการ Gravity ให้เราอัตโนมัติ
        // เราจึงต้องเขียน Logic ส่วนนี้เอง
        if (_controller.isGrounded && _verticalVelocity.y < 0)
        {
            // ถ้าติดพื้น ให้แรงโน้มถ่วงมีค่าน้อยๆ เพื่อให้ตัวละครติดพื้นเสมอ
            _verticalVelocity.y = -2f;
        }

        // เพิ่มแรงโน้มถ่วงในแต่ละเฟรม
        _verticalVelocity.y += _gravity * Time.deltaTime;
        _controller.Move(_verticalVelocity * Time.deltaTime);
    }

    #region Input System Callbacks
    // LEAD COMMENT: นี่คือฟังก์ชันที่จะถูกเรียกโดยอัตโนมัติจาก Component "PlayerInput"
    // เมื่อมีการกดปุ่มที่เราตั้งค่าไว้ใน Input Action Asset
    // การทำแบบนี้ (Event-based) ดีกว่าการเช็คใน Update() เพราะมันเกิดขึ้นเมื่อจำเป็นเท่านั้น

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        Debug.Log("OnMove Called! Input: " + MoveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // State ปัจจุบันจะเป็นคนตัดสินใจเองว่าสามารถกระโดดได้หรือไม่
            _currentState?.OnJump();
        }
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        Debug.Log("!!! PLAYER CONTROLLER RECEIVED LightAttack INPUT !!!");
        if (context.performed)
        {
            _currentState?.OnLightAttack();
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log("!!! PLAYER CONTROLLER RECEIVED HeavyAttack INPUT !!!");
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
    // LEAD COMMENT: สร้างฟังก์ชันกลางที่ State ต่างๆ สามารถเรียกใช้ได้ เพื่อลดการเขียนโค้ดซ้ำซ้อน
    // และทำให้ State ไม่จำเป็นต้องรู้รายละเอียดการทำงานของ CharacterController หรือ Animator โดยตรง
    
    public void MoveCharacter(float speedMultiplier = 1f)
    {
        Debug.Log("MoveCharacter is being executed!");

        // คำนวณทิศทางการเคลื่อนที่ตามมุมกล้อง
        Vector3 moveDirection = new Vector3(MoveInput.x, 0, MoveInput.y);
        moveDirection = _mainCameraTransform.forward * moveDirection.z + _mainCameraTransform.right * moveDirection.x;
        moveDirection.y = 0;
        
        // สั่งให้ CharacterController เคลื่อนที่
        _controller.Move(moveDirection.normalized * (_moveSpeed * speedMultiplier * Time.deltaTime));

        // หันตัวละครไปตามทิศทางที่เดิน
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