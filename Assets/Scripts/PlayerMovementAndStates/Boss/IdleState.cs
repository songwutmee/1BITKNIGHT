using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private readonly BossAIController _controller;

    public IdleState(BossAIController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        // หยุดการเคลื่อนที่ทั้งหมด
        _controller.agent.isStopped = true;
         _controller.animator.SetFloat("Speed", 0);
    }

    public void Update()
    {
        // คำนวณระยะห่างจากผู้เล่น
        float distanceToPlayer = Vector3.Distance(_controller.transform.position, _controller.playerTarget.position);

        // ถ้าผู้เล่นเข้ามาในระยะ aggro, เปลี่ยนไป ChaseState
        if (distanceToPlayer <= _controller.aggroRange)
        {
            _controller.SwitchState(_controller.chaseState);
        }
    }

    public void Exit() { }
}
