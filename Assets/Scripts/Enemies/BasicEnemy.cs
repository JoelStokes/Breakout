using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public int health;
    public int points;
    public GameObject[] drops;
    public ParticleSystem particles;
    
    private float hitTimer = 0;
    private float hitLim = .05f;

    void Start()
    {
        
    }

    void Update()
    {
        if (hitTimer < hitLim){
            hitTimer += Time.deltaTime;
        }
    }

    public void Hit(){
        if (hitTimer >= hitLim){    //Prevent accidently rapid double hit
            health--;
            hitTimer = 0;

            if (health > 0){
                //Play animation of enemy hurt?
            } else {
                //ADD INFO HERE FOR DROPPING LOOT / POWERUPS!
                GameObject.Find("LevelManager").GetComponent<LevelManager>().SubtractObject(true, points);

                for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                }

                Instantiate(particles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
