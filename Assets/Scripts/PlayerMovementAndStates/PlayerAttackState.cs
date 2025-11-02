using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: State นี้จัดการ Logic การโจมตีทั้งหมด
// หน้าที่ของมันคือ:
// 1. เลือกว่าจะเล่น Animation โจมตีเบา หรือ หนัก
// 2. ล็อคการเคลื่อนที่และการกระทำอื่นๆ ขณะโจมตี
// 3. เมื่อ Animation จบ ให้กลับสู่ GroundedState
// 4. (อนาคต) ดักจับ Input เพื่อทำคอมโบต่อเนื่อง
public class PlayerAttackState : IPlayerState
{
    private readonly PlayerController _playerController;

    // --- State-Specific Parameters ---
    private float _attackDuration;
    private float _stateTimer;
    private bool _isHeavyAttack;

    // LEAD COMMENT: เราจะสร้างตัวแปร public เพื่อให้ State อื่นสามารถกำหนดประเภทการโจมตีก่อนจะสลับมา State นี้ได้
    // นี่เป็นวิธีที่ง่ายและสะอาดในการส่งข้อมูลระหว่าง State
    public void SetAttackType(bool isHeavy, float lightAttackAnimTime, float heavyAttackAnimTime)
    {
        _isHeavyAttack = isHeavy;
        _attackDuration = isHeavy ? heavyAttackAnimTime : lightAttackAnimTime;
    }

    public PlayerAttackState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("SUCCESS! Switched to AttackState. Triggering animation now...");
        Debug.Log("Entering Attack State (Heavy: " + _isHeavyAttack + ")");
        _stateTimer = _attackDuration;

        // LEAD COMMENT: เลือก Trigger Animation ที่จะเล่นตามประเภทการโจมตีที่ตั้งค่าไว้
        if (_isHeavyAttack)
        {
            _playerController.GetAnimator().SetTrigger(_playerController.HeavyAttackHash);
        }
        else
        {
            _playerController.GetAnimator().SetTrigger(_playerController.LightAttackHash);
        }

        // (ส่วนนี้ยังไม่เขียน แต่เป็นจุดที่จะเพิ่ม Logic ในอนาคต)
        // PlayerStatus.UseStamina(attackStaminaCost);
        // EnableHitbox(); // เปิดพื้นที่การโจมตีเพื่อสร้างความเสียหาย
    }

    public void Update()
    {
        // นับเวลาถอยหลังเหมือน DodgeState
        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0f)
        {
            _playerController.SwitchState(_playerController.GroundedState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Attack State");

        // (ส่วนนี้ยังไม่เขียน แต่เป็นจุดที่จะเพิ่ม Logic ในอนาคต)
        // DisableHitbox(); // ปิดพื้นที่การโจมตี ป้องกันดาเมจค้าง
    }

    // --- Input Handling for Future Combo System ---
    public void OnLightAttack()
    {
        // LEAD COMMENT: สำหรับตอนนี้เรายังไม่ทำคอมโบ
        // แต่ในอนาคต เราสามารถเช็คได้ที่นี่ว่าถ้าผู้เล่นกดโจมตีอีกครั้ง
        // ในช่วงท้ายๆ ของ Animation ปัจจุบัน ให้เริ่มการโจมตีครั้งต่อไปเลย
        // เช่น if (_canCombo) { ResetTimerAndPlayNextAttack(); }
    }

    // ปล่อยว่างเพื่อล็อคการควบคุม
    public void OnJump() { }
    public void OnDash() { }
    public void OnHeavyAttack() { }
}