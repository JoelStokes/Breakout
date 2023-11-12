using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public GameObject ArrowObj;
    public GameObject BallObj;
    public GameObject projectilePrefab;

    private float leftLim = -7.8f;
    private float rightLim = 7.8f;

    private float bulletTimer = 0;
    private float bulletLim = 2;
    private bool ballAttached = true;

    private float deathHeight = -5f;

    //UI
    public GameObject BulletMeterObj;
    private SpriteRenderer bulletMeterRenderer;
    private float meterMax;

    void Start(){
        meterMax = BulletMeterObj.transform.localScale.y;
        bulletMeterRenderer = BulletMeterObj.GetComponent<SpriteRenderer>();
        HandleMeter();
    }

    void Update()
    {
        if (!ballAttached){ //Don't gain bullet energy until ball fired
            if (bulletTimer < bulletLim){
                bulletTimer += Time.deltaTime;
                HandleMeter();
            }       
        }

        if (BallObj.transform.position.y < deathHeight){
            Die();
        }

        //CHANGE FOR PROPER INPUT SYSTEM FOR CONTROLLER & KEYBOARD LATER!
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)){
            transform.position = new Vector3(transform.position.x - (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
            transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.Space)){
            if (ballAttached){
                BallObj.GetComponent<Ball>().Launch(ArrowObj.transform);
                BallObj.GetComponent<TrailRenderer>().emitting = true;
                BallObj.transform.parent = null;
                ArrowObj.SetActive(false);
                ballAttached = false;
            } else if (bulletTimer >= bulletLim){
                Instantiate(projectilePrefab, transform.position, quaternion.identity);
                bulletTimer = 0;
            }
        } 

        if (transform.position.x < leftLim){
            transform.position = new Vector3(rightLim, transform.position.y, transform.position.z);
        } else if (transform.position.x > rightLim){
            transform.position = new Vector3(leftLim, transform.position.y, transform.position.z);
        }
    }

    private void Die(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleMeter(){
        BulletMeterObj.transform.localScale = new Vector3(BulletMeterObj.transform.localScale.x, (bulletTimer / bulletLim) * meterMax, BulletMeterObj.transform.localScale.z);

        if (bulletTimer >= bulletLim){
            bulletMeterRenderer.color = Color.blue;
        } else {
            bulletMeterRenderer.color = Color.red;
        }
    }
}
