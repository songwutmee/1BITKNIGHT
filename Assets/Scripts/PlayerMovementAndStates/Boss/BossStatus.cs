using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: Script นี้มีโครงสร้างคล้ายกับ PlayerStatus มาก
// แต่ถูกปรับให้เรียบง่ายขึ้นสำหรับศัตรู และเพิ่มการยิง Event ใหม่ๆ เข้าไป
public class BossStatus : MonoBehaviour
{
    [Header("Data Source")]
    public CharacterStats_SO baseStats; // ลากไฟล์ Minotaur_Stats.asset มาใส่ที่นี่

    [Header("Event Channels")]
    public GameEvent onBossDamaged;  // Event สำหรับอัปเดต Health Bar ของบอส
    public GameEvent onCameraShake;  // Event สำหรับสั่งให้กล้องสั่น

    private float _currentHealth;
    public float CurrentHealth => _currentHealth;

    void Start()
    {
        _currentHealth = baseStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_currentHealth <= 0) return; // ไม่ทำอะไรถ้าตายไปแล้ว

        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        Debug.Log("Boss took " + damage + " damage. Current Health: " + _currentHealth);

        // --- Event-Driven Communication ---
        // 1. "ประกาศ" บอก UI ว่าเลือดของฉันเปลี่ยนแล้วนะ!
        if (onBossDamaged != null)
        {
            onBossDamaged.Raise();
        }

        // 2. "ประกาศ" บอก Camera Manager ว่า "ฉันเพิ่งโดนตี! ช่วยสั่นกล้องที!"
        if (onCameraShake != null)
        {
            onCameraShake.Raise();
        }
        // --- End of Communication ---

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss has been defeated!");
        // TODO: สั่ง Animator ให้เล่นท่าตาย
        // GetComponent<Animator>().SetTrigger("Death");
        // อาจจะปิดการทำงานของ AI Controller
        // GetComponent<BossAIController>().enabled = false;
    }
}
