using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public bool right = false;

    void Update()
    {
        if (right){
            transform.Rotate( 0, 0, Time.deltaTime * speed );
        } else {
            transform.Rotate(0, 0,  -Time.deltaTime * speed );
        }
    }

    public void Toggle(){
        right = !right;
    }
}
