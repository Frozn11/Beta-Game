using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starat : MonoBehaviour
{
    public GameObject Win;
    public GameObject Dead;
    public GameObject Menu;
    public GameObject Player;
    public GameObject Cam;
    public GameObject Cam2;
    public GameObject Gun;
    private PlayerMovementGrappling PlayerHp;
    public HealthBar Hp;

    private void Start()
    {
        PlayerHp = GetComponent<PlayerMovementGrappling>();
    }
    void Update()
    {
        if (Hp.slider.value < 0)
        {
            Dead.SetActive(true);
            Menu.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);

            Cam.GetComponent<PlayerCam>().enabled = false;
            Cam.GetComponent<MenuPanal>().enabled = false;
            Cam2.GetComponent<PlayerCam2>().enabled = false;
            Gun.GetComponent<WeaponSway>().enabled = false;
            Player.GetComponent<PlayerMovementGrappling>().enabled = false;
            transform.localScale = new Vector3(transform.localScale.x, PlayerHp.crouchYScale, transform.localScale.z);
            Player.GetComponent<WallRunningAdvanced>().enabled = false;
            Player.GetComponent<Timer>().enabled = false;
            Time.timeScale = 0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "YouWin")
        {
            Win.SetActive(true);
            Menu.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
            
            Player.GetComponent<PlayerMovementGrappling>().enabled = false;
            Cam.GetComponent<PlayerCam>().enabled = false;
            Cam.GetComponent<MenuPanal>().enabled = false;
            Cam2.GetComponent<PlayerCam2>().enabled = false;
            Gun.GetComponent<WeaponSway>().enabled = false;
            Player.GetComponent<WallRunningAdvanced>().enabled = false;
            Player.GetComponent<Timer>().enabled = false;
            Time.timeScale = 0.1f;

        }
        if (other.tag == "Death")
        {

            PlayerHp.currentHealth = 0;
            PlayerHp.maxHalth = 0;

            Dead.SetActive(true);
            Menu.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
 
            Cam.GetComponent<PlayerCam>().enabled = false;
            Cam.GetComponent<MenuPanal>().enabled = false;
            Cam2.GetComponent<PlayerCam2>().enabled = false;
            Gun.GetComponent<WeaponSway>().enabled = false;
            Player.GetComponent<WallRunningAdvanced>().enabled = false;
            Player.GetComponent<Timer>().enabled = false;
            Time.timeScale = 0.1f;

        }
    }
}
