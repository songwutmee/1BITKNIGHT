using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossHitbox : MonoBehaviour
{
    public LayerMask targetLayer;
    public float damage = 20f;
    private Collider _collider;
    void Awake()
    {
        // Cache collider ของตัวเองไว้
        _collider = GetComponent<Collider>();
        // ตรวจสอบให้แน่ใจว่ามันเป็น Trigger
        if (!_collider.isTrigger)
        {
            Debug.LogError("'" + gameObject.name + "' is missing 'Is Trigger' setting on its Collider!", gameObject);
        }
    }

    // --- [เพิ่มใหม่] ---
    // ฟังก์ชันนี้จะถูกเรียกโดย BossCombat เพื่อให้เราสามารถ Log การเปิด/ปิดได้
    public void SetActive(bool active)
    {
        if (_collider != null)
        {
            _collider.enabled = active;
            Debug.Log("'" + gameObject.name + "' Collider state set to: " + active, gameObject);
        }
    }

    // --- [อัปเกรด] ---
    private void OnTriggerEnter(Collider other)
    {
        // สายลับ #1: รายงานทุกอย่างที่เข้ามาชน
        Debug.Log("'" + gameObject.name + "' triggered with '" + other.name + "' on layer " + LayerMask.LayerToName(other.gameObject.layer), other.gameObject);

        // เช็ค Layer
        if (((1 << other.gameObject.layer) & targetLayer.value) != 0)
        {
            // สายลับ #2: รายงานว่าชนถูก Layer เป้าหมายแล้ว
            Debug.Log("Correct Target Layer ('" + LayerMask.LayerToName(other.gameObject.layer) + "') detected!");

            if (other.TryGetComponent<PlayerStatus>(out PlayerStatus playerStatus))
            {
                // สายลับ #3: รายงานว่าเจอ PlayerStatus และกำลังจะทำ Damage
                Debug.Log("PlayerStatus found! Dealing " + damage + " damage.");
                playerStatus.TakeDamage(damage);

                // ปิด Collider ทันทีที่ทำงานเสร็จ
                _collider.enabled = false;
            }
        }
    }
}