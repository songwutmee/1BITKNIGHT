using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene To Load")]
    public string mainGameSceneName = "MainScene"; //reminder กนัลืมมาตั้งชื่อให้ตรงด้วย

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void StartGame()
    {
        Debug.Log("Starting game... Loading scene: " + mainGameSceneName);
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
