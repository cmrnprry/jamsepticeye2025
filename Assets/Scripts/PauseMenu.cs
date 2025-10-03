using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pause;
    private static bool isPaused = false;

    public void OpenClosePause()
    {
        pause.SetActive(!pause.activeSelf);
        isPaused = pause.activeSelf;

        print(isPaused);
    }

    public static bool Pause()
    {
        return isPaused;
    }

    public static void SetPause(bool p)
    {
        isPaused = p;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            OpenClosePause();
        }
    }
}
