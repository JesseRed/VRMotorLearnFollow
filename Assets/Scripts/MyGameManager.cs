using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using TMPro;
public class MyGameManager : MonoBehaviour
{
        // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject playBallPrefab;
    private GameObject cloneplayBall;

    public static int numActiveBalls;
    private OVRPlayerController ovrPlayerController;
    //private static MyGameManager myGameManager;
    public string paradigmFolderName = Path.Combine(Application.streamingAssetsPath, "Paradigms"); 
    public string dataFolderName = Path.Combine(Application.streamingAssetsPath, "Data"); 
    public string datafilename = "tmpsave.json";
    //public PlayerData playerData;

    // has to be -1 do not change 
    private int current_ball_id = -1;

    //public float current_target_radius = 0.2f;
    private OVRCameraRig ovrCameraRig;
    //private Spawner spawner;
    //public GameObject wall;
    public GameSession gameSession;
    public Parameter parameter;
    public Paradigma paradigma;
    public GameObject anzeigeText;
    //public Catmul playBall;
    public bool is_game_active = true;
    private GameObject leftHand, rightHand, rightEye, centerEye;
    private GameObject ovrPlayerController2;
    private GameObject forwardDirection2, ovrCameraRig2, trackingSpace2, offset32;

    private void Awake()
    {
        SetUpSingleton();
        gameSession = FindObjectOfType<GameSession>();
        parameter = FindObjectOfType<Parameter>();
        Physics.gravity = new Vector3(0.0f, -9.81f , 0.0f);
        rightEye = GameObject.Find("RightEyeAnchor");
        centerEye = GameObject.Find("CenterEyeAnchor");
        ovrPlayerController2 = GameObject.Find("OVRPlayerController");
        forwardDirection2 = GameObject.Find("ForwardDirection");
        ovrCameraRig2 = GameObject.Find("OVRCameraRig");
        trackingSpace2 = GameObject.Find("TrackingSpace");
        //offset32 = GameObject.Find("CenterEyeAnchor");
        //playBall = FindObjectOfType<PlayBall>();
    }


    public int get_current_Ball_ID(){
        return current_ball_id;
    }

    
    private void SetUpSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<MyGameManager>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        Debug.Log("MyGameManager:Start()");
        Application.targetFrameRate = 60;
        ovrPlayerController = FindObjectOfType<OVRPlayerController>();
        ovrCameraRig = FindObjectOfType<OVRCameraRig>();
       
        //StartCoroutine (CompensateHeadPosition());
        //parameter.adaptHand();
        //parameter.set_bounding_box_to_play();
        //SpawnNewBall();

        anzeigeText.GetComponent<TextMeshPro>().SetText("Bitte versuchen sie immer mit dem Zeigefinger in Kontakt mit dem roten Ball zu bleiben.\n Der Ball wird sich bewegen sobald sie ihn beruehren");
  
        StartCoroutine(spawnmanagement());
        
        
  
    }


    void Update(){
        // Debug.Log("rightEye Position" + rightEye.transform.position);
        // Debug.Log("OVRPlayerController Position" + ovrPlayerController2.transform.position);
        // Debug.Log("ForwardDirection Position" + forwardDirection2.transform.position);
        // Debug.Log("OVRCameraRig Position" + ovrCameraRig2.transform.position);
        // Debug.Log("TrackingSpace Position" + trackingSpace2.transform.position);
        //SpawnNewBall();
    }

    IEnumerator inter_ball_counter(){
        //zeigt den Counter bis zum erscheinen des naechsten Balls
        int duration = (int) gameSession.paradigma.no_ball_duration;
        anzeigeText.GetComponent<TextMeshPro>().SetText("Weiter in ...\n " + duration.ToString() + "\n Sekunden");
        while (duration>0)
        {
            
            yield return new WaitForSeconds(1.0f);
            duration--;
            anzeigeText.GetComponent<TextMeshPro>().SetText("Weiter in ...\n " + duration.ToString() + "\n Sekunden");
  

        }
    }

    IEnumerator spawnmanagement()
    {
        while (is_game_active ){
        // 1. das Playfield muss initialisiert sein 
        // 2. es darf aktuell keinen activen Ball geben
        // 3. dann wird ein neuer Ball reseted und mit einer neuen Id versehen 
        //    und bei der GameSession registriert
        //if (parameter.is_playarea_initialized){
        //    Debug.Log("MyGameManager:spawnmanagement parameter.is_playarea_initialized ... now SpawnNewBall");
            if (cloneplayBall) { 
                //Debug.Log("MyGameManager:spawnmanagmement: cloneplayBall exists");
                anzeigeText.GetComponent<TextMeshPro>().SetText("Bitte versuchen sie immer mit dem Zeigefinger in Kontakt mit dem roten Ball zu bleiben.\n Der Ball wird sich bewegen sobald sie ihn beruehren");
  
                yield return new WaitForSeconds(2.0f);
            }
            if (! cloneplayBall) {
                parameter.reset_hand();
                current_ball_id += 1;
                if (current_ball_id>=gameSession.paradigma.numBalls){
                    anzeigeText.GetComponent<TextMeshPro>().SetText("Das Spiel ist vorbei! \n Sie können nun das HeadSet absetzen!");
                    yield break;
                }
                Debug.Log("MyGameManager:spawnmanagmement: no cloneplayBall -> instantiate new one");
                yield return StartCoroutine(inter_ball_counter());
//               yield return new WaitForSeconds(1.0f);

                
                // Debug.Log("before............");
                // Debug.Log("rightEye Position" + rightEye.transform.position);
                // Debug.Log("OVRPlayerController Position" + ovrPlayerController2.transform.position);
                // Debug.Log("ForwardDirection Position" + forwardDirection2.transform.position);
                // Debug.Log("OVRCameraRig Position" + ovrCameraRig2.transform.position);
                // Debug.Log("TrackingSpace Position" + trackingSpace2.transform.position);
                // parameter.reset_hand();
                // Debug.Log("after reset hand...........");
                // Debug.Log("rightEye Position" + rightEye.transform.position);
                // Debug.Log("OVRPlayerController Position" + ovrPlayerController2.transform.position);
                // Debug.Log("ForwardDirection Position" + forwardDirection2.transform.position);
                // Debug.Log("OVRCameraRig Position" + ovrCameraRig2.transform.position);
                // Debug.Log("TrackingSpace Position" + trackingSpace2.transform.position);
                parameter.prepare_parameter_for_next_ball(current_ball_id);
                Debug.Log("after..................");
                Debug.Log("rightEye Position" + rightEye.transform.position);
                Debug.Log("OVRPlayerController Position" + ovrPlayerController2.transform.position);
                Debug.Log("ForwardDirection Position" + forwardDirection2.transform.position);
                Debug.Log("OVRCameraRig Position" + ovrCameraRig2.transform.position);
                Debug.Log("TrackingSpace Position" + trackingSpace2.transform.position);
                
                Debug.Log("spawn new Ball at position: " + parameter.playarea_center);

                SpawnNewBall();
                Debug.Log("spqwnmanagement: after Instantiate");
            }
            // if (!playBall.is_active){
            //     Debug.Log("MyGameManager:spawnmanagement playBall.is_active = "+ playBall.is_active);
            //     SpawnNewBall();
            // }
        
        
            yield return new WaitForSeconds(1.0f);
        
        }
        // // StartCoroutine(MoveTheBall(ball, spawnPosition, 5.0f));
    }

    public void show_counter(){

    }

    public void SpawnNewBall()
    {
        
        Debug.Log("spawn a new Ball Nr = " + current_ball_id);
        Quaternion spawnRotation = new Quaternion ();
        Debug.Log("after Quaternion");
        spawnRotation.eulerAngles = new Vector3 (0.0f, 0.0f);        
        Debug.Log("after spawnRotatino");
        Vector3 spawnposition = parameter.GetSpawnPosition();
        Debug.Log("right Eye Postion = " + rightEye.transform.position);
        Debug.Log("center Eye Postion = " + centerEye.transform.position);
        
        Debug.Log("new spawn Position = " + spawnposition);
        Debug.Log("spawn new Ball at position: " + parameter.playarea_center);
        cloneplayBall = Instantiate (playBallPrefab, spawnposition, spawnRotation);
        Debug.Log("SpawnNewBall: after after Instantiate");

       
    }


    IEnumerator CompensateHeadPosition()
    {
        yield return new WaitForSeconds(2.0f);
        //ovrPlayerController.transform.position = new Vector3(0.0f, 0.0f, -2.7f);
        //yield return new WaitForSeconds(2.0f);
        //ovrPlayerController.transform.position = new Vector3(0.0f, 0.0f, -1.7f);
        // yield return new WaitForSeconds(2.0f);
        // ovrCameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.00200245f);
        // yield return new WaitForSeconds(2.0f);
        // ovrCameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.00400245f);
        // yield return new WaitForSeconds(2.0f);
        // ovrCameraRig.transform.position = new Vector3(0.0f, 0.0f, 0.00600245f);
        // yield return new WaitForSeconds(2.0f);
        // ovrPlayerController.transform.position = new Vector3(0.0f, 0.0f, 1.7500245f);
        // yield return new WaitForSeconds(2.0f);
        // ovrPlayerController.transform.position = new Vector3(0.0f, 0.38f, -3.1500245f);
        // yield return new WaitForSeconds(2.0f);
        // ovrPlayerController.transform.position = new Vector3(0.0f, 0.38f, 3.1500245f);
        //yield return new WaitForSeconds(2.0f);
        //ovrCameraRig.transform.position = new Vector3(0.0f, 0.38f, 0.100245f);

    }

}
