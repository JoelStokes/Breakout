using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string power;
    public float fallSpeed;

    private float destroyY = -10f;

    void Update(){
        transform.position = new Vector3(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime), transform.position.z);

        if (transform.position.y < destroyY){
            Destroy(gameObject);
        }
    }
}
