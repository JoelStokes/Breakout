using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    
    private float angleLim = 45f;
    private bool right = true;

    void Update()
    {
        if (right){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (speed * Time.deltaTime));
        } else {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - (speed * Time.deltaTime));
        }

        float angle = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        if ((right && angle > angleLim) || (!right && angle < -angleLim)){
            right = !right;
        }
    }
}
