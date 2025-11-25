using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitState : IPlayerState
{
    private readonly PlayerController _playerController;
    private readonly float _getHitDuration = 0.5f; 
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
        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0f)
        {
            _playerController.SwitchState(_playerController.GroundedState);
        }
    }

    public void Exit() { }

    public void OnJump() { }
    public void OnDash() { }
    public void OnLightAttack() { }
    public void OnHeavyAttack() { }
}

