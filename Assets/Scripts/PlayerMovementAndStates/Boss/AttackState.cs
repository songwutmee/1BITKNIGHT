using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private readonly BossAIController _controller;
    private readonly BossCombat _combat;

    // --- [เพิ่มใหม่] ---
    private readonly float _rotationSpeed = 5f; // ความเร็วในการหมุนตัวของบอส (ปรับค่านี้ได้)

    public AttackState(BossAIController controller, BossCombat combat)
    {
        _controller = controller;
        _combat = combat;
    }

    public void Enter()
    {
        _controller.agent.isStopped = true;
        _combat.PerformAttack();
    }

    public void Update()
    {
        // --- [อัปเกรด] ---
        // LEAD COMMENT: เราจะทำการ "หมุน" ใน Update() แทนที่จะทำใน Enter()
        // เพื่อให้การหมุนเกิดขึ้นอย่างต่อเนื่องตลอดช่วงเวลาที่กำลังจะโจมตี

        // 1. หาเป้าหมายทิศทางที่จะหันไป
        Vector3 directionToPlayer = (_controller.playerTarget.position - _controller.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // 2. ใช้ Slerp เพื่อหมุนอย่างนุ่มนวล
        _controller.transform.rotation = Quaternion.Slerp(
            _controller.transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime);

        // Logic เดิม: รอให้การโจมตีจบ
        if (!_combat.isAttacking)
        {
            _controller.SwitchState(_controller.cooldownState);
        }
    }

    public void Exit() { }
}