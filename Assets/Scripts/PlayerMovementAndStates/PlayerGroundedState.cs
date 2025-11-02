using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: นี่คือ State ที่จัดการ Logic ทั้งหมดเมื่อผู้เล่นอยู่บนพื้น
// ไม่ว่าจะเป็นการยืนนิ่ง (Idle) หรือการเคลื่อนที่ (Movement)
// หน้าที่หลักของมันคือ:
// 1. อ่าน Input การเคลื่อนที่แล้วสั่งให้ตัวละครเดินและเล่น Animation
// 2. คอยดักจับ Input อื่นๆ (โจมตี, หลบ) เพื่อเปลี่ยนไป State อื่น
public class PlayerGroundedState : IPlayerState
{
    // เราต้องเก็บ reference ของ PlayerController ไว้ เพื่อที่จะได้เรียกใช้ฟังก์ชันกลาง
    // เช่น MoveCharacter() หรือ SwitchState()
    private readonly PlayerController _playerController;

    // นี่คือ Constructor ของคลาส จะถูกเรียกเพียงครั้งเดียวตอนที่ PlayerController สร้าง State นี้ขึ้นมา
    public PlayerGroundedState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter()
    {
        // LEAD COMMENT: Enter() จะถูกเรียกเมื่อเรา "เข้ามา" ใน State นี้
        // เป็นที่ที่ดีในการตั้งค่าเริ่มต้น เช่น การเปิด/ปิดบางอย่าง
        // สำหรับตอนนี้ เราแค่ Log ไว้เพื่อ Debug ว่าเราเข้า State ถูกต้อง
        Debug.Log("Entering Grounded State");
    }

    public void Update()
    {
        // LEAD COMMENT: Update() คือหัวใจของ State นี้ มันจะถูกเรียกทุกเฟรม
        // ตราบใดที่ PlayerController ยังอยู่ใน GroundedState

        // อ่านค่า Input จาก PlayerController ที่รับมาจาก Input System
        Vector2 moveInput = _playerController.MoveInput;

        // เช็คว่ามีการกดปุ่มเคลื่อนที่หรือไม่ (ค่า length ของ vector > 0)
        if (moveInput.sqrMagnitude > 0.01f) // ใช้ sqrMagnitude จะเร็วกว่า .magnitude
        {
            // ถ้ามีการเคลื่อนที่, เรียกฟังก์ชันกลางใน Controller เพื่อขยับตัวละคร
            Debug.Log("GroundedState detects movement. Calling MoveCharacter...");
            _playerController.MoveCharacter();
            // และส่งค่าความเร็วไปให้ Animator เพื่อเล่น Animation เดิน/วิ่ง
            _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, moveInput.magnitude);
        }
        else
        {
            // ถ้าไม่มีการเคลื่อนที่ (ปล่อยปุ่ม WASD), ให้ Animator กลับไปที่ท่า Idle
            _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, 0f);
        }
    }

    public void Exit()
    {
        // LEAD COMMENT: Exit() จะถูกเรียกเมื่อเรา "กำลังจะออกจาก" State นี้
        // เป็นที่ที่ดีในการเคลียร์ค่าต่างๆ ก่อนจะไป State ใหม่
        // เช่น เราอาจจะตั้งค่า Speed ใน Animator ให้เป็น 0 เพื่อความแน่นอน
        _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, 0f);
    }


    // --- Input Handling & State Transitions ---
    // LEAD COMMENT: นี่คือส่วนของการ "เปลี่ยน State"
    // GroundedState จะคอยฟัง Input ที่ไม่เกี่ยวกับการเดิน
    // และเมื่อได้รับ, มันจะสั่งให้ PlayerController เปลี่ยนไป State ที่เหมาะสม

    public void OnJump()
    {
        // TODO: ยังไม่ทำใน Scope นี้ แต่ถ้าจะทำคือ
        // _playerController.SwitchState(_playerController.JumpingState);
        Debug.Log("Jump pressed, but JumpingState is not implemented yet.");
    }

    public void OnDash()
    {
        // เมื่อได้รับ Input การ Dash, สั่งเปลี่ยนไป DodgeState ทันที
        _playerController.SwitchState(_playerController.DodgeState);
    }

    public void OnLightAttack()
    {
        // LEAD COMMENT: ก่อนจะสลับไป AttackState เราต้องตั้งค่าประเภทการโจมตีก่อน
        // ค่า 0.6f คือความยาวของ Animation โจมตีเบา (คุณต้องปรับให้ตรงกับของคุณ)
        _playerController.AttackState.SetAttackType(false, 0.7f, 0.8f);
        _playerController.SwitchState(_playerController.AttackState);
    }

    public void OnHeavyAttack()
    {
        // LEAD COMMENT: ตั้งค่าให้เป็นการโจมตีหนัก
        // ค่า 1.2f คือความยาวของ Animation โจมตีหนัก (คุณต้องปรับให้ตรงกับของคุณ)
        _playerController.AttackState.SetAttackType(true, 0.7f, 0.8f);
        _playerController.SwitchState(_playerController.AttackState);
    }
}