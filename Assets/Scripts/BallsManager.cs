using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

    private void Awake(){
        if (_instance != null){
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    public List<Ball> Balls {get; set;}

    private void Start(){
        InitBall();
    }

    private void InitBall(){
        Vector3 startingPosition = new Vector3();
    }
}
