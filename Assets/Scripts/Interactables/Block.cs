using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int health;
    public int points;
    public GameObject[] Drops;
    public float[] dropPercents;
    public Color[] colors;
    public ParticleSystem particles;

    private SpriteRenderer spriteRenderer;

    private float hitTimer = 0;
    private float hitLim = .05f;

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update(){
        if (hitTimer < hitLim){
            hitTimer += Time.deltaTime;
        }
    }

    public void Hit(){
        if (hitTimer >= hitLim){    //Prevent accidently rapid double hit
            health--;
            hitTimer = 0;

            if (health > 0){
                spriteRenderer.color = colors[health-1];  //To be replaced with an array of sprites that show damage?
            } else {
                GameObject.Find("LevelManager").GetComponent<LevelManager>().SubtractObject(true, points);

                for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(gameObject.transform.GetChild(i).gameObject);
                }

                Instantiate(particles, transform.position, Quaternion.identity);
                DetermineDrop();
                
                Destroy(gameObject);
            }
        }
    }

    private void DetermineDrop(){
        Random.Range(0, 100);

        //Add drop rate increase each time no power-up drops to help prevent long droughts
        //if ()
    }
}
