using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // เพิ่มเข้ามาเพื่อใช้ IPointerEnterHandler, IPointerExitHandler

// LEAD COMMENT: นี่คือเวอร์ชันอัปเกรดที่ใช้ Event System ของ Unity
// เพื่อจัดการสถานะ Hover โดยตรง ทำให้โค้ดสะอาดและตอบสนองได้ดีขึ้น
public class SkillButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Data")]
    [HideInInspector] public Skill_SO skillData;
    [HideInInspector] public SkillTreeManager skillTreeManager;

    [Header("UI Elements")]
    public Image iconImage;
    public Image frameImage;
    public Button button;

    [Header("Visual States Colors")]
    public Color unlockedColor = Color.white;
    public Color unlockableFrameColor = Color.green;
    public Color lockedColor = Color.gray;

    // [เพิ่มใหม่] ตัวแปรสำหรับเก็บสถานะปัจจุบันของปุ่ม
    private SkillState _currentState;

    public void Initialize(Skill_SO skill, SkillTreeManager manager)
    {
        skillData = skill;
        skillTreeManager = manager;
        iconImage.sprite = skill.icon;
        button.onClick.AddListener(() => skillTreeManager.UnlockSkill(skillData));
    }

    public void UpdateVisuals(List<Skill_SO> unlockedSkills, int currentSkillPoints)
    {
        bool isUnlocked = unlockedSkills.Contains(skillData);
        if (isUnlocked)
        {
            SetState(SkillState.Unlocked);
            return;
        }

        bool hasEnoughPoints = currentSkillPoints >= skillData.cost;
        bool requirementsMet = true;
        foreach (var requiredSkill in skillData.requiredSkills)
        {
            if (!unlockedSkills.Contains(requiredSkill))
            {
                requirementsMet = false;
                break;
            }
        }

        if (hasEnoughPoints && requirementsMet)
        {
            SetState(SkillState.Unlockable);
        }
        else
        {
            SetState(SkillState.Locked);
        }
    }

    private void SetState(SkillState newState)
    {
        _currentState = newState;
        // ตั้งค่าสีพื้นฐานตามสถานะ
        switch (_currentState)
        {
            case SkillState.Unlocked:
                iconImage.color = unlockedColor;
                frameImage.color = unlockedColor;
                button.interactable = false;
                break;
            case SkillState.Unlockable:
                iconImage.color = lockedColor;
                frameImage.color = unlockableFrameColor; // กรอบเป็นสีพิเศษ
                button.interactable = true;
                break;
            case SkillState.Locked:
                iconImage.color = lockedColor;
                frameImage.color = lockedColor;
                button.interactable = false;
                break;
        }
    }

    // --- [เพิ่มใหม่] Event Handlers สำหรับ Hover ---
    // LEAD COMMENT: ฟังก์ชันนี้จะถูกเรียกโดย Event System ของ Unity "โดยอัตโนมัติ"
    // เมื่อเมาส์เคลื่อนที่เข้ามา "เหนือ" UI Element นี้
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ถ้าปุ่มอยู่ในสถานะ "พร้อมปลดล็อก" เท่านั้น
        if (_currentState == SkillState.Unlockable)
        {
            // ให้เปลี่ยนสีไอคอนเป็นสีเต็ม เพื่อส่งสัญญาณว่า "กดฉันสิ!"
            iconImage.color = unlockedColor;
        }
        // TODO: เพิ่ม Logic การแสดง Tooltip ที่นี่
    }

    // LEAD COMMENT: ฟังก์ชันนี้จะถูกเรียก "โดยอัตโนมัติ"
    // เมื่อเมาส์เคลื่อนที่ "ออก" จาก UI Element นี้
    public void OnPointerExit(PointerEventData eventData)
    {
        // ถ้าปุ่มอยู่ในสถานะ "พร้อมปลดล็อก" เท่านั้น
        if (_currentState == SkillState.Unlockable)
        {
            // ให้เปลี่ยนสีไอคอนกลับไปเป็นสีเทาเหมือนเดิม
            iconImage.color = lockedColor;
        }
        // TODO: เพิ่ม Logic การซ่อน Tooltip ที่นี่
    }

    private enum SkillState { Unlocked, Unlockable, Locked }
}