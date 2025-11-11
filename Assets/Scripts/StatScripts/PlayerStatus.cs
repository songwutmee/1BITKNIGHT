using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerStatus : MonoBehaviour
{
    [Header("Data Source")]
    [Tooltip("ลากไฟล์ 'พิมพ์เขียว' CharacterStats_SO ที่เป็น .asset มาใส่ที่นี่")]
    public CharacterStats_SO baseStats; // พิมพ์เขียวต้นฉบับ

    [Header("Event Channels")]
    public GameEvent onHealthChanged;
    public GameEvent onStaminaChanged;

    // --- [อัปเกรด] ---
    // LEAD COMMENT: นี่คือ "สำเนา" ของค่าพลังที่เราจะใช้ตอนเล่นเกมจริงๆ
    // เราจะซ่อนมันไว้เป็น public (เพื่อให้ Debug ง่าย) แต่จะมี [HideInInspector]
    // เพื่อไม่ให้รกในหน้าต่าง Inspector ปกติ
    [HideInInspector]
    public CharacterStats_SO runtimeStats; // สำเนาสำหรับใช้งาน

    // --- Private runtime variables ---
    private float _currentHealth;
    private float _currentStamina;

    private PlayerController _playerController;

    public float GetCurrentHealth() => _currentHealth;
    public float GetCurrentStamina() => _currentStamina;


    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();

        // --- [อัปเกรด] ---
        // LEAD COMMENT: นี่คือหัวใจของ Instancing Pattern
        // เราทำการ "โคลนนิ่ง" baseStats มาเก็บไว้ใน runtimeStats
        // ตอนนี้เราสามารถแก้ไข runtimeStats ได้อย่างอิสระโดยไม่กระทบไฟล์ต้นฉบับ
        runtimeStats = Instantiate(baseStats);
    }

    private void Start()
    {
        // --- [อัปเกรด] ---
        // เปลี่ยนจากการอ่าน baseStats มาเป็น runtimeStats ทั้งหมด
        _currentHealth = runtimeStats.maxHealth;
        _currentStamina = runtimeStats.maxStamina;

        if (onHealthChanged != null) onHealthChanged.Raise();
        if (onStaminaChanged != null) onStaminaChanged.Raise();
    }

    private void Update()
    {
        float oldStamina = _currentStamina;
        // --- [อัปเกรด] ---
        if (_currentStamina < runtimeStats.maxStamina)
        {
            _currentStamina += runtimeStats.staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, runtimeStats.maxStamina);
        }

        if (oldStamina != _currentStamina && onStaminaChanged != null)
        {
            onStaminaChanged.Raise();
        }
    }

    public bool HasEnoughStamina(float amount)
    {
        return _currentStamina >= amount;
    }

    public void UseStamina(float amount)
    {
        if (HasEnoughStamina(amount))
        {
            _currentStamina -= amount;
            if (onStaminaChanged != null) onStaminaChanged.Raise();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        _playerController.PlayGetHitAnimation();

        if (onHealthChanged != null) onHealthChanged.Raise();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    // --- [เพิ่มใหม่] ---
    #region Stat Upgrades
    // LEAD COMMENT: นี่คือ "ประตู" ที่เราสร้างขึ้นสำหรับ SkillTreeManager
    // ฟังก์ชัน Public เหล่านี้คือวิธีเดียวที่ระบบภายนอกจะสามารถอัปเกรดค่าพลังของผู้เล่นได้
    // ซึ่งเป็นการรักษาหลัก Encapsulation ได้อย่างดีเยี่ยม

    public void UpgradeMaxHealth(float amount)
    {
        runtimeStats.maxHealth += amount;
        _currentHealth += amount; // เพิ่มเลือดปัจจุบันด้วย เพื่อให้เห็นผลทันที
        if (onHealthChanged != null) onHealthChanged.Raise();
        Debug.Log("Max Health upgraded to: " + runtimeStats.maxHealth);
    }

    public void UpgradeMaxStamina(float amount)
    {
        runtimeStats.maxStamina += amount;
        _currentStamina += amount;
        if (onStaminaChanged != null) onStaminaChanged.Raise();
        Debug.Log("Max Stamina upgraded to: " + runtimeStats.maxStamina);
    }

    public void UpgradeStaminaRegen(float amount)
    {
        runtimeStats.staminaRegenRate += amount;
        Debug.Log("Stamina Regen upgraded to: " + runtimeStats.staminaRegenRate);
    }

    #endregion

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}