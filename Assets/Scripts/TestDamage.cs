using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: เราได้อัปเกรด Script นี้ให้ทำงานในโหมด "Trigger"
// ซึ่งจะตรวจจับว่ามี Collider อื่น "เข้ามา" ในพื้นที่ของมันหรือไม่
// โดยไม่จำเป็นต้องมีการชนกันทางกายภาพ
public class TestDamage : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("พลังโจมตีของเซ็นเซอร์นี้")]
    public float damageAmount = 10f;

    // LEAD COMMENT: เราเปลี่ยนมาใช้ OnTriggerEnter ซึ่งเป็นฟังก์ชันพิเศษของ Unity
    // ที่จะถูกเรียกโดยอัตโนมัติเมื่อ Is Trigger ถูกติ๊ก และมี Collider อื่นเข้ามา
    // Parameter ของมันคือ Collider other (ไม่ใช่ Collision collision)
    private void OnTriggerEnter(Collider other)
    {
        // LEAD COMMENT: เรายังคงเช็ค Tag "Player" เหมือนเดิม เพื่อให้แน่ใจว่า
        // เราสร้างความเสียหายให้กับผู้เล่นเท่านั้น
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the damage trigger!");

            // LEAD COMMENT: Logic ส่วนที่เหลือเหมือนเดิมทุกประการ
            // เราดึง PlayerStatus มาจาก Collider ที่ "other" เข้ามา
            PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();

            if (playerStatus != null)
            {
                playerStatus.TakeDamage(damageAmount);
                Debug.Log("Dealt " + damageAmount + " damage to the player via Trigger.");
            }
            else
            {
                Debug.LogWarning("Player entered Trigger, but PlayerStatus component was not found!");
            }
        }
    }
}