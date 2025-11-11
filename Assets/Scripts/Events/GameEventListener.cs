using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

// LEAD COMMENT: นี่คือ Component อรรถประโยชน์ที่เราจะนำไป "แปะ" บน GameObject
// ที่ต้องการรอฟังเหตุการณ์ เช่น บน HealthBar_UI
// มันจะจัดการเรื่องการลงทะเบียนและยกเลิกการลงทะเบียนให้เราอัตโนมัติ
public class GameEventListener : MonoBehaviour
{
    [Tooltip("ช่องสัญญาณวิทยุ (GameEvent) ที่จะรอฟัง")]
    public GameEvent gameEvent;

    [Tooltip("จะให้ทำอะไรเมื่อได้รับสัญญาณ? (ลากฟังก์ชันมาวางที่นี่ได้เลย)")]
    public UnityEvent response; // LEAD COMMENT: UnityEvent คือสิ่งที่ทำให้เราสามารถลากวางฟังก์ชันใน Inspector ได้!

    // เมื่อ GameObject นี้ถูกเปิดใช้งาน, ให้ไปลงทะเบียนรอฟัง
    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    // เมื่อ GameObject นี้ถูกปิดใช้งาน, ให้ยกเลิกการรับฟัง
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    // ฟังก์ชันนี้จะถูกเรียกโดย GameEvent เมื่อมีการ Raise()
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
