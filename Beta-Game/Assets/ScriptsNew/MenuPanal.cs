using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanal : MonoBehaviour
{
    public GameObject setting;
    public bool issettingctive;
    public GameObject Cam;
    public GameObject Cam2;
    public GameObject Gun;
    public GameObject player;

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
        Cam.GetComponent<PlayerCam>().enabled = false;
        Gun.GetComponent<GrapplingGun>().enabled = false;
        Gun.GetComponent<WeaponSway>().enabled = false;
        Gun.GetComponent<WeaponSway>().enabled = false;
        player.GetComponent<PlayerMovementGrappling>().enabled = false;
        Cursor.visible = (true);
        Time.timeScale = 0.1f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        setting.SetActive(false);
        issettingctive = false;
        Cam.GetComponent<PlayerCam>().enabled = true;
        Gun.GetComponent<GrapplingGun>().enabled = true;
        Gun.GetComponent<WeaponSway>().enabled = true;
        Cam2.GetComponent<PlayerCam2>().enabled = false;
        player.GetComponent<PlayerMovementGrappling>().enabled = true;
        Cursor.visible = (false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

    }
    public void Restart()
    {
        issettingctive = false;
        setting.SetActive(false);
        Time.timeScale = 1f;
    }
}
