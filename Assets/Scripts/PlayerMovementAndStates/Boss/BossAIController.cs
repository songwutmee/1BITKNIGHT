using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(BossStatus), typeof(BossCombat))] // เพิ่ม BossCombat
public class BossAIController : MonoBehaviour
{
    [Header("Core Dependencies")]
    public Transform playerTarget;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public BossStatus status;
    [HideInInspector] public Animator animator;
    [HideInInspector] public BossCombat combat; // [เพิ่มใหม่]

    // --- State Machine ---
    private IState _currentState;
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState; // [ปลดคอมเมนต์]
    public CooldownState cooldownState; // [ปลดคอมเมนต์]

    [Header("AI Parameters")]
    public float aggroRange = 15f;
    public float attackRange = 3f;

    // ... (Phase Logic & Properties เหมือนเดิม) ...
    [Header("Phase Logic")]
    public int currentPhase = 1;
    public float phase2HealthThreshold = 0.5f;
    [Header("Phase 1 Settings")]
    public float phase1_ChaseSpeed = 3.5f;
    public float phase1_CooldownTime = 2.0f;
    [Header("Phase 2 Settings")]
    public float phase2_ChaseSpeed = 5.0f;
    public float phase2_CooldownTime = 1.0f;
    public float CurrentChaseSpeed => currentPhase == 1 ? phase1_ChaseSpeed : phase2_ChaseSpeed;
    public float CurrentCooldownTime => currentPhase == 1 ? phase1_CooldownTime : phase2_CooldownTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<BossStatus>();
        animator = GetComponent<Animator>();
        combat = GetComponent<BossCombat>(); // [เพิ่มใหม่] Cache BossCombat

        // --- สร้าง Instance ของ State ทั้งหมด ---
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this, combat); // [อัปเกรด] ส่ง BossCombat เข้าไปด้วย
        cooldownState = new CooldownState(this);
    }

    // ... (Start, Update, SwitchState, SwitchToPhase2 เหมือนเดิม) ...
    void Start()
    {
        SwitchState(idleState);
    }

    void Update()
    {
        _currentState?.Update();

        if (currentPhase == 1 && status.CurrentHealth / status.baseStats.maxHealth <= phase2HealthThreshold)
        {
            SwitchToPhase2();
        }
    }

    public void SwitchState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    private void SwitchToPhase2()
    {
        currentPhase = 2;
        Debug.LogWarning("BOSS HAS ENTERED PHASE 2!");
    }
}