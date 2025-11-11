using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: [CreateAssetMenu] คือ Attribute มหัศจรรย์ที่ทำให้เราสามารถสร้าง "ไฟล์ข้อมูล"
// จาก Script นี้ได้โดยตรงผ่านเมนู Assets > Create ของ Unity
// นี่คือหัวใจของ Data-Oriented Architecture
[CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Soul-like/Character Stats", order = 1)]
public class CharacterStats_SO : ScriptableObject
{
    [Header("Core Stats")]
    [Tooltip("พลังชีวิตสูงสุด")]
    public float maxHealth = 100f;

    [Tooltip("พลังความแข็งแกร่งสูงสุด")]
    public float maxStamina = 100f;

    [Header("Stamina Costs & Regeneration")]
    [Tooltip("อัตราการฟื้นฟู Stamina ต่อวินาที")]
    public float staminaRegenRate = 10f;

    [Tooltip("Stamina ที่ใช้ในการกลิ้งหลบ")]
    public float dodgeStaminaCost = 15f;

    [Tooltip("Stamina ที่ใช้ในการโจมตีเบา")]
    public float lightAttackStaminaCost = 10f;

    [Tooltip("Stamina ที่ใช้ในการโจมตีหนัก")]
    public float heavyAttackStaminaCost = 25f;

    // LEAD COMMENT: เราสามารถเพิ่มค่าพลังอื่นๆ ได้ไม่จำกัดในอนาคต เช่น
    // public float attackPower = 10f;
    // public float defensePower = 5f;
    // โดยที่ไม่ต้องไปยุ่งกับโค้ด Logic เลย
}