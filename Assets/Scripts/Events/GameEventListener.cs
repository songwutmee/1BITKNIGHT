using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public UnityEvent response; 

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }
//gameobecjt เรียกถ้าโดนเรียกใช้ไรงี้
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
