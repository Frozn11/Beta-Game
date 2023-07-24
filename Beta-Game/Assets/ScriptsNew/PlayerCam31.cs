using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerCam31 : MonoBehaviour
{
    //public float sensX;
    //public float sensY;
    public float sensX;
    public float sensY;


    public Transform orientation;
    public Transform camHolder;

    public Slider slider;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sensX = PlayerPrefs.GetFloat("currentSensitivity", 100);
        sensY = PlayerPrefs.GetFloat("currentSensitivity", 100);
        slider.value = sensX / 10;
        slider.value = sensY / 10;
    }

    private void Update()
    {
        // get mouse input
        //mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        // float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        PlayerPrefs.SetFloat("currentSensitivity", sensX);
        PlayerPrefs.SetFloat("currentSensitivity", sensY);
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void AdjustSpeed(float newSpeed)
    {
        sensX = newSpeed * 10;
        sensY = newSpeed * 10;
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}