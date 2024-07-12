using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public ParticleSystem particles;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);

        if (transform.position.y > 30){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag != "Player" && other.transform.tag != "Ball"){
            if (other.transform.tag == "Block"){
                other.gameObject.GetComponent<Block>().Hit();
            } else if (other.transform.tag == "Switch"){
                other.gameObject.GetComponent<Switch>().ToggleSwitch();
            }
            Instantiate(particles, other.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
