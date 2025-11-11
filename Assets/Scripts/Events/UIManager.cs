using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // สำคัญมาก! ต้องมีเพื่อเข้าถึง Slider

// LEAD COMMENT: Script นี้จะจัดการ UI ทั้งหมด และจะทำหน้าที่เป็น "ผู้ฟัง" (Listener)
// มันไม่ต้องรู้จัก PlayerStatus เลยแม้แต่น้อย มันแค่ต้องรู้จัก Player เท่านั้น
// เพื่อที่จะได้ไปดึงค่าพลัง "ปัจจุบัน" มาแสดงผลเมื่อได้รับสัญญาณ
public class UIManager : MonoBehaviour
{
    [Header("Player References")]
    [Tooltip("ลาก GameObject Player มาใส่ที่นี่")]
    public PlayerStatus playerStatus; // เรายังต้องมี reference เพื่อไป "ถาม" ว่าค่าปัจจุบันคือเท่าไหร่

    [Header("UI Elements")]
    [Tooltip("ลาก Slider ที่เป็น HealthBar มาใส่ที่นี่")]
    public Slider healthBar;

    [Tooltip("ลาก Slider ที่เป็น StaminaBar มาใส่ที่นี่")]
    public Slider staminaBar;

    private void Start()
    {
        // LEAD COMMENT: เมื่อเกมเริ่ม เราจะตั้งค่า MaxValue ของ Slider
        // ให้ตรงกับค่าพลังสูงสุดของผู้เล่นก่อน
        // นี่คือเหตุผลที่เรายังต้องมี reference ไปยัง PlayerStatus
        if (playerStatus != null)
        {
            healthBar.maxValue = playerStatus.baseStats.maxHealth;
            staminaBar.maxValue = playerStatus.baseStats.maxStamina;
            UpdateHealth(); // อัปเดตครั้งแรกเพื่อให้แถบเต็ม
            UpdateStamina();
        }
    }

    // LEAD COMMENT: นี่คือฟังก์ชัน Public ที่เราจะให้ "เครื่องรับวิทยุ" (GameEventListener) เรียกใช้
    // เมื่อได้รับสัญญาณจากช่อง OnPlayerHealthChanged
    public void UpdateHealth()
    {
        if (playerStatus != null)
        {
            // ไป "ถาม" PlayerStatus ว่าเลือดปัจจุบันเหลือเท่าไหร่ แล้วเอามาตั้งค่าให้ Slider
            healthBar.value = playerStatus.GetCurrentHealth();
        }
    }

    // ฟังก์ชันสำหรับ Stamina
    public void UpdateStamina()
    {
        if (playerStatus != null)
        {
            staminaBar.value = playerStatus.GetCurrentStamina();
        }
    }
}