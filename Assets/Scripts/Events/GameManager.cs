using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerInput playerInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void EnterUIMode()
    {
        playerInput.SwitchCurrentActionMap("UI");
        // --- [เพิ่มใหม่] ---
        // สั่งปิด "ทางด่วนพิเศษ" ของคลิกซ้ายด้วย
        InputManager.Instance.EnableLightClickAction(false);
        Debug.Log("GAME STATE -> UI Mode");
    }

    public void EnterGameplayMode()
    {
        playerInput.SwitchCurrentActionMap("PlayerControls");
        // --- [เพิ่มใหม่] ---
        // สั่งเปิด "ทางด่วนพิเศษ" กลับคืนมา
        InputManager.Instance.EnableLightClickAction(true);
        Debug.Log("GAME STATE -> Gameplay Mode");
    }
}