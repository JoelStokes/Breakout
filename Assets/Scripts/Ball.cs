using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector3 lastVelocity;

    private bool launched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastVelocity = Vector3.up;
    }

    private void FixedUpdate() {
        if (launched)
            rb.velocity = lastVelocity * speed;
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
    }
}
