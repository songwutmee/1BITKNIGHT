using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Dependencies")]
    public PlayerHitbox weaponHitbox;

    void Start()
    {
        if (weaponHitbox == null)
        {
            Debug.LogError("Player's Weapon Hitbox is not assigned in PlayerCombat!", this.gameObject);
        }
        else
        {
            weaponHitbox.gameObject.SetActive(false);
        }
    }

    public void EnableWeaponHitbox()
    {
        if (weaponHitbox != null)
        {
            weaponHitbox.gameObject.SetActive(true);
        }
    }

    public void DisableWeaponHitbox()
    {
        if (weaponHitbox != null)
        {
            weaponHitbox.gameObject.SetActive(false);
        }
    }
}
