using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    //private Vector3 lastVelocity;

    private bool launched = false;

    private float warpTimer = 0;
    private float warpLim = .25f;
    private float deathHeight = -5f;
    private float speedIncrease = .05f;

    private LevelManager levelManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update(){
        if (warpTimer < warpLim){
            warpTimer += Time.deltaTime;
        }

        if (transform.position.y < deathHeight){
            levelManager.DestroyBall(gameObject);
        }
    }

    public void Launch(Transform launchTransform){
        rb.AddForce(launchTransform.up * speed);
        launched = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Block"){
            Block newBlock = other.gameObject.GetComponent<Block>();
            newBlock.Hit();
        } else if (other.transform.tag == "Enemy"){
            BasicEnemy newEnemy = other.gameObject.GetComponent<BasicEnemy>();
            newEnemy.Hit();
        } else if (other.transform.tag == "Player"){
            Vector3 hitPoint = other.contacts[0].point;
            Vector3 paddleCenter = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y);

            rb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;
            if (hitPoint.x < paddleCenter.x){
                rb.AddForce(new Vector2(-(Mathf.Abs(difference * 350)), speed));
            } else {
                rb.AddForce(new Vector2((Mathf.Abs(difference * 350)), speed));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.transform.tag == "Warp" && warpTimer >= warpLim){
            warpTimer = 0;
            transform.position = other.gameObject.GetComponent<Warp>().destination;
        } else if (other.transform.tag == "Switch"){
            other.gameObject.GetComponent<Switch>().ToggleSwitch();
        }
    }

}
