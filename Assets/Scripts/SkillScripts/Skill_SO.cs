using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: [CreateAssetMenu] คือหัวใจสำคัญ มันจะเพิ่มเมนูใหม่ใน Assets > Create
// ทำให้เราสามารถสร้าง "ไฟล์ข้อมูลสกิล" ได้โดยตรงจาก Editor
[CreateAssetMenu(fileName = "New Skill", menuName = "Soul-like/Skill", order = 3)]

public class Skill_SO : ScriptableObject
{
    [Header("Skill Information")]
    public string skillName;

    // LEAD COMMENT: [TextArea] ทำให้ช่อง Description ใน Inspector ขยายได้
    // เหมาะสำหรับคำอธิบายยาวๆ
    [TextArea(3, 10)]
    public string description;
    public Sprite icon;

    [Header("Skill Cost & Requirements")]
    public int cost; // แต้มที่ต้องใช้ในการปลดล็อก

    // LEAD COMMENT: นี่คือการสร้าง "ความสัมพันธ์" ระหว่างสกิล
    // เราสามารถลาก Skill_SO .asset อื่นๆ มาใส่ในนี้เพื่อกำหนดว่า
    // "ต้องอัปเกรดสกิลเหล่านี้ก่อน ถึงจะอัปเกรดสกิลนี้ได้"
    public List<Skill_SO> requiredSkills;

    [Header("Skill Effect")]
    // LEAD COMMENT: Enum คือเครื่องมือที่ทรงพลังมากในการกำหนด "ประเภท" ของสิ่งต่างๆ
    // มันจะกลายเป็น Dropdown menu สวยๆ ใน Inspector ทำให้เราเลือกได้ง่ายและป้องกันการพิมพ์ผิด
    public StatType statToUpgrade;
    public float upgradeValue;

    // LEAD COMMENT: เรานิยามประเภทของ Stat ที่สามารถอัปเกรดได้ไว้ที่นี่
    // ถ้าในอนาคตมี Stat ใหม่ๆ เพิ่มขึ้น (เช่น Attack Power) เราก็แค่มาเพิ่มใน enum นี้
    // ระบบทั้งหมดก็จะรองรับมันโดยอัตโนมัติ! นี่คือการวางแผนเพื่ออนาคต
    public enum StatType
    {
        MaxHealth,
        MaxStamina,
        StaminaRegenRate,
        AttackPower, // เพิ่มไว้เพื่ออนาคต
        Defense     // เพิ่มไว้เพื่ออนาคต
    }

    // LEAD COMMENT: คำถามสำคัญ: "แล้ว isUnlocked (ปลดล็อกหรือยัง) ล่ะ?"
    // เรา "จงใจ" ไม่ใส่ตัวแปร isUnlocked ไว้ในนี้ครับ! เพราะนี่คือ "พิมพ์เขียว"
    // ถ้าเราใส่ isUnlocked ไว้ที่นี่ มันจะหมายความว่าถ้าปลดล็อกสกิลนี้ ทุกคน (รวมถึงศัตรู)
    // ก็จะปลดล็อกสกิลนี้ไปด้วย! สถานะการปลดล็อกเป็น "ข้อมูลของผู้เล่น"
    // ซึ่งจะถูกจัดการโดย SkillTreeManager หรือ PlayerData ในอนาคต
}
