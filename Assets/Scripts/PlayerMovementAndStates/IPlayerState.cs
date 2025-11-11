using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Enter();

    void Update();

    void Exit();

    void OnJump();
    void OnDash();
    void OnLightAttack();
    void OnHeavyAttack();
}
