using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : IPlayerState
{
    private readonly PlayerController _playerController;
    private float _dodgeDuration = 0.7f; // <<<--- ตรวจสอบและแก้ไขค่านี้ให้ตรงกับ Animation ของคุณ!
    private float _dodgeSpeed = 8f;      // <<<--- เพิ่มตัวแปรนี้เข้าไป: ความเร็วในการพุ่ง
    private float _stateTimer;
    private Vector3 _dodgeDirection;     // <<<--- เพิ่มตัวแปรนี้เข้าไป: ทิศทางที่จะพุ่งไป

    public PlayerDodgeState(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Enter()
    {
        Debug.Log("Entering Dodge State");
        _stateTimer = _dodgeDuration;
        _playerController.GetAnimator().SetTrigger(_playerController.DodgeHash);

        // --- คำนวณทิศทางที่จะพุ่งไป ---
        if (_playerController.MoveInput.sqrMagnitude > 0.01f)
        {
            // ถ้าผู้เล่นกดทิศทางค้างไว้ ให้พุ่งไปทางนั้น
            Vector2 moveInput = _playerController.MoveInput;
            _dodgeDirection = new Vector3(moveInput.x, 0, moveInput.y);

            Transform cameraTransform = Camera.main.transform;
            _dodgeDirection = cameraTransform.forward * _dodgeDirection.z + cameraTransform.right * _dodgeDirection.x;
            _dodgeDirection.y = 0;
            _dodgeDirection.Normalize(); // ทำให้ทิศทางมีความยาวเท่ากับ 1
        }
        else
        {
            // ถ้าไม่ได้กดอะไรเลย ให้พุ่งไปข้างหน้าที่ตัวละครหันอยู่
            _dodgeDirection = _playerController.transform.forward;
        }
    }

    public void Update()
    {
        // --- สั่งให้ตัวละครเคลื่อนที่ในแต่ละเฟรม ---
        // เราจะใช้ _playerController.GetComponent<CharacterController>() เพื่อความแน่นอน
        _playerController.GetComponent<CharacterController>().Move(_dodgeDirection * _dodgeSpeed * Time.deltaTime);

        _stateTimer -= Time.deltaTime;
        if (_stateTimer <= 0f)
        {
            _playerController.SwitchState(_playerController.GroundedState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Dodge State");
    }

    // ฟังก์ชันรับ Input ปล่อยว่างไว้เหมือนเดิม
    public void OnJump() { }
    public void OnDash() { }
    public void OnLightAttack() { }
    public void OnHeavyAttack() { }
}