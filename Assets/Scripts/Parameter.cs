using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;
using TMPro;

public class Parameter : MonoBehaviour
{
    public GameSession gameSession;

    public MyGameManager myGameManager;
    public PlayBall playBall;
    //public OVREye 
    //public RightHandAnchor rightHandAnchor;
    // the general difficulty specifize the probability that different 
    // disturbing influences come together
    public float current_general_difficulty = 10.0f;

    private Vector3 playarea_min;
    private Vector3 playarea_max;
    public Vector3 playarea_center;
    
    
    //private GameObject playBall;

    //scaling difficulty ... each offset scales with this factor
    public float current_difficulty_scaling = 1.0f;
    public int counter=0;
    //public float current_ball_mass;
    //public Vector3 current_gravity;
    //public Vector3 current_force;
    //public Vector3 current_offset_hand_pos;
    //public Vector3 current_offset_hand_vel;
    //public Vector3 current_invert;
    //public Vector4 current_tremor; // der letzte Eintrag fuer die Frequenz
    //public Vector3 max_force;
    //public Vector3 max_offset_hand_pos;
    //public Vector3 max_offset_hand_vel;
    //public Vector3 max_invert;
    //public Vector4 max_tremor; // der letzte Eintrag fuer die Frequenz
    // However every one of these influences represents an different difficulty scale which is represented
    // in the following table   
    //                            X Y Z
    // current force            [ 5 5 2 ]
    // current_offset_hand_pos  [ 2 2 2 ]
    // current_offset_hand_vel  [ 2 2 2 ]
    // current_invert           [ 7 7 9 ]
    // current_tremor           [ 1 1 1 1]
    // many of these influences make it very difficult to grap the ball but not necessarily to throw the ball
    // make changes here for different Difficulty ratings
    // the difficulty is however also mainly influenced by the scaling in the parameter file 
    public Vector3 difficulty_force =           new Vector3(5.0f, 5.0f, 2.0f);
    public Vector3 difficulty_offset_hand_pos = new Vector3(2.0f, 2.0f, 2.0f);
    public Vector3 difficulty_offset_hand_vel = new Vector3(2.0f, 2.0f, 2.0f);
    public Vector3 difficulty_invert =          new Vector3(7.0f, 7.0f, 9.0f);
    public Vector4 difficulty_tremor =          new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    public bool isdebug = true;
    private GameObject leftHand, rightHand, rightEye;
    public GameObject mycenterEyeAnchor;
    public int current_ball_ID;
    public bool is_playarea_initialized = false;
    public bool is_in_initializing_process = false;

    public List<int> punkteBlockList = new List<int>();
    public List<int> max_moegliche_punkteBlockList = new List<int>();
    public List<float> previous_targetDifficulties = new List<float>();
    public List<float> previous_percentageHit = new List<float>();
    public float cur_target_difficulty = 50.0f;
    public int punkteBlock = 0;
    public int max_moegliche_punkteBlock = 0;
    public float lastBallEndVeloc = 0.0f;
    public int punkteLast5Blocks = 0;
    public int max_moegliche_punkteLast5Blocks = 0;
    public int max_moegliche_punkteGesamt = 0;
    public int punkteGesamt = 0;
    private Vector3 uninverted_right_eye = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 uninverted_right_hand = new Vector3(0.0f,0.0f,0.0f);
    
    //public GameObject anzeigeText;
    public GameObject anzeigeTextBlock;
    public GameObject anzeigeTextPunkteBlock;
    public GameObject anzeigeTextPunkteGesamt;
    public GameObject anzeigeTextDifficulty;
    public GameObject anzeigeTextTargetRate;


    public Ball_Difficulty ballDifficulty; 
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log("Parameter:Start()");
        //playBall = GameObject.Find("Catmul");
        //playBall = FindObjectOfType<PlayBall>();
        gameSession = FindObjectOfType<GameSession>();
//        lineObj = FindObjectOfType<LineObj>(); // Zeichnet den Zielkreis auf die Wand
        myGameManager = FindObjectOfType<MyGameManager>();
   //     leftHand = GameObject.Find("CustomHandLeft");
  //      rightHand = GameObject.Find("CustomHandRight");
        leftHand = GameObject.Find("LeftHandAnchor");
        //rightHand = GameObject.Find("RightHandAnchor");
        rightHand = GameObject.Find("hands:b_r_index_ignore");
        rightEye = GameObject.Find("RightEyeAnchor");
        //anzeigeTextPunkteBlock = GameObject.Find("AnzeigeTextPunkteBlock");
        //anzeigeTextPunkteGesamt = GameObject.Find("AnzeigeTextPunkteGesamt");
        ballDifficulty = new Ball_Difficulty();
        ballDifficulty.init(gameSession);

        // festlegen des Spiegelpunkts der Invertierung



        //current_target_radius = gameSession.paradigma.target_size;
       // current_ball_mass = gameSession.paradigma.ball_mass;
    }

    void Update(){

    }

    public void register_finished_block(){
        // methode wird aufgerufen wenn ein block vorbei ist
        Debug.Log("register_finished_block");
        punkteBlockList.Add(punkteBlock);
        max_moegliche_punkteBlockList.Add(max_moegliche_punkteBlock);
        previous_percentageHit.Add(punkteBlock/max_moegliche_punkteBlock);
        previous_targetDifficulties.Add(cur_target_difficulty);
    }


    public float estimate_new_target_difficulty(){
        Debug.Log("estimate_new_target_difficulty");

        float target_rate = gameSession.paradigma.desired_hit_rate*100;
        if (previous_percentageHit.Count==0){
            // first block 
            Debug.Log("estimate_new_target_difficulty first block");
            cur_target_difficulty = 100 - target_rate;
            if (cur_target_difficulty>50.0f){
                cur_target_difficulty = 50.0f;
            }
            return(cur_target_difficulty);
        }
        Debug.Log("estimate_new_target_difficulty after first block");

        

        float last_real_rate = previous_percentageHit[previous_percentageHit.Count-1];
        float last_targetDifficulties = previous_targetDifficulties[previous_targetDifficulties.Count-1];
        float avg_targetDifficulties_all = get_average_of_last_num(previous_targetDifficulties, previous_targetDifficulties.Count);
        float real_rate_last5 = get_average_of_last_num(previous_percentageHit,5);
        float real_rate_all = get_average_of_last_num(previous_percentageHit,previous_percentageHit.Count);
        float diff_all = target_rate - real_rate_all;

        cur_target_difficulty = last_targetDifficulties;    

        /// 
        float abs_difficulty_diff = Mathf.Abs(last_real_rate - target_rate);
        float difficulty_diff = last_real_rate - target_rate;
        // positive Werte heissen, dass es zu einfach war
        // ich fuehre 20 Level ein 
        // d.h. Schwierigkeitsgrad 0 bis 20 == in 5 er Schritten von 0 bis 100
        bool is_decrease = true;
        if (last_real_rate>target_rate){
            // es war im letzten blodk zu einfach
            // dann war es die Einstellung auf last_targetDifficulties zu niedrig
            is_decrease = false;
        }
        // nun noch bestimmen um wieviele Level wir aendern
        // wir aendern die Schwierigkeit um 1/3 des Unterschieds des targets
        // 50 -> 17
        // 
        float to_change = abs_difficulty_diff/3;

        if (is_decrease){
            cur_target_difficulty = last_targetDifficulties - to_change;
        }else{
            cur_target_difficulty = last_targetDifficulties + to_change;
        }

        if (cur_target_difficulty<1.0f){
            cur_target_difficulty = 1.0f;
        }

        if (cur_target_difficulty>100.0f){
            cur_target_difficulty = 100.0f;
        }
        Debug.Log("target_rate = " + target_rate);
        Debug.Log("last_real_rate = " + last_real_rate);
        Debug.Log("old difficulty = " + last_targetDifficulties);
        Debug.Log("new difficulty = " + cur_target_difficulty);
        return(cur_target_difficulty);
        
    }

    private float get_average_of_last_num(List<float> L , int n){
        // berechnet den Mittelwert der letzten n elemente der Liste L
        float average = 0.0f;
        if (n>L.Count){
            n = L.Count;
        }
        for (int i=L.Count-n; i<L.Count; i++){
            average += L[i];
        }
        average /= n;
        return(average);
    }

    public int push_infos(float currentFingerBallDist, float currentInBlockHitRate){
        // der Ball ruft bei jedem Update die MEthode auf um die Infos ueber
        // den /FingerBallabstand zu uebergeben

//        Debug.Log("currentFingerBallDist =  " + currentFingerBallDist);
        // ich berechne die Punkte in Relation zur Laenge der imaginaeren Box in der der Ball fliegt
        float laenge =  Mathf.Abs(gameSession.paradigma.playarea_max_x - gameSession.paradigma.playarea_min_x)/2.0f;
        // wenn der Abstand groesser ist gibt es 0 Punkte 
        // ab dort wird der Abstand halbiert und bei jeder halbierung gibt es einen Punkt mehr (10 Schritte)
        // wie oft (x) muss ich laenge halbieren um eine Zahl kleiner als currentFingerBallDist zu erhalten
        // laenge/(2`x)= currentFingerBallDist
        // laenge = currentFingerBallDist * 2^x
        // (laenge/currentFingerBallDist) =  2^x
        // x = log(laenge/currentFingerBallDist) zur Basis 2
        //Debug.Log("laenger ="  + laenge + "  cuFBD = " + currentFingerBallDist.ToString());
        
        int punkteAdd =  Mathf.RoundToInt(Mathf.Log((laenge / currentFingerBallDist),2));
        if (punkteAdd>10){ punkteAdd = 10; }
        if (punkteAdd <0){ punkteAdd = 0;  }
        //Debug.Log("estimated Add Punkte="+punkteAdd  + " (  currentFingerBallDist =  " + currentFingerBallDist +")" );
        punkteBlock += punkteAdd;
        punkteGesamt+= punkteAdd;
        max_moegliche_punkteBlock += 10;
        max_moegliche_punkteGesamt += 10;
        
        anzeigeTextPunkteBlock.GetComponent<TextMeshPro>().SetText(punkteBlock.ToString());
        anzeigeTextPunkteGesamt.GetComponent<TextMeshPro>().SetText(punkteGesamt.ToString());
        anzeigeTextDifficulty.GetComponent<TextMeshPro>().SetText(cur_target_difficulty.ToString());
        anzeigeTextTargetRate.GetComponent<TextMeshPro>().SetText(currentInBlockHitRate.ToString("n2"));
        

        //Debug.Log("estimated Punkte in push_info ="  + punkteAdd);
        return (punkteAdd);
    }




    public void prepare_parameter_for_next_ball(int _ID){
        Debug.Log("prepare_parameter_for_next_ball by ID="+_ID);

        current_ball_ID = _ID;
        int block_nr = current_ball_ID + 1;
        anzeigeTextBlock.GetComponent<TextMeshPro>().SetText(block_nr.ToString());
        
        cur_target_difficulty = estimate_new_target_difficulty();
        Debug.Log("-------------------------------"); 
        Debug.Log("now ballDifficulty.estimate_new_difficulty"); 
        Debug.Log("cur_target_difficulty = " + cur_target_difficulty);
        Debug.Log("no_ball_duration = " + gameSession.paradigma.no_ball_duration.ToString());
        ballDifficulty.estimate_new_difficulty(cur_target_difficulty); 
        punkteBlock = 0;
        max_moegliche_punkteBlock = 0;
    }
 
    IEnumerator save_uninverted_pos(){
        // da ich es nicht hinbekomme die neuen Positionen 
        // zu berechnen speichere ich die alten ab
        yield return new WaitForSeconds(0.03f);
        uninverted_right_eye[0] = rightEye.transform.position.x;
        uninverted_right_eye[1] = rightEye.transform.position.y;
        uninverted_right_eye[2] = rightEye.transform.position.z;
        uninverted_right_hand[0] = rightHand.transform.position.x;
        uninverted_right_hand[1] = rightHand.transform.position.y;
        uninverted_right_hand[2] = rightHand.transform.position.z;
        
    }
    public void reset_hand(){
        OVRPlugin.carsten_offset_hand_pos = new Vector3(0.0f, 0.0f, 0.0f);
        OVRPlugin.carsten_offset_hand_vel = new Vector3(1.0f, 1.0f, 1.0f);
        OVRPlugin.carsten_invert = new Vector3(0.0f, 0.0f, 0.0f);
        OVRPlugin.carsten_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        StartCoroutine(save_uninverted_pos());

    }
    public void apply_current_difficulty_to_hand(){
        Debug.Log("start apply_current_difficulty_to_hand()");

        OVRPlugin.carsten_spiegelpunkt_der_invertierung = new Vector3(
            gameSession.paradigma.spiegelpunkt_der_invertierung_x, 
            gameSession.paradigma.spiegelpunkt_der_invertierung_y, 
            gameSession.paradigma.spiegelpunkt_der_invertierung_z 
            );

        // 04.07.2021 
        // setzte den neuen Spiegelpunkt in die Mitte der Spielflaeche
        // das ist zugleich auch die Spawn Position
        //OVRPlugin.carsten_spiegelpunkt_der_invertierung = playarea_center;
        OVRPlugin.carsten_offset_hand_pos = ballDifficulty.get_offset_hand_pos();
        OVRPlugin.carsten_offset_hand_vel = ballDifficulty.get_offset_hand_vel();
        OVRPlugin.carsten_invert = ballDifficulty.get_invert();
        OVRPlugin.carsten_tremor = ballDifficulty.get_tremor();
        Debug.Log("OVRPlugin.carsten_spiegelpunkt_der_invertierung = "+ OVRPlugin.carsten_spiegelpunkt_der_invertierung);
        Debug.Log("OVRPlugin.carsten_offset_hand_pos = "+ OVRPlugin.carsten_offset_hand_pos);
        Debug.Log("OVRPlugin.carsten_invert = "+ OVRPlugin.carsten_invert);
        // if (OVRPlugin.carsten_invert[0]>0.1){
        //     eyeposition_offset_x = gameSession.paradigma.spiegelpunkt_der_invertierung_x;
        // }
        // if (OVRPlugin.carsten_is_invert_in_effect_Y){
        //     eyeposition_offset_y = gameSession.paradigma.spiegelpunkt_der_invertierung_y;
        // }
        // if (OVRPlugin.carsten_is_invert_in_effect_Z){
        //     eyeposition_offset_z = gameSession.paradigma.spiegelpunkt_der_invertierung_z;
        // }
    }

    public Vector3 GetSpawnPosition () {
        // gibt die Spawn Position des Balls zurueck
        // Die spawn position sollte sich in der Mitte eines Quaders befinden, der 
        // durch das Parameter File vorgegeben ist
        // es sollte sich aber immer auch in relation zu der aktuellen
        // Position des Headsets befinden

        // den offset muessen wir nur einfuehren wenn die invertierung wirklich auch im OvRPlugin angewendet wurden
        float eyeposition_offset_x = 0.0f;
        float eyeposition_offset_y = 0.0f;
        float eyeposition_offset_z = 0.0f;


        if (OVRPlugin.carsten_is_invert_in_effect_X){
            eyeposition_offset_x = gameSession.paradigma.spiegelpunkt_der_invertierung_x;
        }
        if (OVRPlugin.carsten_is_invert_in_effect_Y){
            eyeposition_offset_y = gameSession.paradigma.spiegelpunkt_der_invertierung_y;
        }
        if (OVRPlugin.carsten_is_invert_in_effect_Z){
            eyeposition_offset_z = gameSession.paradigma.spiegelpunkt_der_invertierung_z;
        }


        playarea_min[0] = rightEye.transform.position.x + eyeposition_offset_x + gameSession.paradigma.playarea_min_x;
        playarea_min[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_min_y;
        playarea_min[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_min_z;
        playarea_max[0] = rightEye.transform.position.x + eyeposition_offset_x +gameSession.paradigma.playarea_max_x;
        playarea_max[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_max_y;
        playarea_max[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_max_z;
        playarea_center = (playarea_min+playarea_max)/2.0f;



//        spawnPosition = new Vector3(rightEye.transform.position.x, rightEye.transform.position.y, rightEye.transform.position.z+0.2f);
        // Debug.Log("----------------------------------");
        // Debug.Log("----------------------------------");
        // Debug.Log("----------------------------------");
        // Debug.Log("GetSpawnPosition");

        // Debug.Log("paradigma MIN =" + gameSession.paradigma.playarea_min_x + gameSession.paradigma.playarea_min_y + gameSession.paradigma.playarea_min_z);
        // Debug.Log("paradigma MAX =" + gameSession.paradigma.playarea_max_x + gameSession.paradigma.playarea_max_y + gameSession.paradigma.playarea_max_z);
        // Debug.Log("spawnQuader MIN =" + playarea_min.ToString());
        //Debug.Log("spawnQuader MAX =" + playarea_max.ToString());
        Debug.Log("INVERT_X = " + OVRPlugin.carsten_is_invert_in_effect_X);
        Debug.Log("INVERT_Y = " + OVRPlugin.carsten_is_invert_in_effect_Y);
        Debug.Log("INVERT_Z = " + OVRPlugin.carsten_is_invert_in_effect_Z);
        Debug.Log("Right Hand = " + rightHand.transform.position.ToString());
        Debug.Log("eyeposition = " + rightEye.transform.position.ToString());
        Debug.Log("eyeposition_offset_x = " + eyeposition_offset_x.ToString());
        Debug.Log("eyeposition_offset_y = " + eyeposition_offset_y.ToString());
        Debug.Log("eyeposition_offset_z = " + eyeposition_offset_z.ToString());
        Debug.Log("spawnPosition=" + playarea_center.ToString());
        Debug.Log("spawnrightEyePosition=" + rightEye.transform.position.ToString());
        Debug.Log("playarea_min = " + playarea_min.ToString());
        Debug.Log("playarea_max = " + playarea_max.ToString());
        Debug.Log("old eye Pos = " + uninverted_right_eye.ToString());
        return(playarea_center);
    }
    
    public Vector3 GetSpawnPosition2 () {
        // gibt die Spawn Position des Balls zurueck
        // Die spawn position sollte sich in der Mitte eines Quaders befinden, der 
        // durch das Parameter File vorgegeben ist
        // es sollte sich aber immer auch in relation zu der aktuellen
        // Position des Headsets befinden
        Debug.Log("-----------------------------");
        Debug.Log("----GetSpawn Position 2------");
        
        // den offset muessen wir nur einfuehren wenn die invertierung wirklich auch im OvRPlugin angewendet wurden
        float eyeposition_offset_x = 0.0f;
        float eyeposition_offset_y = 0.0f;
        float eyeposition_offset_z = 0.0f;


        if (OVRPlugin.carsten_is_invert_in_effect_X){
            //eyeposition_offset_x = gameSession.paradigma.spiegelpunkt_der_invertierung_x;
            eyeposition_offset_x = uninverted_right_eye[0]- rightEye.transform.position.x;
        }
        if (OVRPlugin.carsten_is_invert_in_effect_Y){
//            eyeposition_offset_y = gameSession.paradigma.spiegelpunkt_der_invertierung_y;
            eyeposition_offset_y = uninverted_right_eye[1]- rightEye.transform.position.y;
    
        }
        if (OVRPlugin.carsten_is_invert_in_effect_Z){
            //eyeposition_offset_z = gameSession.paradigma.spiegelpunkt_der_invertierung_z*(-2.0f);
            eyeposition_offset_z = uninverted_right_eye[2]- rightEye.transform.position.z;
        }


        // playarea_min[0] = rightEye.transform.position.x + eyeposition_offset_x + gameSession.paradigma.playarea_min_x;
        // playarea_min[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_min_y;
        // playarea_min[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_min_z;
        // playarea_max[0] = rightEye.transform.position.x + eyeposition_offset_x +gameSession.paradigma.playarea_max_x;
        // playarea_max[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_max_y;
        // playarea_max[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_max_z;
        // playarea_center = (playarea_min+playarea_max)/2.0f;


        playarea_min[0] = rightEye.transform.position.x + eyeposition_offset_x + gameSession.paradigma.playarea_min_x;
        playarea_min[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_min_y;
        playarea_min[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_min_z;
        playarea_max[0] = rightEye.transform.position.x + eyeposition_offset_x +gameSession.paradigma.playarea_max_x;
        playarea_max[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_max_y;
        playarea_max[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_max_z;
        playarea_center = (playarea_min+playarea_max)/2.0f;


        playarea_min[0] = rightEye.transform.position.x + eyeposition_offset_x + gameSession.paradigma.playarea_min_x;
        playarea_min[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_min_y;
        playarea_min[2] = gameSession.paradigma.playarea_min_z;
        playarea_max[0] = rightEye.transform.position.x + eyeposition_offset_x +gameSession.paradigma.playarea_max_x;
        playarea_max[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_max_y;
        playarea_max[2] = gameSession.paradigma.playarea_max_z;
        playarea_center = (playarea_min+playarea_max)/2.0f;





        // playarea_min[0] = rightEye.transform.position.x + eyeposition_offset_x + gameSession.paradigma.playarea_min_x;
        // playarea_min[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_min_y;
        // playarea_min[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_min_z;
        // playarea_max[0] = rightEye.transform.position.x + eyeposition_offset_x +gameSession.paradigma.playarea_max_x;
        // playarea_max[1] = rightEye.transform.position.y + eyeposition_offset_y +gameSession.paradigma.playarea_max_y;
        // playarea_max[2] = rightEye.transform.position.z + eyeposition_offset_z +gameSession.paradigma.playarea_max_z;
        // playarea_center = (playarea_min+playarea_max)/2.0f;



//        spawnPosition = new Vector3(rightEye.transform.position.x, rightEye.transform.position.y, rightEye.transform.position.z+0.2f);
        // Debug.Log("----------------------------------");
        // Debug.Log("----------------------------------");
        // Debug.Log("----------------------------------");
        // Debug.Log("GetSpawnPosition");

        // Debug.Log("paradigma MIN =" + gameSession.paradigma.playarea_min_x + gameSession.paradigma.playarea_min_y + gameSession.paradigma.playarea_min_z);
        // Debug.Log("paradigma MAX =" + gameSession.paradigma.playarea_max_x + gameSession.paradigma.playarea_max_y + gameSession.paradigma.playarea_max_z);
        // Debug.Log("spawnQuader MIN =" + playarea_min.ToString());
        //Debug.Log("spawnQuader MAX =" + playarea_max.ToString());
        Debug.Log("INVERT_X = " + OVRPlugin.carsten_is_invert_in_effect_X);
        Debug.Log("INVERT_Y = " + OVRPlugin.carsten_is_invert_in_effect_Y);
        Debug.Log("INVERT_Z = " + OVRPlugin.carsten_is_invert_in_effect_Z);
        Debug.Log("eyeposition = " + rightEye.transform.position.ToString());
        Debug.Log("Right Hand = " + rightHand.transform.position.ToString());
 
        Debug.Log("eyeposition old Z = " + uninverted_right_eye[2].ToString());
        Debug.Log("eyeposition new Z = " + rightEye.transform.position.z.ToString());
        Debug.Log("eyeposition_offset_z = " + eyeposition_offset_z.ToString());
 
        Debug.Log("Right Hand Z org = " + uninverted_right_hand.z.ToString());
        Debug.Log("Right Hand Z new = " + rightHand.transform.position.z.ToString());
        float d = rightHand.transform.position.z-uninverted_right_hand[2];
        Debug.Log("Right Hand dist = " + d.ToString());

        Debug.Log("eyeposition = " + rightEye.transform.position.ToString());
        
        Debug.Log("eyeposition_offset_x = " + eyeposition_offset_x.ToString());
        Debug.Log("eyeposition_offset_y = " + eyeposition_offset_y.ToString());
        Debug.Log("spawnPosition=" + playarea_center.ToString());
        Debug.Log("spawnrightEyePosition=" + rightEye.transform.position.ToString());
        Debug.Log("playarea_min = " + playarea_min.ToString());
        Debug.Log("playarea_max = " + playarea_max.ToString());
        Debug.Log("old eye Pos = " + uninverted_right_eye.ToString());

        return(playarea_center);
    }

    public Vector3 get_playarea_min(){
        return(playarea_min);
    }
    public Vector3 get_playarea_max(){
        return(playarea_max);
    }

    // public float estimate_current_general_difficulty()
    // {
    //     // Very important function
    //     // This function estimates the general difficulty
    //     // this difficulty rate decides how many disturbing influences will come into play for the next ball
    //     // The estimation is heuristic with currently a very simple approach:
    //     // 1. If the player scores a hit than the difficulty should be increased by 10% 
    //     // 2. If the player misses than the difficulty stays the same unless the player has missed 1/desired_hit_rate times
    //     //     than the dificulty will be decreased by 10%
    //     // Although this seem simple, the real difficulty is in the rating of the different influences
    //     // how much difficulty will be generated by an hand offset and how much by an invertation and ...
    //     if (gameSession.playerData._Balls.Count<1)
    //     {
    //         return 10.0f;
    //     }
    //     float last5_success_rate = estimate_success_rate_of_last_x(5);
    //     //int last_ball_hit = gameSession.playerData._Balls[gameSession.playerData._Balls.Count-1].is_Hit;
    //     int last_ball_hit = 0;
    //     float diff = current_general_difficulty; 
    //     // letzter Ball war ein Treffer
    //     if (last_ball_hit>0)
    //     {
    //         diff += diff* 0.1f;
    //     }
    //     if (last5_success_rate<gameSession.paradigma.desired_hit_rate)
    //     {
    //         diff -= diff* 0.1f;
    //     }
    //     return diff;
    // }




}

public class Ball_Difficulty
{
    // diese Klasse haelt alle Informationen zum Schwierigkeitsgrad
    // das setzten des letztlichen Schwierigkeitsgrades erfolgt nicht ueber eine Skalierung der einzelwerte 
    //   z.B. der Staerke des Offsets der hand in X-Richtung sondern nur
    //   ueber das anwenden oder nicht-anwenden einer Alteration

    // es gibt ein Difficulty max ... 
    //  das ist der maximale Schwierigkeitsgrad der angeboten werden kann
    //  dieser setzt sich aus einer skalierten Bewertung der einzelnen Offset Elemente zusammen
    //    Offset hand x 1 Punkt 
    //    Offset vel  x 2 Punkte
    //    hand_tremor x 3 Punkte
    //    hand_invert x 5 Punkte
    //    diese Skalierung steht auch mit im Paradigma File
    //    diese Punkte werden als erstes vergeben.
    //    Wenn es hiermit nicht moeglich sein sollte den Schwierigkeitsgrad auf das
    //     Zielniveau anzuheben dann wird als letztes die Geschwindigkeit angepasst
    //     Fuer die Geschwindigkeit gibt es auch ein ball_veloc_diff value sowie ein min und max value
    //     der maximale Schwierigkeitsgrad bezieht sich auf die maximale Geschwindigkeit dies erlaubt 
    //     eine darstellung der Schwierigkeit der Geschwindigkeit im verhaeltnis zur Geschwindigkeit der
    //     anderen Faktoren


    // Zur Berechnung des Schwierigkeitsgrades eines neuen Balls wird ein Wert zwischen 0 und 100 uebergeben
    // Dieser WErt wird dann skaliert auf den maximalen Schwierigkeitsgrad 
    //   z.B. max Schwierigkeit = 50  uebergebener Schwierrigkeitsanforderung 60 von 100
    //        dann ist der Punktescore der hier zusammenkommen muss = 30
    // um diesen Punktwert zu erreichen wird ein Einzelelement zufaellig ausgewaehlt ... z.B. invert_Y
    // dies wird angeschaltet und der Punktwert addiert und das naechste Element zufaellig ausgewaehlt
    // wenn der Punktwert des Ziels ueberschritten wird dann werden keine weiteren Schwierigkeiten mehr zugeschaltet
    // fuer alle ausgewaehlten Schwierigkeiten wird ein Zufallswert im angegebenen Bereich berechnet 
    // und dies auf die Hand uebertragen 
    // FERTIG

    // WICHTIG
    // bei dem Invert wird auch die Position des EyeCenters veraendert
    // allerdings nicht die Position von OVRCamera Rig und ForwardDirection
    // wegen dieser Veraenderungen ist es das beste wenn die Anwendungen der HandVeraenderungen
    // erst in Kraft treten nachdem der Ball an der korrekten Position instanziert wurde
    // 

    Vector3 carsten_offset_hand_pos;
    Vector3 carsten_offset_hand_vel;
    Vector3 carsten_invert;
    Vector4 carsten_tremor;
    // Offset velocity
    
    private float offset_hand_pos_X_cur = 0.0f;
    private float offset_hand_pos_Y_cur =0.0f;
    private float offset_hand_pos_Z_cur= 0.0f; 
    private float offset_hand_vel_X_cur= 1.0f; 
    private float offset_hand_vel_Y_cur= 1.0f; 
    private float offset_hand_vel_Z_cur= 1.0f; 
    private float hand_invert_X_cur = 0.0f; 
    private float hand_invert_Y_cur = 0.0f; 
    private float hand_invert_Z_cur = 0.0f; 
    private bool is_hand_invert_X_cur = false; 
    private bool is_hand_invert_Y_cur = false; 
    private bool is_hand_invert_Z_cur = false; 
    private float hand_tremor_X_cur = 0.0f;
    private float hand_tremor_Y_cur = 0.0f;
    private float hand_tremor_Z_cur = 0.0f;
    private float hand_tremor_freq_cur = 0.0f;
    public float max_diff_score_norm;
    public float max_diff_score;
    private float ball_veloc = 0.0f;

    public float new_ball_veloc = 0.0f;

    //public List<string> adaptable_Items = new List<string>();
    public List<AdaptableItems> adaptableItems = new List<AdaptableItems>();

    public GameSession gameSession;


    public void init(GameSession mygameSession){

        gameSession = mygameSession;

        // in dem gameSession object stehen alle Parameter unter z.B. gameSession.paradigma.offset_hand_pos_X_min;
        create_adpatable_Item_list();
        // berechne die maximal moegliche Schwierigkeit in Punkten
        //max_diff_score = estimate_max_diff_score();


    }

    public bool is_hand_inverted_in_X(){ return(is_hand_invert_X_cur); }
    public bool is_hand_inverted_in_Y(){ return(is_hand_invert_Y_cur); }
    public bool is_hand_inverted_in_Z(){ return(is_hand_invert_Z_cur); }

    public void estimate_new_difficulty(float target_difficulty){
            Debug.Log("now select_qualities_for_hand_alterationselect_qualities_for_hand_alteration"); 
            Debug.Log("target difficulty = " + target_difficulty.ToString()); 
            select_qualities_for_hand_alteration(target_difficulty);
            
            Debug.Log("now estimate_quatities_for_hand_alteration()");
            estimate_quatities_for_hand_alteration();
    }

    public void create_adpatable_Item_list(){
        Debug.Log("create_adaptable_Items");
        Debug.Log("gameSession.paradigma.adapt_offset_hand_pos_X = " + gameSession.paradigma.adapt_offset_hand_pos_X);
        max_diff_score = 0.0f; 
        if (gameSession.paradigma.adapt_offset_hand_pos_X){ 
            Debug.Log("gameSession.paradigma.adapt_offset_hand_pos_X");
            max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_pos_X", 
                diff_score = gameSession.paradigma.offset_hand_pos_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max)                }
            );
        }
        if (gameSession.paradigma.adapt_offset_hand_pos_Y){ 
            max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_pos_Y", 
                diff_score = gameSession.paradigma.offset_hand_pos_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max)
                }
            );
        }
        if (gameSession.paradigma.adapt_offset_hand_pos_Z){ 
            max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_pos_Z", 
                diff_score = gameSession.paradigma.offset_hand_pos_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max)
                }
            );
        }
        if (gameSession.paradigma.adapt_offset_hand_vel_X){ 
            max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_vel_X", 
                diff_score = gameSession.paradigma.offset_hand_vel_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max)
                });
            }
        if (gameSession.paradigma.adapt_offset_hand_vel_Y){ 
            max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_vel_Y", 
                diff_score = gameSession.paradigma.offset_hand_vel_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max)
                }
            );
            }
        if (gameSession.paradigma.adapt_offset_hand_vel_Z){ 
            max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_offset_hand_vel_Z", 
                diff_score = gameSession.paradigma.offset_hand_vel_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max)
                }
            );
            }
        if (gameSession.paradigma.adapt_invert_X){ 
            max_diff_score += gameSession.paradigma.hand_invert_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_invert_X", 
                diff_score = gameSession.paradigma.hand_invert_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = 1.0f,
                }
            );
        }
        if (gameSession.paradigma.adapt_invert_Y){ 
            max_diff_score += gameSession.paradigma.hand_invert_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_invert_Y", 
                diff_score = gameSession.paradigma.hand_invert_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = 1.0f,
                }
            );
        }
        if (gameSession.paradigma.adapt_invert_Z){ 
            max_diff_score += gameSession.paradigma.hand_invert_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_invert_Z", 
                diff_score = gameSession.paradigma.hand_invert_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value = 1.0f,
                }
            );
        }
        if (gameSession.paradigma.adapt_tremor_X){ 
            max_diff_score += gameSession.paradigma.hand_tremor_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_tremor_X", 
                diff_score = gameSession.paradigma.hand_tremor_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value =  gameSession.paradigma.hand_tremor_X
                }
            );
        }
        if (gameSession.paradigma.adapt_tremor_Y){ 
            max_diff_score += gameSession.paradigma.hand_tremor_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_tremor_Y", 
                diff_score = gameSession.paradigma.hand_tremor_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value =  gameSession.paradigma.hand_tremor_Y
                }
            );
        }
        if (gameSession.paradigma.adapt_tremor_Z){ 
            max_diff_score += gameSession.paradigma.hand_tremor_diff; 
            adaptableItems.Add(new AdaptableItems() {
                name = "adapt_tremor_Z", 
                diff_score = gameSession.paradigma.hand_tremor_diff, 
                is_active = false,
                default_value = 0.0f,
                current_value =  gameSession.paradigma.hand_tremor_Z
                }
            );
        }
        if (gameSession.paradigma.adapt_veloc){
            max_diff_score += gameSession.paradigma.ball_veloc_diff;
            // fuer die Anpassung der Ballgeschwindigkeit erstelle ich kein Adaptable Item
            // da die GEschwindigkeit erst zum schluss aufgefuellt wird
            // wenn die Schwierigkeitsanforderung anders gedeckt werden kann dann wird 
            // die Geschwindigkeit auf standard gesetzt (gameSession.paradigma.ball_veloc_std)
        }


    }

    public void select_qualities_for_hand_alteration(float target_diff){
        // setzten der Information ob ein Offset Anwendung findet oder nicht
        // target_diff ist ein WErt zwischen 0 und 100 der die Zielschwierigkeit angiebt die 
        // im Verhaeltnis zu den im Parameterfile dargestellten Interventionen auf die Hand repraesentiert
        max_diff_score_norm = max_diff_score * target_diff / 100.0f;
        Debug.Log("in select_qualities_for_hand_alteration");
        Debug.Log("max_diff_score = " + max_diff_score);
        Debug.Log("target_diff = " + target_diff);
        Debug.Log("max_diff_score_norm = " + max_diff_score_norm);
        new_ball_veloc = gameSession.paradigma.ball_veloc_std;
        Random rnd = new Random();
        float current_diff_score = 0.0f;
        List<int> item_num_list = new List<int>();

        int number_of_active_invert_axes = 0;
        
        int idx = 0;
        // erstelle eine simple Liste aus aufsteigenden Nummern welche der Anzahl von adaptableItems entspricht
        for (int i = 0; i<adaptableItems.Count; i++){item_num_list.Add(i); }
        // setzte alle acivity values auf false 
        for (int i = 0; i<adaptableItems.Count; i++){adaptableItems[i].is_active = false; }


        // Zuerst kommen die nicht-adaptable Items
        // diese sind fest auf den Wert gesetzt und muessen als erstes uebertragen werden
        // hierbei handelt es sich um feste werte, die nicht in die Schwierigkeitsberechnung eingehen

        // setzte in adaptable Items Werte auf true die dann auf die Hand uebertragen werden 
        //  bis die maximale Schwierigkeit erfuellt ist
        while (current_diff_score < max_diff_score_norm && idx<adaptableItems.Count){
            Debug.Log("in while loop with current_diff_score =" + current_diff_score.ToString());
            // waehle eine Zufallszahl 
            int my_rand = Random.Range(0,item_num_list.Count-1);
            // die item_number ist nun ein zufaelliger Index von adpatable Items der
            // nicht wieder erneut auftaucht
            int item_number = item_num_list[my_rand];
            // entferne die Zahl damit sie nicht erneut auftaucht
            item_num_list.RemoveAt(my_rand);
            current_diff_score += adaptableItems[item_number].diff_score;
            adaptableItems[item_number].is_active = true;
            if (adaptableItems[item_number].name == "adapt_invert_X") {number_of_active_invert_axes++;}
            if (adaptableItems[item_number].name == "adapt_invert_Y") {number_of_active_invert_axes++;}
            if (adaptableItems[item_number].name == "adapt_invert_Z") {number_of_active_invert_axes++;}
            idx += 1;
        }
        // zum schluss auffuellen mit der Geschwindigkeit
        // hier bestimme ich dann aber auch schon die Geschwindigkeit
        if (gameSession.paradigma.adapt_veloc && current_diff_score < max_diff_score_norm){ 
            // welches Mass an Schwierigkeit fehlt noch?
            //Debug.Log("Adapt Ball velocity ... ");
            //Debug.Log("current_diff_score = " + current_diff_score);
            //Debug.Log("max_diff_score_norm = " + max_diff_score_norm);
            float req_diff = max_diff_score_norm - current_diff_score;
            if (req_diff>gameSession.paradigma.ball_veloc_diff){req_diff = gameSession.paradigma.ball_veloc_diff;}
            float ball_vel_scale = req_diff/ gameSession.paradigma.ball_veloc_diff;
            new_ball_veloc = gameSession.paradigma.ball_veloc_min + ((gameSession.paradigma.ball_veloc_max-gameSession.paradigma.ball_veloc_min)*ball_vel_scale);
            if (new_ball_veloc>gameSession.paradigma.ball_veloc_max){new_ball_veloc=gameSession.paradigma.ball_veloc_max;}
            if (new_ball_veloc<gameSession.paradigma.ball_veloc_min){new_ball_veloc=gameSession.paradigma.ball_veloc_min;}
            current_diff_score += req_diff;
        }
        // Wenn die gesamte Schwierigkeitsberechnung abgeschlossen ist wird noch geschaut ob minimum Erforderungen erfuellt sind
        // wir wollen z.B. das mindestens 1 Axis immer invertiert bleibt
        int dummy_counter=0;
        int cur_item_num = 0;
        while (number_of_active_invert_axes<gameSession.paradigma.num_invert_axis_min){
            int sel_axis = Random.Range(1, 4); // second number exclusive
            cur_item_num = 0;
            while (cur_item_num<adaptableItems.Count){
                if (sel_axis==1 && adaptableItems[cur_item_num].name == "adapt_invert_X"){
                    adaptableItems[cur_item_num].is_active = true;
                    number_of_active_invert_axes++;
                }
                if (sel_axis==2 && adaptableItems[cur_item_num].name == "adapt_invert_Y"){
                    adaptableItems[cur_item_num].is_active = true;
                    number_of_active_invert_axes++;
                }
                if (sel_axis==3 && adaptableItems[cur_item_num].name == "adapt_invert_Z"){
                    adaptableItems[cur_item_num].is_active = true;
                    number_of_active_invert_axes++;
                }
                cur_item_num++;
                   
            }
            dummy_counter++;
            if (dummy_counter>100){
                Debug.Log("ERROR With paradigm file ...");
                Debug.Log("There are less adaptable invert axis than num_invert_axis_min");
            }
        }
            


        Debug.Log("adaptableItems ...activity");
        for (int i = 0; i<adaptableItems.Count; i++){Debug.Log(adaptableItems[i].name.ToString() + " " + adaptableItems[i].is_active);}
        Debug.Log("-----------------------------------------");
    }


    public Vector3 get_offset_hand_pos(){
        Vector3 offset_hand_pos = new Vector3(offset_hand_pos_X_cur, offset_hand_pos_Y_cur, offset_hand_pos_Z_cur);
        return(offset_hand_pos);
    }
        public Vector3 get_offset_hand_vel(){
        Vector3 offset_hand_pos = new Vector3(offset_hand_vel_X_cur, offset_hand_vel_Y_cur, offset_hand_vel_Z_cur);
        return(offset_hand_pos);
    }
        public Vector3 get_invert(){
        Vector3 invert = new Vector3(hand_invert_X_cur, hand_invert_Y_cur, hand_invert_Z_cur);
        return(invert);
    }
        public Vector4 get_tremor(){
        Vector4 tremor = new Vector4(hand_tremor_X_cur, hand_tremor_Y_cur, hand_tremor_Z_cur, hand_tremor_freq_cur);
        return(tremor);
    }


    public void estimate_quatities_for_hand_alteration(){
        // setzte alles auf 0
        // das ist notwendig da in der adaptableItems liste nur die stehen die wirklich adaptiert werden und nicht unbedingt alle
        // offset_hand_pos_X_cur = 0.0f;
        // offset_hand_pos_Y_cur =0.0f;
        // offset_hand_pos_Z_cur= 0.0f; 
        // offset_hand_vel_X_cur= 1.0f; 
        // offset_hand_vel_Y_cur= 1.0f; 
        // offset_hand_vel_Z_cur= 1.0f; 
        // hand_invert_X_cur = 0.0f; 
        // hand_invert_Y_cur = 0.0f; 
        // hand_invert_Z_cur = 0.0f; 
        // hand_tremor_X_cur = 0.0f;
        // hand_tremor_Y_cur = 0.0f;
        // hand_tremor_Z_cur = 0.0f;
        // hand_tremor_freq_cur = 0.0f;
        
        // Nutzung der standardwerte .... wenn nicht adaptable werden diese dann auch nicht angepasst
        offset_hand_pos_X_cur = gameSession.paradigma.offset_hand_pos_X_min + (gameSession.paradigma.offset_hand_pos_X_max-gameSession.paradigma.offset_hand_pos_X_min)/2.0f;
        offset_hand_pos_Y_cur = gameSession.paradigma.offset_hand_pos_Y_min + (gameSession.paradigma.offset_hand_pos_Y_max-gameSession.paradigma.offset_hand_pos_Y_min)/2.0f;
        offset_hand_pos_Z_cur = gameSession.paradigma.offset_hand_pos_Z_min + (gameSession.paradigma.offset_hand_pos_Z_max-gameSession.paradigma.offset_hand_pos_Z_min)/2.0f; 
        offset_hand_vel_X_cur= gameSession.paradigma.offset_hand_vel_X_min + (gameSession.paradigma.offset_hand_vel_X_max-gameSession.paradigma.offset_hand_vel_X_min)/2.0f;
        offset_hand_vel_Y_cur= gameSession.paradigma.offset_hand_vel_Y_min + (gameSession.paradigma.offset_hand_vel_Y_max-gameSession.paradigma.offset_hand_vel_Y_min)/2.0f;
        offset_hand_vel_Z_cur= gameSession.paradigma.offset_hand_vel_Z_min + (gameSession.paradigma.offset_hand_vel_Z_max-gameSession.paradigma.offset_hand_vel_Z_min)/2.0f;
        hand_invert_X_cur = gameSession.paradigma.hand_invert_X; 
        hand_invert_Y_cur = gameSession.paradigma.hand_invert_Y; 
        hand_invert_Z_cur = gameSession.paradigma.hand_invert_Z;
        hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
        hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
        hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
        hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;

        ball_veloc = gameSession.paradigma.ball_veloc_std;
        is_hand_invert_X_cur = false;
        if (gameSession.paradigma.hand_invert_X>0){ is_hand_invert_X_cur= true;}
        is_hand_invert_Y_cur = false;
        if (gameSession.paradigma.hand_invert_Y>0){ is_hand_invert_Y_cur= true;}
        is_hand_invert_Z_cur = false;
        if (gameSession.paradigma.hand_invert_Z>0){ is_hand_invert_Z_cur= true;}

        Debug.Log("estimate hand parameter offsets()");
        // in der adaptableItems Liste stehen nur die Items die adaptierbar sind alle anderen muessen false sein

        // for (int i = 0; i<adaptableItems.Count; i++){
        //     if (adaptableItems[i].is_active){
        //         switch (adaptableItems[i].name){
        //             case "adapt_offset_hand_pos_X":
        //                 offset_hand_pos_X_cur = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max);
        //                 break;
        //             case "adapt_offset_hand_pos_Y":
        //                 offset_hand_pos_Y_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max);
        //                 break;
        //             case "adapt_offset_hand_pos_Z":
        //                 offset_hand_pos_Z_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max);
        //                 break;
        //             case "adapt_offset_hand_vel_X":
        //                 offset_hand_vel_X_cur = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max);
        //                 break;
        //             case "adapt_offset_hand_vel_Y":
        //                 offset_hand_vel_Y_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max);
        //                 break;
        //             case "adapt_offset_hand_vel_Z":
        //                 offset_hand_vel_Z_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max);
        //                 break;
        //             case "adapt_invert_X":
        //                 hand_invert_X_cur = 1.0f;
        //                 is_hand_invert_X_cur = true;
        //                 break;
        //             case "adapt_invert_Y":
        //                 hand_invert_Y_cur = 1.0f;
        //                 is_hand_invert_Y_cur = true;
        //                 break;
        //             case "adapt_invert_Z":
        //                 hand_invert_Z_cur = 1.0f;
        //                 is_hand_invert_Z_cur = true;
        //                 break;
        //             case "adapt_tremor_X":
        //                 hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
        //                 hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
        //                 break;
        //             case "adapt_tremor_Y":
        //                 hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
        //                 hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
        //                 break;
        //             case "adapt_tremor_Z":
        //                 hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
        //                 hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
        //                 break;
        //             case "adapt_veloc":
        //                 hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
        //                 hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
        //                 break;
        //         }
        //     }
        // }


        // changes 20210925
        // die adaptableItems.is_active = false wurden zuvor nicht ausgeschaltet
        for (int i = 0; i<adaptableItems.Count; i++){
            switch (adaptableItems[i].name){
                case "adapt_offset_hand_pos_X":
                    if (adaptableItems[i].is_active){
                        offset_hand_pos_X_cur = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max);
                        break;
                    }else{
                        offset_hand_pos_X_cur = 0.0f;
                        break;
                    }
                case "adapt_offset_hand_pos_Y":
                    if (adaptableItems[i].is_active){
                        offset_hand_pos_Y_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max);
                        break;
                    }else{
                        offset_hand_pos_Y_cur = 0.0f;
                        break;
                    }
                case "adapt_offset_hand_pos_Z":
                    if (adaptableItems[i].is_active){
                        offset_hand_pos_Z_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max);
                        break;
                    }else{
                        offset_hand_pos_Z_cur = 0.0f;
                        break;
                    }
                case "adapt_offset_hand_vel_X":
                    if (adaptableItems[i].is_active){
                        offset_hand_vel_X_cur = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max);
                        break;
                    }else{
                        offset_hand_vel_X_cur = 1.0f;
                        break;
                    }                    
                case "adapt_offset_hand_vel_Y":
                    if (adaptableItems[i].is_active){
                        offset_hand_vel_Y_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max);
                        break;
                    }else{
                        offset_hand_vel_Y_cur = 1.0f;
                        break;
                    }      
                case "adapt_offset_hand_vel_Z":
                    if (adaptableItems[i].is_active){
                        offset_hand_vel_Z_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max);
                        break;
                    }else{
                        offset_hand_vel_Z_cur = 1.0f;
                        break;
                    }    
                case "adapt_invert_X":
                    if (adaptableItems[i].is_active){
                        hand_invert_X_cur = 1.0f;
                        is_hand_invert_X_cur = true;
                        break;
                    }else{
                        hand_invert_X_cur = 0.0f;
                        is_hand_invert_X_cur = false;
                        break;
                    }    
                case "adapt_invert_Y":
                    if (adaptableItems[i].is_active){
                        hand_invert_Y_cur = 1.0f;
                        is_hand_invert_Y_cur = true;
                        break;
                    }else{
                        hand_invert_Y_cur = 0.0f;
                        is_hand_invert_Y_cur = false;
                        break;
                    }   
                case "adapt_invert_Z":
                    if (adaptableItems[i].is_active){
                        hand_invert_Z_cur = 1.0f;
                        is_hand_invert_Z_cur = true;
                        break;
                    }else{
                        hand_invert_Z_cur = 0.0f;
                        is_hand_invert_Z_cur = false;
                        break;
                    }    
                case "adapt_tremor_X":
                    if (adaptableItems[i].is_active){
                        hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
                        hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
                        break;
                    }else{
                        hand_tremor_X_cur = 0.0f;
                        break;
                    }    

                case "adapt_tremor_Y":
                    if (adaptableItems[i].is_active){
                        hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
                        hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
                        break;
                    }else{
                        hand_tremor_Y_cur = 0.0f;
                        break;
                    }    
                case "adapt_tremor_Z":
                    if (adaptableItems[i].is_active){
                        hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
                        hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
                        break;
                    }else{
                        hand_tremor_Z_cur = 0.0f;
                        break;
                    }   
                case "adapt_veloc":
                    if (adaptableItems[i].is_active){
                        ball_veloc = Random.Range(gameSession.paradigma.ball_size_min, gameSession.paradigma.ball_size_max); 
                        break;
                    }else{
                        ball_veloc = gameSession.paradigma.ball_veloc_std;
                        break;
                    }  
            }
            
        }

        // Vector3 offset_hand_pos = new Vector3(offset_hand_pos_X_cur, offset_hand_pos_Y_cur, offset_hand_pos_Z_cur);
        // Vector3 offset_hand_vel = new Vector3(offset_hand_vel_X_cur, offset_hand_vel_Y_cur, offset_hand_vel_Z_cur);
        // Vector3 invert = new Vector3(hand_invert_X_cur, hand_invert_Y_cur, hand_invert_Z_cur);
        // Vector4 tremor = new Vector4(hand_tremor_X_cur, hand_tremor_Y_cur, hand_tremor_Z_cur, hand_tremor_freq_cur);

    

        
            // adaptableItems[i].is_active = false; 
            // offset_hand_pos_X_cur = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max);
            // offset_hand_pos_Y_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max);
            // offset_hand_pos_Z_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max);
            // offset_hand_vel_X_cur = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max);
            // offset_hand_vel_Y_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max);
            // offset_hand_vel_Z_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max);
            // if (gameSession.paradigma.hand_invert_X>0){ hand_invert_X_cur = 1.0f; }else{ hand_invert_X_cur = 0.0f; }
            // if (gameSession.paradigma.hand_invert_Y>0){ hand_invert_Y_cur = 1.0f; }else{ hand_invert_Y_cur = 0.0f; }
            // if (gameSession.paradigma.hand_invert_Z>0){ hand_invert_Z_cur = 1.0f; }else{ hand_invert_Z_cur = 0.0f; }
            
            // hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
            // hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
            // hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
            // hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;

    }


    public class AdaptableItems{
        public string name { get; set;}
        public float diff_score { get; set;}
    //    public bool is_available {get; set;}
        public float current_value {get; set;} // aktuell nicht genutzt
        public float default_value {get; set;} // aktuell nicht genutzt
        public bool is_active {get; set;}
    
    }
}

