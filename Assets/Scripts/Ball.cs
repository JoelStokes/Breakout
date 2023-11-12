using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector3 lastVelocity;

    private bool launched = false;

    private float warpTimer = 0;
    private float warpLim = .25f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = Vector3.up;
    }

    private void Update(){
        if (warpTimer < warpLim){
            warpTimer += Time.deltaTime;
        }       
    }

    private void FixedUpdate() {
        if (launched)
            rb.velocity = lastVelocity * speed * Random.Range(0.1f, 0.2f);  //Random added to prevent ball stuck in perfect corners
    }

    public void Launch(Transform launchTransform){
        lastVelocity = launchTransform.up;
        launched = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        var newSpeed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, other.contacts[0].normal);

        lastVelocity = direction * Mathf.Max(newSpeed, 0f);

        if (other.transform.tag == "Block"){
            other.gameObject.GetComponent<Block>().Hit();
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.transform.tag == "Warp" && warpTimer >= warpLim){
            warpTimer = 0;
            transform.position = other.gameObject.GetComponent<Warp>().destination;
        }
    }

}
