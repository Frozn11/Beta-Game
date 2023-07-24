using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bers : MonoBehaviour
{
    public GameObject setting;
    public bool issettingctive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (issettingctive == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
    public void Pause()
    {
        setting.SetActive(true);
        issettingctive = true;
        this.GetComponent<PlayerCam>().enabled = false;
        Cursor.visible = (true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        setting.SetActive(false);
        issettingctive = false;
        this.GetComponent<PlayerCam>().enabled = true;
        Cursor.visible = (false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
