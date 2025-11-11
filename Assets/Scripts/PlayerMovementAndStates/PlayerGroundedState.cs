using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : IPlayerState
{
    private readonly PlayerController _playerController;

    public PlayerGroundedState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter() { }

    public void Update()
    {
        Vector2 moveInput = _playerController.MoveInput;
        if (moveInput.sqrMagnitude > 0.01f)
        {
            _playerController.MoveCharacter();
            _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, moveInput.magnitude);
        }
        else
        {
            _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, 0f);
        }
    }

    public void Exit()
    {
        _playerController.GetAnimator().SetFloat(_playerController.SpeedHash, 0f);
    }

    public void OnJump()
    {
        Debug.Log("Jump pressed, but JumpingState is not implemented yet.");
    }

    public void OnDash()
    {
        // --- [อัปเกรด] ---
        // ดึงค่า Stamina ที่ต้องใช้มาจาก ScriptableObject
        float staminaCost = _playerController.PlayerStatus.baseStats.dodgeStaminaCost;
        // ตรวจสอบว่ามี Stamina เพียงพอหรือไม่
        if (_playerController.PlayerStatus.HasEnoughStamina(staminaCost))
        {
            _playerController.SwitchState(_playerController.DodgeState);
        }
        else
        {
            Debug.Log("Not enough stamina to dodge!");
        }
    }

    public void OnLightAttack()
    {
        // --- [อัปเกรด] ---
        float staminaCost = _playerController.PlayerStatus.baseStats.lightAttackStaminaCost;
        if (_playerController.PlayerStatus.HasEnoughStamina(staminaCost))
        {
            _playerController.AttackState.SetAttackType(false, 0.7f, 0.8f);
            _playerController.SwitchState(_playerController.AttackState);
        }
        else
        {
            Debug.Log("Not enough stamina for light attack!");
        }
    }

    public void OnHeavyAttack()
    {
        // --- [อัปเกรด] ---
        float staminaCost = _playerController.PlayerStatus.baseStats.heavyAttackStaminaCost;
        if (_playerController.PlayerStatus.HasEnoughStamina(staminaCost))
        {
            _playerController.AttackState.SetAttackType(true, 0.7f, 0.8f);
            _playerController.SwitchState(_playerController.AttackState);
        }
        else
        {
            Debug.Log("Not enough stamina for heavy attack!");
        }
    }
}