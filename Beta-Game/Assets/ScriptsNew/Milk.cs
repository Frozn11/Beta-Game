using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milk : MonoBehaviour
{
    void Update()
    {
        float z = Mathf.PingPong(t: Time.time, length: 1f);
        Vector3 axis = new Vector3(x: 1, y: 1, z);
        transform.Rotate(axis, angle: 0.7f);
        
    }
}
