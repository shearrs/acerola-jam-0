using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] private PauseMenu pauseMenu;

    public bool Paused { get; private set; } = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void TogglePause()
    {
        if (!Level.Instance.HasStarted)
            return;

        if (!Paused)
        {
            pauseMenu.Enable();
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.Disable();
            Time.timeScale = 1;
        }

        Paused = !Paused;
    }
}
