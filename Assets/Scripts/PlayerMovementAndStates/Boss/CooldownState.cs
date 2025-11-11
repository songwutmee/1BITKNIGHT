using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownState : IState
{
    private readonly BossAIController _controller;
    private float _cooldownTimer;

    public CooldownState(BossAIController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        // เริ่มนับเวลาถอยหลัง โดยใช้เวลาตาม Phase ปัจจุบัน
        _cooldownTimer = _controller.CurrentCooldownTime;
    }

    public void Update()
    {
        _cooldownTimer -= Time.deltaTime;

        // เมื่อพักเสร็จ, กลับไปตัดสินใจใหม่ (อาจจะไล่ล่าต่อ หรือ โจมตีทันทีถ้าผู้เล่นยังอยู่ใกล้)
        if (_cooldownTimer <= 0f)
        {
            float distanceToPlayer = Vector3.Distance(_controller.transform.position, _controller.playerTarget.position);
            if (distanceToPlayer <= _controller.attackRange)
            {
                _controller.SwitchState(_controller.attackState);
            }
            else
            {
                _controller.SwitchState(_controller.chaseState);
            }
        }
    }
    public void Exit() { }
}
