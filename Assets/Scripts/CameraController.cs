using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target To Follow")]
    public Transform playerTarget;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float distanceFromTarget = 5f;
    public float heightOffset = 2f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.25f; // เพิ่มค่าเริ่มต้น
    public float shakeMagnitude = 0.5f;   // เพิ่มค่าเริ่มต้น

    private float _yaw;
    private float _pitch;

    // --- [อัปเกรด] ---
    private Vector3 _currentShakeOffset = Vector3.zero; // ตัวแปรเก็บค่าการสั่นในแต่ละเฟรม

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (playerTarget == null) return;

        _yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        _pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 currentRotation = new Vector3(_pitch, _yaw);
        transform.eulerAngles = currentRotation;

        // คำนวณตำแหน่ง "พื้นฐาน" ที่กล้องควรจะอยู่
        Vector3 basePosition = playerTarget.position + Vector3.up * heightOffset - transform.forward * distanceFromTarget;

        // นำตำแหน่งพื้นฐาน มา "บวก" กับค่าการสั่นปัจจุบัน
        transform.position = basePosition + _currentShakeOffset;
    }

    public void TriggerShake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // สร้างค่าการสั่นแบบสุ่ม
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            // "อัปเดต" ค่าการสั่น แทนที่จะไปยุ่งกับ transform.position โดยตรง
            _currentShakeOffset = new Vector3(x, y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // เมื่อสั่นเสร็จ, รีเซ็ตค่าการสั่นให้กลับเป็นศูนย์
        _currentShakeOffset = Vector3.zero;
    }
}