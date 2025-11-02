using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    // ฟังก์ชันที่จะถูกเรียกเมื่อเข้ามาใน State นี้
    void Enter();

    // ฟังก์ชันที่จะถูกเรียกทุกๆ เฟรม ตราบใดที่ยังอยู่ใน State นี้
    void Update();

    // ฟังก์ชันที่จะถูกเรียกเมื่อออกจาก State นี้
    void Exit();

    // ฟังก์ชันสำหรับรับ Input ที่ส่งมาจาก PlayerController
    void OnJump();
    void OnDash();
    void OnLightAttack();
    void OnHeavyAttack();
}
