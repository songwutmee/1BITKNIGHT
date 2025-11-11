using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: Script นี้คือ "ผู้จัดการการต่อสู้" ของฝั่งผู้เล่น
// รับผิดชอบแค่เรื่องการเปิด/ปิด Hitbox ตามคำสั่งจาก Animation Events
[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("ลาก GameObject ที่เป็น Hitbox ของดาบมาใส่ที่นี่")]
    public PlayerHitbox weaponHitbox;

    void Start()
    {
        // ตรวจสอบและปิด Hitbox ไว้ก่อนเมื่อเริ่มเกม
        if (weaponHitbox == null)
        {
            Debug.LogError("Player's Weapon Hitbox is not assigned in PlayerCombat!", this.gameObject);
        }
        else
        {
            weaponHitbox.gameObject.SetActive(false);
        }
    }

    // --- Animation Event Functions ---
    // LEAD COMMENT: ฟังก์ชัน Public ที่จะถูกเรียกโดย Animation Events บน Clip ท่าโจมตีของผู้เล่น
    public void EnableWeaponHitbox()
    {
        if (weaponHitbox != null)
        {
            // เราจะใช้ SetActive เพราะมันจัดการกับ Awake() ได้ดีกว่า
            weaponHitbox.gameObject.SetActive(true);
        }
    }

    public void DisableWeaponHitbox()
    {
        if (weaponHitbox != null)
        {
            weaponHitbox.gameObject.SetActive(false);
        }
    }
}