using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Spawner : MonoBehaviour {
    // analog zu https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events?signup=true#5cf5960fedbc2a281acd21fa

    //public int spawnCount;
    //public int activeBalls = 0;
    // [Range (1,100)]
    // public int spawnSize = 1;
    // public float minionOffset = 1;
    public GameObject ball;

//    private UnityAction spawnListener;
//
//    void Awake () {
//        spawnListener = new UnityAction (Spawn);
//    }
 //   public MyGameManager myGameManager;
    //public List<BallInfo> _BallInfos = new List<BallInfo>();
    // public MyBallCollection myBallCollection = new MyBallCollection();
    private bool first_ball;
    void Start()
    {
        first_ball = true;
//        myGameManager = FindObjectOfType<MyGameManager>();

    }

    void Update(){

    }


    public void Spawn_A_NewBall () {
        // Wenn ein neuer Ball gespawned wird dann
        // muss auch vom Gamemanager der aktuelle Zielradius erfragt werden
        // und dieser sollte gezeichnet werden
        Vector3 spawnPosition = GetSpawnPosition ();
        
        float sec=2.0f; 
        if (first_ball) {
            //sec =0.1f;
            first_ball = false;
        }
        // StartCoroutine(InstantiateNow(spawnPosition, sec));
        Quaternion spawnRotation = new Quaternion ();
        spawnRotation.eulerAngles = new Vector3 (0.0f, 0.0f);
        
        // GameObject myball = Instantiate (ball, spawnPosition, spawnRotation) as GameObject;
        // myball.set_bounding_box(playarea_min, playarea_max);
        //         playBall.start_moving();
        // // StartCoroutine(MoveTheBall(ball, spawnPosition, 5.0f));
    }


    // IEnumerator  MoveTheBall(GameObject ball, Vector3 spawnPosition, float timeLeft) 
    // {
        
    //     Debug.Log("move  the Ball from Spawner start position =" + spawnPosition);
        
    //     // Quaternion spawnRotation = new Quaternion ();
    //     // spawnRotation.eulerAngles = new Vector3 (0.0f, 0.0f);
    //     while (timeLeft >= 0.0f) {
    //         // while (isPaused) {
    //             yield return new WaitForEndOfFrame ();
    //         // }

    //        timeLeft -= Time.deltaTime;
    //        Vector3 newPosition = ball.transform.position;
    //        newPosition.x += 0.1f;
    //        ball.transform.position = newPosition ;
    //     //  Debug.Log (timeLeft);
    //       //new WaitForSeconds (0.01f);
    //     }
    //     yield return null;
    // }
    //     // Instantiate (ball, spawnPosition, spawnRotation);
    

    Vector3 GetSpawnPosition () {
        Vector3 spawnPosition = new Vector3 ();
        //spawnPosition = new Vector3(0.217f, 1.068f, 0.53f);
        //spawnPosition = new Vector3(0.0f, 0.534f, 0.0f);
        //spawnPosition = new Vector3(0.0f, -0.951f, -2.65f);
        spawnPosition = new Vector3(0.0f, 0.0f, -2.65f);

        return spawnPosition;
    }        


    IEnumerator  InstantiateNow(Vector3 spawnPosition, float sec) 
    {
        yield return new WaitForSeconds (sec);
        Quaternion spawnRotation = new Quaternion ();
        spawnRotation.eulerAngles = new Vector3 (0.0f, 0.0f);

        Instantiate (ball, spawnPosition, spawnRotation);
    }



}