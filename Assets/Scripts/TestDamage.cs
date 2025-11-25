using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: เราได้อัปเกรด Script นี้ให้ทำงานในโหมด "Trigger"
// ซึ่งจะตรวจจับว่ามี Collider อื่น "เข้ามา" ในพื้นที่ของมันหรือไม่
// โดยไม่จำเป็นต้องมีการชนกันทางกายภาพ
public class TestDamage : MonoBehaviour
{
    [Header("Settings")]
    public float damageAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the damage trigger!");

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