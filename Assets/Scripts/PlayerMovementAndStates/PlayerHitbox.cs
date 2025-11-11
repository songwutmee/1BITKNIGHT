using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float damage = 15f;
    private Collider _collider;

    // เราจะใช้ List เพื่อเก็บศัตรูที่โจมตีไปแล้วใน "การเหวี่ยงครั้งเดียว"
    // เพื่อป้องกันปัญหา Multi-hit ที่สมบูรณ์แบบที่สุด
    private List<Collider> _hitEnemies = new List<Collider>();

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    // LEAD COMMENT: OnEnable จะถูกเรียกทุกครั้งที่ SetActive(true)
    // เราจะใช้มันเพื่อ "รีเซ็ต" รายชื่อศัตรูที่เคยตีไปแล้ว
    void OnEnable()
    {
        _hitEnemies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        // เช็คว่าเคยตีศัตรูตัวนี้ไปแล้วหรือยังในการเหวี่ยงครั้งนี้
        if (_hitEnemies.Contains(other))
        {
            return; // ถ้าเคยตีแล้ว, ไม่ทำอะไร
        }

        // เรามองหา BossStatus โดยตรง เพราะในเกมเรามีศัตรูแค่ตัวเดียว
        if (other.TryGetComponent<BossStatus>(out BossStatus bossStatus))
        {
            Debug.Log("Player hit the Boss!");
            bossStatus.TakeDamage(damage);

            // เพิ่มศัตรูตัวนี้เข้าไปในลิสต์ เพื่อไม่ให้ตีซ้ำ
            _hitEnemies.Add(other);
        }
    }
}