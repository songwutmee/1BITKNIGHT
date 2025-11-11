using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private readonly PlayerController _playerController;
    
    private float _attackDuration;
    private float _stateTimer;
    private bool _isHeavyAttack;

    // --- [เพิ่มใหม่] ---
    [Header("Attack Targeting Settings")]
    private float _attackSearchRadius = 5f; // ระยะสูงสุดที่จะค้นหาศัตรูเพื่อหันไปหา
    private LayerMask _enemyLayer;

    public void SetAttackType(bool isHeavy, float lightAttackAnimTime, float heavyAttackAnimTime)
    {
        _isHeavyAttack = isHeavy;
        _attackDuration = isHeavy ? heavyAttackAnimTime : lightAttackAnimTime;
    }

    public PlayerAttackState(PlayerController playerController)
    {
        _playerController = playerController;
        // --- [เพิ่มใหม่] ---
        _enemyLayer = LayerMask.GetMask("Enemy"); // ดึง Layer "Enemy" มาใช้
    }

    public void Enter()
    {
        // --- [อัปเกรด] ---
        // LEAD COMMENT: นี่คือ Logic การ "ล็อคเป้าหมาย" ของเรา
        RotateTowardsClosestEnemy();

        float staminaCost = _isHeavyAttack ?
            _playerController.PlayerStatus.runtimeStats.heavyAttackStaminaCost :
            _playerController.PlayerStatus.runtimeStats.lightAttackStaminaCost;
        _playerController.PlayerStatus.UseStamina(staminaCost);

        _stateTimer = _attackDuration;

        if (_isHeavyAttack)
            _playerController.GetAnimator().SetTrigger(_playerController.HeavyAttackHash);
        else
            _playerController.GetAnimator().SetTrigger(_playerController.LightAttackHash);
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
    
    // --- [เพิ่มใหม่] ---
    private void RotateTowardsClosestEnemy()
    {
        // 1. ค้นหา Collider ทั้งหมดที่อยู่ใน Layer "Enemy" และอยู่ในรัศมีที่กำหนด
        Collider[] enemies = Physics.OverlapSphere(
            _playerController.transform.position,
            _attackSearchRadius,
            _enemyLayer
        );

        // 2. ถ้าเจอศัตรูอย่างน้อยหนึ่งตัว
        if (enemies.Length > 0)
        {
            // (ในเกมนี้เรารู้ว่ามีแค่ตัวเดียว เลยเอาตัวแรกได้เลย)
            Transform closestEnemy = enemies[0].transform;

            // 3. คำนวณทิศทางที่จะหันไปหา
            Vector3 directionToEnemy = (closestEnemy.position - _playerController.transform.position).normalized;

            // 4. "วาร์ป" การหมุนของตัวละครให้หันไปหาศัตรูทันทีในเฟรมแรก
            _playerController.transform.rotation = Quaternion.LookRotation(new Vector3(directionToEnemy.x, 0, directionToEnemy.z));
        }
        // ถ้าไม่เจอศัตรูในระยะ, ก็จะโจมตีไปข้างหน้าที่หันอยู่ตามปกติ
    }
    
    // --- Input Handling ---
    public void OnJump() { }
    public void OnDash() { }
    public void OnLightAttack() { }
    public void OnHeavyAttack() { }
}