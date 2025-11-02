using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerInput _playerInput;

    // --- [วิธีใหม่] สร้าง Action สำหรับคลิกซ้ายโดยตรงในโค้ด ---
    // นี่คือการ "ต่อสายตรง" ของเรา
    private InputAction _lightClickAction;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerInput = GetComponent<PlayerInput>();

        // --- [วิธีใหม่] กำหนดค่าและเตรียม "สายไฟ" ของเรา ---
        // 1. สร้าง Action ขึ้นมาใหม่ บอกว่าเราสนใจการคลิกเมาส์ซ้ายโดยเฉพาะ
        _lightClickAction = new InputAction(binding: "<Mouse>/leftButton");

        // 2. บอกว่าถ้า Action นี้เกิดขึ้น (performed), ให้ไปเรียกฟังก์ชัน OnLightAttackPerformed
        // สังเกตว่าเราไม่ได้ใช้ context จาก PlayerInput แต่จะสร้างของเราเอง
        _lightClickAction.performed += OnLightAttackPerformed;
    }

    private void OnEnable()
    {
        // --- [วิธีใหม่] เปิดใช้งาน "สายไฟตรง" ของเรา ---
        _lightClickAction.Enable();

        // --- [วิธีเก่า] เรายังคงใช้ระบบเดิมสำหรับ Action อื่นๆ ที่ทำงานได้ดีอยู่แล้ว ---
        _playerInput.actions["HeavyAttack"].performed += OnHeavyAttack;
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Jump"].performed += OnJump;
        _playerInput.actions["Dash"].performed += OnDash;
    }

    private void OnDisable()
    {
        // --- [วิธีใหม่] ปิดใช้งานและเก็บกวาด "สายไฟตรง" ของเรา ---
        _lightClickAction.Disable();

        // --- [วิธีเก่า] ยกเลิกการรับฟังของ Action อื่นๆ ---
        _playerInput.actions["HeavyAttack"].performed -= OnHeavyAttack;
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Jump"].performed -= OnJump;
        _playerInput.actions["Dash"].performed -= OnDash;
    }

    // --- [วิธีใหม่] ฟังก์ชันที่จะถูกเรียกโดยตรงจาก _lightClickAction ของเรา ---
    private void OnLightAttackPerformed(InputAction.CallbackContext context)
    {
        // เราส่ง context ที่ได้รับมา ไปให้ PlayerController เหมือนเดิมเป๊ะๆ
        _playerController.OnLightAttack(context);
    }

    // --- ฟังก์ชันตัวกลางสำหรับ Action อื่นๆ ยังคงเหมือนเดิม ---
    private void OnMove(InputAction.CallbackContext context)
    {
        _playerController.OnMove(context);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _playerController.OnJump(context);
    }

    private void OnHeavyAttack(InputAction.CallbackContext context)
    {
        _playerController.OnHeavyAttack(context);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        _playerController.OnDash(context);
    }
}
