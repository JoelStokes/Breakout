using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Add variables for alternate win conditions later

    private int blockCount = 0;
    private int blockTotal = 0;
    private int enemyCount = 0;
    private int enemyTotal = 0;
    private List<GameObject> Balls = new List<GameObject>();

    private GameManager gameManager;
    private Player player;

    private int score = 0;
    private int coins = 0;

    void Start()    //Must happen after awake to grab newest GameManager
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();  //Level should instantiate player rather than needing to find them?

        //Get number of blocks & enemies to help calculate win condition
        blockCount = blockTotal = GameObject.FindGameObjectsWithTag("Block").Length;
        enemyCount = enemyTotal =  GameObject.FindGameObjectsWithTag("Enemy").Length;

        //Get starting ball to check losing condition
        Balls.Add(GameObject.Find("Ball"));
    }

    public void SubtractObject(bool block, int value){
        if (block){
            blockCount--;
        } else {
            enemyCount--;
        }
        score += value;

        Debug.Log("Remaining Blocks: " + blockCount + ", Remaining Enemies: " + enemyCount);

        if (enemyCount <= 0 && blockCount <= 0){
            WinLevel();
        }
    }

    public void AddCoins(int amount){
        coins += amount;
    }

    public void AddBalls(bool triple){
        for (int i=0; i<Balls.Count; i++){
            CreateBall(i, Balls[i].transform, false);
            if (triple){
                CreateBall(i, Balls[i].transform, true);
            }
        }
    }

    private void CreateBall(int i, Transform newTransform, bool triple){ //May need to switch to object pooling method for performance improvement
        Debug.Log("Transform in LevelManager: " + newTransform);
        GameObject Clone;
        if (!triple){
            Clone = GameObject.Instantiate(Balls[i], new Vector3(Balls[i].transform.position.x+.01f, Balls[i].transform.position.y+.01f, Balls[i].transform.position.z), Quaternion.identity);
        } else {
            Clone = GameObject.Instantiate(Balls[i], new Vector3(Balls[i].transform.position.x-.01f, Balls[i].transform.position.y-.01f, Balls[i].transform.position.z), Quaternion.identity);
        }
        Clone.GetComponent<Ball>().Launch(newTransform);
    }

    public void GrowBalls(){
        //Called from Player powerup, increases size of all balls in level
    }

    public void ShrinkBalls(){
        //Called from Player powerup, reduces size of all balls in level
    }

    public void ChangeBallTypes(){
        //Called from Player powerup, changed type of all balls to metal or fire
    }

    public void DestroyBall(GameObject BallToRemove){
        Balls.Remove(BallToRemove);
        Destroy(BallToRemove);

        if (Balls.Count <= 0){
            LoseLevel();    //Switch to losing player health & relauching ball later!
        }
    }

    private void ApplyPlayerStats(){    //Get saved data about purchased player upgrades and apply them
        player.ApplyStats();
    }

    private void LoseLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void WinLevel(){
        for (int i=0; i<Balls.Count; i++){
            Balls[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;   //Change to call to Ball script stopping motion & shrinking / vanishing?
        }
        SaveData();
    }

    private void SaveData(){
        //Called after the level has been beaten to add data to the player's save file & update high score if applicable
    }
}
