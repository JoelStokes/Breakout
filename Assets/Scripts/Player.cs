using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public GameObject ArrowObj;
    public GameObject BallObj;
    public GameObject projectilePrefab;
    public ParticleSystem collectParticles;

    private float leftLim = -7.8f;
    private float rightLim = 7.8f;

    private float bulletTimer = 0;
    private float bulletLim = 2;
    private bool ballAttached = true;
    private float currentMove = 0;

    //Power-Up Variables
    private float powerUpTimer = 0;
    private float powerUpLim = 10;

    //UI
    public GameObject BulletMeterObj;
    private SpriteRenderer bulletMeterRenderer;
    private float meterMax;

    private LevelManager levelManager;

    void Start(){
        meterMax = BulletMeterObj.transform.localScale.y;
        bulletMeterRenderer = BulletMeterObj.GetComponent<SpriteRenderer>();
        HandleMeter();

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    void Update()
    {
        if (!ballAttached){ //Don't gain bullet energy until ball fired
            if (bulletTimer < bulletLim){
                bulletTimer += Time.deltaTime;
                HandleMeter();
            }       
        }

        transform.position = new Vector3(transform.position.x + (moveSpeed * Time.deltaTime * currentMove), transform.position.y, transform.position.z);

        if (transform.position.x < leftLim){
            transform.position = new Vector3(rightLim, transform.position.y, transform.position.z);
        } else if (transform.position.x > rightLim){
            transform.position = new Vector3(leftLim, transform.position.y, transform.position.z);
        }
    }

    private void Damage(){
        //This will be set up once the gameManager & health systems are in place. Take damage from last ball falling or hit by "hurt" object
    }

    private void Die(){
        //No more health
    }

    private void HandleMeter(){
        BulletMeterObj.transform.localScale = new Vector3(BulletMeterObj.transform.localScale.x, (bulletTimer / bulletLim) * meterMax, BulletMeterObj.transform.localScale.z);

        if (bulletTimer >= bulletLim){
            bulletMeterRenderer.color = Color.blue;
        } else {
            bulletMeterRenderer.color = Color.red;
        }
    }

    public void ApplyStats(){
        //Apply purchased stats, such as size, move speed, battery, to player from Level Manager saved data
    }

    public void Move(InputAction.CallbackContext context){  //Set facing direction along with applying move
        currentMove = context.ReadValue<float>();
    }

    public void Launch(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
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
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Hurt"){
            Damage();
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "Powerup"){
            Instantiate(collectParticles, other.ClosestPoint(transform.position), Quaternion.identity);

            string value = other.gameObject.GetComponent<Powerup>().power;
            switch (value){
                case "PlayerGrow":
                    transform.localScale = new Vector3(transform.localScale.x * 1.25f, transform.localScale.y * 1.1f, transform.localScale.z);
                    break;
                case "PlayerShrink":
                    transform.localScale = new Vector3(transform.localScale.x * .75f, transform.localScale.y * .9f, transform.localScale.z);
                    break;
                case "BallGrow":
                    //Ball doubles in size, timer
                    break;
                case "BallShrink":
                    //Ball halves in size, timer
                    break;
                case "Multiball":
                    levelManager.AddBalls(false);
                    break;
                case "Tripleball":
                    levelManager.AddBalls(true);
                    break;
                case "Battery":
                    bulletLim = bulletLim/2;    //Should this be redone with a timer?
                    break;
                case "LowBattery":
                    bulletLim = bulletLim*2;
                    break;
                case "Bullet":
                    //add an extra bullet per shot
                    break;
                case "Homing":
                    //make bullets auto track towards remaining block or enemy
                    break;
                case "Magnet":
                    //Ball sticks to saucer to allow reshoot arrow
                    break;
                case "PlayerFast":
                    //Player moves faster, timer
                    break;
                case "PlayerSlow":
                    //Player moves slower, timer
                    break;
                case "BallFast":
                    //Ball moves faster, timer
                    break;
                case "BallSlow":
                    //Ball moves slower, timer
                    break;
                case "FireBall":
                    //Set ball on fire, allows breaking through flammable obstacles
                    break;
                case "FireShot":
                    //Set ball on fire, allows breaking through flammable obstacles
                    break;
                case "MetalBall":
                    //Set ball metal, breaks through metal surfaces & obstacles
                    break;
                case "MetalShot":
                    //Set shot metal
                    break;
                default:
                    Debug.Log("ERROR! Invalid powerup name for " + value);
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
