using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void SnowBut(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
