using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LEAD COMMENT: นี่คือหัวใจของระบบ ScriptableObject Event
// มันคือ "ช่องสัญญาณ" ที่เราสามารถสร้างเป็นไฟล์ .asset ได้
// มันไม่ทำอะไรเลยนอกจาก "เก็บรายชื่อผู้รับฟัง" และ "ส่งข่าวสาร"
[CreateAssetMenu(fileName = "New Game Event", menuName = "Soul-like/Game Event", order = 2)]
public class GameEvent : ScriptableObject
{
    // LEAD COMMENT: เราใช้ List เก็บ GameEventListener ทั้งหมดที่กำลัง "เปิดวิทยุฟัง" ช่องนี้อยู่
    // การใช้ List ทำให้ผู้ฟังหนึ่งคน สามารถ "เลิกฟัง" ได้โดยไม่กระทบคนอื่น
    private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

    // ฟังก์ชันสำหรับ "ตะโกน" หรือ "ประกาศข่าวสาร"
    // ระบบอื่นจะเรียกใช้ฟังก์ชันนี้ (เช่น PlayerStatus.OnHealthChanged.Raise())
    public void Raise()
    {
        // เราจะวนลูปจากท้ายมาหน้าเสมอ ซึ่งเป็นเทคนิคป้องกัน Error
        // ในกรณีที่มีผู้ฟังคนใดคนหนึ่ง "เลิกฟัง" ทันทีที่ได้รับข่าว
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    // ฟังก์ชันสำหรับให้ "ผู้ฟัง" มาลงทะเบียน
    public void RegisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    // ฟังก์ชันสำหรับให้ "ผู้ฟัง" ยกเลิกการลงทะเบียน
    public void UnregisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}