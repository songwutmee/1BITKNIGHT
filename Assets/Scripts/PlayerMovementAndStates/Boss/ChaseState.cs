using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IState
{
    private readonly BossAIController _controller;

    public ChaseState(BossAIController controller)
    {
        _controller = controller;
    }

    public void Enter()
    {
        // เริ่มเคลื่อนที่
        _controller.agent.isStopped = false;
        _controller.agent.speed = _controller.CurrentChaseSpeed; // ใช้ความเร็วตาม Phase ปัจจุบัน
         _controller.animator.SetFloat("Speed", 1);
    }

    public void Update()
    {
        _controller.agent.SetDestination(_controller.playerTarget.position);

        float distanceToPlayer = Vector3.Distance(_controller.transform.position, _controller.playerTarget.position);

        // --- [เพิ่มใหม่] ---
        // LEAD COMMENT: นี่คือสายลับของเรา มันจะรายงานระยะห่างให้เราเห็นใน Console ทุกเฟรม
        Debug.Log("Distance to player: " + distanceToPlayer + "  |  Required Attack Range: " + _controller.attackRange);

        if (distanceToPlayer <= _controller.attackRange)
        {
            _controller.SwitchState(_controller.attackState);
        }
        else if (distanceToPlayer > _controller.aggroRange * 1.5f)
        {
            _controller.SwitchState(_controller.idleState);
        }
    }

    public void Exit()
    {
        // หยุดการเคลื่อนที่เมื่อออกจาก State นี้ (สำคัญมาก!)
        _controller.agent.isStopped = true;
    }
}
