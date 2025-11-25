using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    [Header("Player References")]
    public PlayerStatus playerStatus; 

    [Header("UI Elements")]
    public Slider healthBar;

    public Slider staminaBar;

    private void Start()
    {
        if (playerStatus != null)
        {
            healthBar.maxValue = playerStatus.baseStats.maxHealth;
            staminaBar.maxValue = playerStatus.baseStats.maxStamina;
            UpdateHealth(); 
            UpdateStamina();
        }
    }

    public void UpdateHealth()
    {
        if (playerStatus != null)
        {
            healthBar.value = playerStatus.GetCurrentHealth();
        }
    }

    public void UpdateStamina()
    {
        if (playerStatus != null)
        {
            staminaBar.value = playerStatus.GetCurrentStamina();
        }
    }
}