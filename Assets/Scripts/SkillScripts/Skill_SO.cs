using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Soul-like/Skill", order = 3)]
public class Skill_SO : ScriptableObject
{
    [Header("Skill Information")]
    public string skillName;
    [TextArea(3, 10)]
    public string description;

    public Sprite unlockedIcon;
    public Sprite lockedIcon;


    [Header("Skill Cost & Requirements")]
    public int cost;
    public List<Skill_SO> requiredSkills;

    [Header("Skill Effect")]
    public StatType statToUpgrade;
    public float upgradeValue;

    public enum StatType
    {
        MaxHealth,
        MaxStamina,
        StaminaRegenRate,
        AttackPower,
        Defense
    }
}
