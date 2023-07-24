using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamControl : MonoBehaviour
{
    public Transform CurrentMount;
    public float SpeedFactor = 0.1f;
    public float zoomFactor = 1.0f;
    public Camera CameraComp;



    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, CurrentMount.position, SpeedFactor);
        transform.rotation = Quaternion.Slerp(transform.rotation, CurrentMount.rotation, SpeedFactor);

        float Velocity;

        Velocity = Vector3.Magnitude(transform.position - lastPosition);
        CameraComp.fieldOfView = 60 + Velocity * zoomFactor;

        lastPosition = transform.position;
    }
    public void setMount(Transform newMount)
    {
        CurrentMount = newMount;
    }
}
