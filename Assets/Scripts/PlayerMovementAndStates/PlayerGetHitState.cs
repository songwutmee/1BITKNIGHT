using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: State นี้จะถูกเรียกใช้เมื่อผู้เล่นได้รับความเสียหาย
// หน้าที่หลักของมันคือการ "ขัดจังหวะ" การกระทำของผู้เล่นชั่วขณะ (Stagger)
// เพื่อสร้างความรู้สึกว่าการโจมตีของศัตรูนั้น "มีน้ำหนัก"
public class PlayerGetHitState : IPlayerState
{
    private readonly PlayerController _playerController;
    private readonly float _getHitDuration = 0.5f; // ระยะเวลาที่จะถูกล็อคควบคุม (ปรับให้ตรงกับ Animation)
    private float _stateTimer;

    public PlayerGetHitState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter()
    {
        _stateTimer = _getHitDuration;
        _playerController.GetAnimator().SetTrigger(_playerController.GetHitHash);
    }

    public void Update()
    {
        // เมื่อเวลาล็อคควบคุมหมดลง ให้กลับไปที่ GroundedState
        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0f)
        {
            _playerController.SwitchState(_playerController.GroundedState);
        }
    }

    public void Exit() { }

    // ปล่อยว่างทั้งหมดเพื่อ "ล็อค" การควบคุมของผู้เล่น
    public void OnJump() { }
    public void OnDash() { }
    public void OnLightAttack() { }
    public void OnHeavyAttack() { }
}
