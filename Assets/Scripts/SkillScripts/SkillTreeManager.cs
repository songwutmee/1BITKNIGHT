using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerStatus playerStatus;
    public GameObject skillTreeWindow;
    public TextMeshProUGUI skillPointsText;

    [Header("Skill UI Links")]
    public List<SkillUILink> skillButtons;

    [Header("Event Channels")]
    public GameEvent onSkillTreeChanged;

    [Header("Player's Progression State")]
    public int skillPoints = 5;
    public List<Skill_SO> unlockedSkills = new List<Skill_SO>();

    [System.Serializable]
    public struct SkillUILink
    {
        public Skill_SO skillData;
        public SkillButtonUI skillButton;
    }

    void Start()
    {
        foreach (var link in skillButtons)
        {
            if (link.skillButton != null && link.skillData != null)
                link.skillButton.Initialize(link.skillData, this);
        }

        UpdateAllButtonVisuals();
        skillTreeWindow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            bool isWindowActive = !skillTreeWindow.activeSelf;
            skillTreeWindow.SetActive(isWindowActive);

            if (isWindowActive)
            {
                GameManager.Instance.EnterUIMode();
            }
            else
            {
                GameManager.Instance.EnterGameplayMode();
            }
        }
    }

    public void UpdateAllButtonVisuals()
    {
        if (skillPointsText != null)
        {
            skillPointsText.text = $"Skill Points : {skillPoints}";
        }

        foreach (var link in skillButtons)
        {
            if (link.skillButton != null)
                link.skillButton.UpdateVisuals(unlockedSkills, skillPoints);
        }
    }

    public void UnlockSkill(Skill_SO skillToUnlock)
    {
        if (unlockedSkills.Contains(skillToUnlock) || skillPoints < skillToUnlock.cost) return;

        bool requirementsMet = true;
        foreach (Skill_SO requiredSkill in skillToUnlock.requiredSkills)
        {
            if (!unlockedSkills.Contains(requiredSkill))
            {
                requirementsMet = false;
                break;
            }
        }
        if (!requirementsMet) return;

        skillPoints -= skillToUnlock.cost;
        unlockedSkills.Add(skillToUnlock);
        ApplySkillEffect(skillToUnlock);


        UpdateAllButtonVisuals();

        if (onSkillTreeChanged != null)
        {
            onSkillTreeChanged.Raise();
        }
    }

    private void ApplySkillEffect(Skill_SO skill)
    {
        switch (skill.statToUpgrade)
        {
            case Skill_SO.StatType.MaxHealth:
                playerStatus.UpgradeMaxHealth(skill.upgradeValue);
                break;
            case Skill_SO.StatType.MaxStamina:
                playerStatus.UpgradeMaxStamina(skill.upgradeValue);
                break;
            case Skill_SO.StatType.StaminaRegenRate:
                playerStatus.UpgradeStaminaRegen(skill.upgradeValue);
                break;
            case Skill_SO.StatType.AttackPower:
                playerStatus.UpgradeAttackPower(skill.upgradeValue);
                break;
            case Skill_SO.StatType.Defense:
                playerStatus.UpgradeDefense(skill.upgradeValue);
                break;
        }
    }
}
