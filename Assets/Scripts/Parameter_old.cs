// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// //using System;
// using TMPro;

// public class Parameter : MonoBehaviour
// {
//     public GameSession gameSession;

//     public MyGameManager myGameManager;
//     public PlayBall playBall;
//     //public OVREye 
//     //public RightHandAnchor rightHandAnchor;
//     // the general difficulty specifize the probability that different 
//     // disturbing influences come together
//     public float current_general_difficulty = 10.0f;

//     public Vector3 playarea_min;
//     public Vector3 playarea_max;
//     public Vector3 playarea_center;
    
    
//     //private GameObject playBall;

//     //scaling difficulty ... each offset scales with this factor
//     public float current_difficulty_scaling = 1.0f;
//     public int counter=0;
//     public float current_target_radius;
//     public float current_ball_mass;
//     public Vector3 current_gravity;
//     public Vector3 current_force;
//     public Vector3 current_offset_hand_pos;
//     public Vector3 current_offset_hand_vel;
//     public Vector3 current_invert;
//     public Vector4 current_tremor; // der letzte Eintrag fuer die Frequenz
//     public Vector3 max_force;
//     public Vector3 max_offset_hand_pos;
//     public Vector3 max_offset_hand_vel;
//     public Vector3 max_invert;
//     public Vector4 max_tremor; // der letzte Eintrag fuer die Frequenz
//     // However every one of these influences represents an different difficulty scale which is represented
//     // in the following table   
//     //                            X Y Z
//     // current force            [ 5 5 2 ]
//     // current_offset_hand_pos  [ 2 2 2 ]
//     // current_offset_hand_vel  [ 2 2 2 ]
//     // current_invert           [ 7 7 9 ]
//     // current_tremor           [ 1 1 1 1]
//     // many of these influences make it very difficult to grap the ball but not necessarily to throw the ball
//     // make changes here for different Difficulty ratings
//     // the difficulty is however also mainly influenced by the scaling in the parameter file 
//     public Vector3 difficulty_force =           new Vector3(5.0f, 5.0f, 2.0f);
//     public Vector3 difficulty_offset_hand_pos = new Vector3(2.0f, 2.0f, 2.0f);
//     public Vector3 difficulty_offset_hand_vel = new Vector3(2.0f, 2.0f, 2.0f);
//     public Vector3 difficulty_invert =          new Vector3(7.0f, 7.0f, 9.0f);
//     public Vector4 difficulty_tremor =          new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
//     public bool isdebug = true;
//     private GameObject leftHand, rightHand, rightEye;
//     public GameObject mycenterEyeAnchor;
//     public int current_ball_ID;
//     public bool is_playarea_initialized = false;
//     public bool is_in_initializing_process = false;

//     public int punkteBlock =0;
//     public int punkteGesamt =0;
//     //public GameObject anzeigeText;
//     public GameObject anzeigeTextBlock;
//     public GameObject anzeigeTextPunkteBlock;
//     public GameObject anzeigeTextPunkteGesamt;

//     public Ball_Difficulty ballDiffcultiy; 
//     // Start is called before the first frame update

//     void Start()
//     {
//         Debug.Log("Parameter:Start()");
//         //playBall = GameObject.Find("Catmul");
//         //playBall = FindObjectOfType<PlayBall>();
//         gameSession = FindObjectOfType<GameSession>();
// //        lineObj = FindObjectOfType<LineObj>(); // Zeichnet den Zielkreis auf die Wand
//         myGameManager = FindObjectOfType<MyGameManager>();
//    //     leftHand = GameObject.Find("CustomHandLeft");
//   //      rightHand = GameObject.Find("CustomHandRight");
//         leftHand = GameObject.Find("LeftHandAnchor");
//         rightHand = GameObject.Find("RightHandAnchor");
//         rightEye = GameObject.Find("RightEyeAnchor");
//         //anzeigeTextPunkteBlock = GameObject.Find("AnzeigeTextPunkteBlock");
//         //anzeigeTextPunkteGesamt = GameObject.Find("AnzeigeTextPunkteGesamt");
//         ballDiffcultiy = new Ball_Difficulty();
//         ballDiffcultiy.init(gameSession);
//         // float xh = rightHand.transform.position.x;
//         // float yh = rightHand.transform.position.y;
//         // float zh = rightHand.transform.position.z;
        
//         // float xe = rightEye.transform.position.x;
//         // float ye = rightEye.transform.position.y;
//         // float ze = rightEye.transform.position.z;
        
//         // // offset of bounding box in relation to the head of the player
//         // float xoffset = 0.0f;
//         // float yoffset = -0.2f;
//         // float zoffset = 0.2f;

//         // xh = xe+xoffset;
//         // yh = ye+yoffset;
//         // zh = ze+zoffset;

        
//         //Debug.Log("rightHand.transform.position = " + xh + " " + yh + " "+ zh);
//         //Debug.Log("rightEye.transform.position = " + xe + " " + ye + " "+ ze);
        

//         // playarea_min = new Vector3(xh, yh, zh);
//         // playarea_max = new Vector3(xh, yh, zh);


//         current_target_radius = gameSession.paradigma.target_size;
//         current_ball_mass = gameSession.paradigma.ball_mass;
//         current_gravity = initialize_gravity();
//         // current_force = initialize_force();
//         // current_offset_hand_pos = initialize_offset_hand_pos();
//         // current_offset_hand_vel = initialize_offset_hand_vel();
//         // current_invert = new Vector3(1.0f, 1.0f, 1.0f);
//         // current_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
//         //UpdateAnchors();
//     }

//     public void push_infos(float currentFingerBallDist){
// //        Debug.Log("currentFingerBallDist =  " + currentFingerBallDist);
//         // ich berechne die Punkte in Relation zur Laenge der imaginaeren Box in der der Ball fliegt
//         float laenge =  Mathf.Abs(gameSession.paradigma.playarea_max_x - gameSession.paradigma.playarea_min_x)/2.0f;
//         // wenn der Abstand groesser ist gibt es 0 Punkte 
//         // ab dort wird der Abstand halbiert und bei jeder halbierung gibt es einen Punkt mehr (10 Schritte)
//         // wie oft (x) muss ich laenge halbieren um eine Zahl kleiner als currentFingerBallDist zu erhalten
//         // laenge/(2`x)= currentFingerBallDist
//         // laenge = currentFingerBallDist * 2^x
//         // (laenge/currentFingerBallDist) =  2^x
//         // x = log(laenge/currentFingerBallDist) zur Basis 2
//         int punkteAdd =  Mathf.RoundToInt(Mathf.Log((laenge / currentFingerBallDist),2));
//         if (punkteAdd>10){ punkteAdd = 10; }
//         if (punkteAdd <0){ punkteAdd = 0;  }
//         //Debug.Log("estimated Add Punkte="+punkteAdd  + " (  currentFingerBallDist =  " + currentFingerBallDist +")" );
//         punkteBlock += punkteAdd;
//         punkteGesamt+= punkteAdd;
//         anzeigeTextPunkteBlock.GetComponent<TextMeshPro>().SetText(punkteBlock.ToString());
//         anzeigeTextPunkteGesamt.GetComponent<TextMeshPro>().SetText(punkteGesamt.ToString());

//     }

//     void Update(){

// //         if (! is_playarea_initialized){

// // //            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)){
// //             if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.RTouch)>0){
// //                 float xe = rightEye.transform.position.x;
// //                 float ye = rightEye.transform.position.y;
// //                 float ze = rightEye.transform.position.z; 
// //                 playBall.stop_moving(); 
// //                 playarea_min[0] = xe-0.2f;
// //                 playarea_min[1] = ye-0.3f;
// //                 playarea_min[2] = ze+0.15f;
// //                 playarea_max[0] = xe+0.2f;
// //                 playarea_max[1] = ye-0.1f;
// //                 playarea_max[2] = ze+0.4f;
// //                 playarea_center = (playarea_min+playarea_max)/2.0f;
// //                 is_playarea_initialized = true;
// //                 is_in_initializing_process = false;  
// // //                string mytext = anzeigeText.GetComponent<TextMeshPro>().text;
// //           //      Debug.Log("mytext = "+ mytext);
// //                 anzeigeText.GetComponent<TextMeshPro>().SetText("Bitte versuchen sie immer mit dem Zeigefinger in Kontakt mit dem roten Ball zu bleiben.\n Der Ball wird sich bewegen sobald sie ihn beruehren");
// //             }
// //         }
//     }

//     // Update is called once per frame
//     private void UpdateAnchors()
//     {
//         //OVRPlugin.Controller.RHand.Position.x
//         // OVRPose leftEye = OVRManager.display.GetEyePose(OVREye.Left);
//         // OVRPose rightEye = OVRManager.display.GetEyePose(OVREye.Right);

//         // leftEyeAnchor.localRotation = leftEye.orientation;
//         // centerEyeAnchor.localRotation = leftEye.orientation; // using left eye for now
//         // rightEyeAnchor.localRotation = rightEye.orientation;

//         // leftEyeAnchor.localPosition = leftEye.position;
//         // centerEyeAnchor.localPosition = 0.5f * (leftEye.position + rightEye.position);
//         // rightEyeAnchor.localPosition = rightEye.position;
//         // rightEyeAnchor.localScale = new Vector3 (1, -1, 1);
//         // leftEyeAnchor.localScale = new Vector3 (1, -1, 1);
//         }
    
//     // public void set_bounding_box_to_play(){
//     //     Debug.Log("set_bounding_box_to_play()");
//     //     is_playarea_initialized = false;
//     // }

//     public void prepare_parameter_for_next_ball(int _ID){
//         Debug.Log("prepare_parameter_for_next_ball by ID="+_ID);

//         current_ball_ID = _ID;
//         int block_nr = current_ball_ID + 1;
//         anzeigeTextBlock.GetComponent<TextMeshPro>().SetText(block_nr.ToString());
        
//         float target_difficulty = 75.0f; 
//         ballDiffcultiy.set_difficulty_bits(target_difficulty);
//         ballDiffcultiy.apply_to_hand();



//         // max_invert = estimate_invert();
//         // max_tremor = estimate_tremor();
//         // OVRPlugin.carsten_invert = new Vector3(0.0f, 1.0f, 0.0f);;
//         // OVRPlugin.carsten_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);; 
//         punkteBlock = 0;
//         if (isdebug){
//             // Debug.Log("current_general_difficulty = " + current_general_difficulty);
//             // Debug.Log("max_force = " + max_force);
//             // Debug.Log("max_offset_hand_pos = " + max_offset_hand_pos);
//             // Debug.Log("max_offset_hand_vel = " + max_offset_hand_vel);
//             // Debug.Log("max_invert = " + max_invert);
//             // Debug.Log("max_tremor = " + max_tremor); 
//             // Debug.Log("current_force = " + current_force);
//             // Debug.Log("current_offset_hand_pos = " + current_offset_hand_pos);
//             // Debug.Log("current_offset_hand_vel = " + current_offset_hand_vel);
//             Debug.Log("max_invert = " + max_invert);
//             Debug.Log("max_tremor = " + max_tremor);        
//             // Debug.Log("current_invert = " + current_invert);
//             // Debug.Log("current_tremor = " + current_tremor);        
//         }
//     }
//     public void adaptHand(){
//                //rightHandAnchor.Position = 
//         // current_general_difficulty = estimate_current_general_difficulty();
//         // Debug.Log("into configure_ParameterForNextBall");
//         // current_target_radius = estimate_current_target_radius(current_target_radius);
//         // Debug.Log("current_target_radius = " + current_target_radius);
//         //* Estimate Gravity ggf. change in 
//         //current_gravity = estimate_current_gravity();

//         current_force = new Vector3(0.0f, 0.0f, 0.0f);
//         current_offset_hand_pos = new Vector3(0.0f, 0.0f, 0.0f);
//         current_offset_hand_vel = new Vector3(1.0f, 1.0f, 1.0f);
//         current_invert = new Vector3(0.0f, 0.0f, 0.0f);
//         current_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
//         OVRPlugin.carsten_offset_hand_pos = new Vector3(0.0f, 0.0f, 0.0f);
//         OVRPlugin.carsten_offset_hand_vel = new Vector3(1.0f, 1.0f, 1.0f);
//         OVRPlugin.carsten_invert = new Vector3(0.0f, 0.0f, 0.0f);;
//         OVRPlugin.carsten_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);;  

//         // max_force = estimate_current_force();
//         // max_offset_hand_pos = estimate_offset_hand_pos(current_offset_hand_pos);
//         // max_offset_hand_vel = estimate_offset_hand_vel(current_offset_hand_vel);
//         // max_invert = estimate_invert();
//         // #max_tremor = estimate_tremor(); 

//     }

//     public void configureParameterForNextBall()
//     {

//         //rightHandAnchor.Position = 
//         current_general_difficulty = estimate_current_general_difficulty();
//         Debug.Log("into configure_ParameterForNextBall");
//         current_target_radius = estimate_current_target_radius(current_target_radius);
//         Debug.Log("current_target_radius = " + current_target_radius);
//         //* Estimate Gravity ggf. change in 
//         current_gravity = estimate_current_gravity();

//         current_force = new Vector3(0.0f, 0.0f, 0.0f);
//         current_offset_hand_pos = new Vector3(0.0f, 0.0f, 0.0f);
//         current_offset_hand_vel = new Vector3(1.0f, 1.0f, 1.0f);
//         current_invert = new Vector3(0.0f, 0.0f, 0.0f);
//         current_tremor = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);


//         max_force = estimate_current_force();
//         max_offset_hand_pos = estimate_offset_hand_pos(current_offset_hand_pos);
//         max_offset_hand_vel = estimate_offset_hand_vel(current_offset_hand_vel);
//         max_invert = estimate_invert();
//         max_tremor = estimate_tremor();
//         // now all forces and offsets are estimated according to the parameter file
//         // now we will select only some of same
//         // the decision of how many is mainly influenced by the current_general_difficulty score
//         // However every one of these influences represents an different difficulty scale which is represented
//         // in the table of the general variable definitions  
//         // many of these influences make it very difficult to grap the ball but not necessarily to throw the ball
//         // choose every time a different starting point
//         set_difficulty();
//         if (isdebug){
//             Debug.Log("current_general_difficulty = " + current_general_difficulty);
//             Debug.Log("max_force = " + max_force);
//             Debug.Log("max_offset_hand_pos = " + max_offset_hand_pos);
//             Debug.Log("max_offset_hand_vel = " + max_offset_hand_vel);
//             Debug.Log("max_invert = " + max_invert);
//             Debug.Log("max_tremor = " + max_tremor); 
//             Debug.Log("current_force = " + current_force);
//             Debug.Log("current_offset_hand_pos = " + current_offset_hand_pos);
//             Debug.Log("current_offset_hand_vel = " + current_offset_hand_vel);
//             Debug.Log("current_invert = " + current_invert);
//             Debug.Log("current_tremor = " + current_tremor);        
//         }
//         counter++;
        
//     }

//     public void set_difficulty2()
//     {
//         // setzte den Schwierigkeitsgrad
//         // der Schwierigkeitsgrad besteht aus dem ein oder Ausschlaten von verschiedenen Modifikatoren
//         // die Staerke der Modifikatoren wird entsprechend der Vorgaben des Paradigma Files dann per Zufall ermittelt

//     }

//     public void set_difficulty()
//     {
//         // sets the estimated max_ interventions until the difficulty scale is full
//         int num_while_loops=0;
//         int entry_point = counter % 5;
//         float tmp_difficulity = 0.0f;
//         while (tmp_difficulity<current_general_difficulty && num_while_loops<5)
//         {
//             //Debug.Log("in while loop tmp_difficulty = " + tmp_difficulity);
//             //Debug.Log("entry point = " + entry_point);
//             for (int i = 0; i<3; i++){
//                 //Debug.Log("in for loop tmp_difficulty = " + tmp_difficulity);
//                 //Debug.Log("in for loop entry point = " + entry_point);
//                 //Debug.Log("in for loop current_force = " + current_force);
//                 //Debug.Log("in for loop current_offset_hand_pos = " + current_offset_hand_pos);
//                 //Debug.Log("in for loop current_offset_hand_vel = " + current_offset_hand_vel);
//                 //Debug.Log("in for loop current_invert = " + current_invert);
//                 //Debug.Log("in for loop current_tremor = " + current_tremor);   
//                 switch (entry_point)
//                 {
//                     case 0:
//                         if ( (max_force[i] < -Mathf.Epsilon) || (max_force[i] > Mathf.Epsilon))
//                         {
//                             current_force[i] = max_force[i];
//                             tmp_difficulity += difficulty_force[i];
//                         }
//                         break;
//                     case 1:
//                         if ((max_offset_hand_pos[i]< -Mathf.Epsilon) || (max_offset_hand_pos[i] > Mathf.Epsilon))
//                         {
//                             current_offset_hand_pos[i] = max_offset_hand_pos[i];
//                             tmp_difficulity += difficulty_offset_hand_pos[i];
//                         }
//                         break;
//                     case 2:
//                         if ((max_offset_hand_vel[i] < -Mathf.Epsilon) || (max_offset_hand_vel[i] > Mathf.Epsilon))
//                         {
//                             current_offset_hand_vel[i] = max_offset_hand_vel[i];
//                             tmp_difficulity += difficulty_offset_hand_vel[i];
//                         }
//                         break;
//                     case 3:
//                         if ((max_invert[i] < -Mathf.Epsilon) || (max_invert[i] > Mathf.Epsilon))
//                         {
//                             current_invert[i] = max_invert[i];
//                             tmp_difficulity += difficulty_invert[i];
//                         }
//                         break;
//                     case 4:
//                         if ((max_tremor[i] < -Mathf.Epsilon) || (max_tremor[i] > Mathf.Epsilon))
//                         {
//                             current_tremor[i] = max_tremor[i];
//                             tmp_difficulity += difficulty_tremor[i];
//                             current_tremor[3] = max_tremor[3]; // freqenz musss immer dabei sein
//                         }
//                         break;
//                 }
//                 //Debug.Log("After switch within for loop with tmp_difficulty = " + tmp_difficulity + " (general = " + current_general_difficulty + ")");
//                 if (tmp_difficulity>=current_general_difficulty)
//                 {
//                     //Debug.Log("tmp_difficulty>current_general_difficultz ");

//                     break;
//                 }
//             }
//             //Debug.Log("after For Loop entry point = " + entry_point);
//             entry_point +=1;
//             entry_point = entry_point % 5;
//             num_while_loops ++;
//             //Debug.Log("after For Loop increased entry point = " + entry_point);
//         }
//     }
    

//     public float estimate_current_general_difficulty()
//     {
//         // Very important function
//         // This function estimates the general difficulty
//         // this difficulty rate decides how many disturbing influences will come into play for the next ball
//         // The estimation is heuristic with currently a very simple approach:
//         // 1. If the player scores a hit than the difficulty should be increased by 10% 
//         // 2. If the player misses than the difficulty stays the same unless the player has missed 1/desired_hit_rate times
//         //     than the dificulty will be decreased by 10%
//         // Although this seem simple, the real difficulty is in the rating of the different influences
//         // how much difficulty will be generated by an hand offset and how much by an invertation and ...
//         if (gameSession.playerData._Balls.Count<1)
//         {
//             return 10.0f;
//         }
//         float last5_success_rate = estimate_success_rate_of_last_x(5);
//         //int last_ball_hit = gameSession.playerData._Balls[gameSession.playerData._Balls.Count-1].is_Hit;
//         int last_ball_hit = 0;
//         float diff = current_general_difficulty; 
//         // letzter Ball war ein Treffer
//         if (last_ball_hit>0)
//         {
//             diff += diff* 0.1f;
//         }
//         if (last5_success_rate<gameSession.paradigma.desired_hit_rate)
//         {
//             diff -= diff* 0.1f;
//         }
//         return diff;
//     }



//     public Vector3 get_gravitiy(){
//         return current_gravity;
//     }

//     public Vector3 estimate_current_gravity()
//     {
//         float gravity_X = Random.Range(gameSession.paradigma.gravity_X_min, gameSession.paradigma.gravity_X_max);
//         float gravity_Y = Random.Range(gameSession.paradigma.gravity_Y_min, gameSession.paradigma.gravity_Y_max);
//         float gravity_Z = Random.Range(gameSession.paradigma.gravity_Z_min, gameSession.paradigma.gravity_Z_max);
//         //Physics.gravity = new Vector3(gravity_X, gravity_Y, gravity_Z);
//         return new Vector3(gravity_X, gravity_Y, gravity_Z);
        
//     }

//     public Vector3 estimate_current_force()
//     {
//         float force_X = Random.Range(gameSession.paradigma.force_X_min, gameSession.paradigma.force_X_max);
//         float force_Y = Random.Range(gameSession.paradigma.force_Y_min, gameSession.paradigma.force_Y_max);
//         float force_Z = Random.Range(gameSession.paradigma.force_Z_min, gameSession.paradigma.force_Z_max);
//         //Physics.force = new Vector3(force_X, force_Y, force_Z);
//         return new Vector3(force_X, force_Y, force_Z);
        
//     }

//     public Vector3 estimate_offset_hand_pos(Vector3 old_offset_hand_pos){
//         Vector3 new_offset_hand_pos = old_offset_hand_pos;
//         if (gameSession.paradigma.Adaptive==0){
//             return new Vector3(0.0f, 0.0f, 0.0f);

//         }
//         else{
//             float new_offset_hand_pos_X = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max) * current_difficulty_scaling;
//             float new_offset_hand_pos_Y = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max) * current_difficulty_scaling;
//             float new_offset_hand_pos_Z = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max) * current_difficulty_scaling;
//             new_offset_hand_pos = new Vector3(new_offset_hand_pos_X, new_offset_hand_pos_Y, new_offset_hand_pos_Z);
//             //Debug.Log("in Parameter new_offst_hand_pos = " + new_offset_hand_pos);
//         }    
//         return new_offset_hand_pos;
//     }

//     public Vector3 estimate_offset_hand_vel(Vector3 old_offset_hand_vel){
//         Vector3 new_offset_hand_vel = old_offset_hand_vel;
//         if (gameSession.paradigma.Adaptive==0){
//             return new Vector3(1.0f, 1.0f, 1.0f);

//         }
//         else{
//             float new_offset_hand_vel_X = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max) * current_difficulty_scaling;
//             float new_offset_hand_vel_Y = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max) * current_difficulty_scaling;
//             float new_offset_hand_vel_Z = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max) * current_difficulty_scaling;
//             new_offset_hand_vel = new Vector3(new_offset_hand_vel_X, new_offset_hand_vel_Y, new_offset_hand_vel_Z);
//             //Debug.Log("in Parameter new_offst_hand_pos = " + new_offset_hand_vel);
//         }    
//         return new_offset_hand_vel;
//     }

//     public Vector3 estimate_invert()
//     {
//         Debug.Log("hand_invert_X = "+ gameSession.paradigma.hand_invert_X);
//         return new Vector3(gameSession.paradigma.hand_invert_X, gameSession.paradigma.hand_invert_Y, gameSession.paradigma.hand_invert_Z);
//         // Vector3 new_invert = new Vector3(1.0f, 1.0f, 1.0f);
//         // if (gameSession.paradigma.hand_invert_X>0) {
//         //     int rnd = Random.Range(1,2);
//         //     switch (rnd)
//         //     {
//         //         case 1:
//         //             new_invert = new Vector3(1.0f, 1.0f, 1.0f);
//         //             break;
//         //         case 2:
//         //             new_invert = new Vector3(1.0f, -1.0f, 1.0f);
//         //             break;
//         //         case 3:
//         //             new_invert = new Vector3(-1.0f, 1.0f, 1.0f);
//         //             break;
//         //         case 4:
//         //             new_invert = new Vector3(-1.0f, -1.0f, 1.0f);
//         //             break;
//         //         case 5:
//         //             new_invert = new Vector3(1.0f, 1.0f, -1.0f);
//         //             break;
//         //         case 6:
//         //             new_invert = new Vector3(1.0f, -1.0f, -1.0f);
//         //             break;
//         //         case 7:
//         //             new_invert = new Vector3(-1.0f, 1.0f, -1.0f);
//         //             break;
//         //         case 8:
//         //             new_invert = new Vector3(-1.0f, -1.0f, -1.0f);
//         //             break;
//         //     }
//         // }
//         // return new_invert; 
//     }

//     public Vector4 estimate_tremor()
//     {
//         return new Vector4(gameSession.paradigma.hand_tremor_X, gameSession.paradigma.hand_tremor_X, gameSession.paradigma.hand_tremor_Z, gameSession.paradigma.hand_tremor_freq);

//     }

//     // ToDo 
//     public float estimate_success_rate_all(){
//         float rate = -1.0f;
//         if (gameSession.playerData._Balls.Count>2) 
//         {
//             //Debug.Log("num_hits = " + gameSession.playerData.num_hits);
//             Debug.Log("ball count = " + gameSession.playerData._Balls.Count);
//             //rate = (float)gameSession.playerData.num_hits/gameSession.playerData._Balls.Count;
//             rate = 0.0f;
//         }
//         Debug.Log("estimated Success rate in Parameter = " + rate);
//         return rate;
//     }
//     // ToDo 
//     public float estimate_success_rate_of_last_x(int num_x){
//         // float rate = -1.0f;
//         // int tmp_num_hits = 0;
//         // // wenn nicht genuegend Baelle geworfen wurden dann greife auf alle verfuegbaren zurueck
//         // if (gameSession.playerData._Balls.Count<num_x)
//         // {
//         //     return estimate_success_rate_all();
//         // }
//         // if (gameSession.playerData._Balls.Count>=num_x) 
//         // {
//         //     for (int i = gameSession.playerData._Balls.Count-1; i>gameSession.playerData._Balls.Count-num_x-1; i--)
//         //     {
//         //         tmp_num_hits += gameSession.playerData._Balls[i].is_Hit;
//         //     }

//         //     rate = (float)tmp_num_hits/(float)num_x;
//         // }
//         // Debug.Log("estimated Success rate in Parameter of last " + num_x + " = " + rate);
//         // return rate;
//         return 0.0f;
//     }
//     // ToDo 
//     public float estimate_current_target_radius(float old_target_radius){
//         // // Anpassung nur wenn schon min 3 Baelle geworfen wurden
//         // float new_target_radius = old_target_radius;
//         // int num_balls = gameSession.playerData._Balls.Count;
//         // if (num_balls>=3){
//         //     if (estimate_success_rate_all()>gameSession.paradigma.desired_hit_rate) {
//         //         // verkleinere nur wenn der letzte BAll ein Treffer war
//         //         if (gameSession.playerData._Balls[num_balls-1].is_Hit==1){
//         //             new_target_radius= old_target_radius - old_target_radius * 0.1f;
//         //         }
//         //     }
//         //     else {
//         //         // vergroessere nur wenn der letzte Ball ein Fehler war
//         //         if (gameSession.playerData._Balls[num_balls-1].is_Hit==0){
//         //             new_target_radius= old_target_radius + old_target_radius * 0.1f;
//         //         }
//         //     }
//         // }
        
//         // if (new_target_radius>=0.5f)
//         // {
//         //     new_target_radius = 0.5f;
//         // }
//         // return new_target_radius;
//         return 0.0f;
//     }

//     private Vector3 initialize_gravity(){
//         float gravity_X = Random.Range(gameSession.paradigma.gravity_X_min, gameSession.paradigma.gravity_X_max);
//         float gravity_Y = Random.Range(gameSession.paradigma.gravity_Y_min, gameSession.paradigma.gravity_Y_max);
//         float gravity_Z = Random.Range(gameSession.paradigma.gravity_Z_min, gameSession.paradigma.gravity_Z_max);
//         //Physics.gravity = new Vector3(gravity_X, gravity_Y, gravity_Z);
//         return new Vector3(gravity_X, gravity_Y, gravity_Z);
           
//     }

//     private Vector3 initialize_force(){
//         return new Vector3(0.0f, 0.0f, 0.0f);
           
//     }

//     private Vector4 initialize_offset_hand_pos()
//     {
//         return new Vector4(1.0f, 0.0f, 0.0f, 5.0f);
//     }
//     private Vector3 initialize_offset_hand_vel()
//     {
//             float new_offset_hand_vel_X = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max) * current_difficulty_scaling;
//             float new_offset_hand_vel_Y = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max) * current_difficulty_scaling;
//             float new_offset_hand_vel_Z = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max) * current_difficulty_scaling;
//             return new Vector3(new_offset_hand_vel_X, new_offset_hand_vel_Y, new_offset_hand_vel_Z);
            
//     }

// }

// public class Ball_Difficulty
// {
//     // diese Klasse haelt alle Informationen zum Schwierigkeitsgrad
//     // das setzten des letztlichen Schwierigkeitsgrades erfolgt nicht ueber eine Skalierung der einzelwerte 
//     //   z.B. der Staerke des Offsets der hand in X-Richtung sondern nur
//     //   ueber das anwenden oder nicht-anwenden einer Alteration

//     // es gibt ein Difficulty max ... 
//     //  das ist der maximale Schwierigkeitsgrad der angeboten werden kann
//     //  dieser setzt sich aus einer skalierten Bewertung der einzelnen Offset Elemente zusammen
//     //    Offset hand x 1 Punkt 
//     //    Offset vel  x 2 Punkte
//     //    hand_tremor x 3 Punkte
//     //    hand_invert x 5 Punkte
//     //    diese Skalierung steht auch mit im Paradigma File

//     // Zur Berechnung des Schwierigkeitsgrades eines neuen Balls wird ein Wert zwischen 0 und 100 uebergeben
//     // Dieser WErt wird dann skaliert auf den maximalen Schwierigkeitsgrad 
//     //   z.B. max Schwierigkeit = 50  uebergebener Schwierrigkeitsanforderung 60 von 100
//     //        dann ist der Punktescore der hier zusammenkommen muss = 30
//     // um diesen Punktwert zu erreichen wird ein Einzelelement zufaellig ausgewaehlt ... z.B. invert_Y
//     // dies wird angeschaltet und der Punktwert addiert und das naechste Element zufaellig ausgewaehlt
//     // wenn der Punktwert des Ziels ueberschritten wird dann werden keine weiteren Schwierigkeiten mehr zugeschaltet
//     // fuer alle ausgewaehlten Schwierigkeiten wird ein Zufallswert im angegebenen Bereich berechnet 
//     // und dies auf die Hand uebertragen 
//     // FERTIG

//     // Offset velocity
//     public float offset_hand_pos_X_cur;
//     public float offset_hand_pos_Y_cur;
//     public float offset_hand_pos_Z_cur;

//     // Offset Position
//     public float offset_hand_vel_X_cur;
//     public float offset_hand_vel_Y_cur;
//     public float offset_hand_vel_Z_cur;

//     // Hand Invert
//     public float hand_invert_X_cur;
//     public float hand_invert_Y_cur;
//     public float hand_invert_Z_cur;


//     // Hand Tremor
//     public float hand_tremor_X_cur;
//     public float hand_tremor_Y_cur;
//     public float  hand_tremor_Z_cur;
//     public float hand_tremor_X_bit;
//     public float hand_tremor_Y_bit;
//     public float  hand_tremor_Z_bit;

//     public float hand_tremor_freq_cur;    
//     public float max_diff_score_norm;

//     public float max_diff_score;

//     //public List<string> adaptable_Items = new List<string>();
//     public List<AdaptableItems> adaptableItems = new List<AdaptableItems>();


//     public GameSession gameSession;


//     public void init(GameSession mygameSession){

//         gameSession = mygameSession;
//         // in dem gameSession object stehen alle Parameter unter z.B. gameSession.paradigma.offset_hand_pos_X_min;
//         create_adpatable_Item_list();
//         // berechne die maximal moegliche Schwierigkeit in Punkten
//         //max_diff_score = estimate_max_diff_score();


//     }

//     public void adapt_hand_to_difficulty(float target_difficulty){
//             set_difficulty_bits(target_difficulty);
//             apply_to_hand();
//     }

//     public void create_adpatable_Item_list(){
//         max_diff_score = 0.0f; 
//         if (gameSession.paradigma.adapt_offset_hand_pos_X){ 
//             max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_pos_X", 
//                 diff_score = gameSession.paradigma.offset_hand_pos_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max)                }
//             );
//         }
//         if (gameSession.paradigma.adapt_offset_hand_pos_Y){ 
//             max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_pos_Y", 
//                 diff_score = gameSession.paradigma.offset_hand_pos_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max)
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_offset_hand_pos_Z){ 
//             max_diff_score += gameSession.paradigma.offset_hand_pos_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_pos_Z", 
//                 diff_score = gameSession.paradigma.offset_hand_pos_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max)
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_offset_hand_vel_X){ 
//             max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_vel_X", 
//                 diff_score = gameSession.paradigma.offset_hand_vel_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max)
//                 });
//             }
//         if (gameSession.paradigma.adapt_offset_hand_vel_Y){ 
//             max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_vel_Y", 
//                 diff_score = gameSession.paradigma.offset_hand_vel_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max)
//                 }
//             );
//             }
//         if (gameSession.paradigma.adapt_offset_hand_vel_Z){ 
//             max_diff_score += gameSession.paradigma.offset_hand_vel_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_offset_hand_vel_Z", 
//                 diff_score = gameSession.paradigma.offset_hand_vel_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max)
//                 }
//             );
//             }
//         if (gameSession.paradigma.adapt_invert_X){ 
//             max_diff_score += gameSession.paradigma.hand_invert_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_invert_X", 
//                 diff_score = gameSession.paradigma.hand_invert_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = 1.0f,
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_invert_Y){ 
//             max_diff_score += gameSession.paradigma.hand_invert_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_invert_Y", 
//                 diff_score = gameSession.paradigma.hand_invert_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = 1.0f,
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_invert_Z){ 
//             max_diff_score += gameSession.paradigma.hand_invert_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_invert_Z", 
//                 diff_score = gameSession.paradigma.hand_invert_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value = 1.0f,
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_tremor_X){ 
//             max_diff_score += gameSession.paradigma.hand_tremor_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_tremor_X", 
//                 diff_score = gameSession.paradigma.hand_tremor_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value =  gameSession.paradigma.hand_tremor_X
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_tremor_Y){ 
//             max_diff_score += gameSession.paradigma.hand_tremor_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_tremor_Y", 
//                 diff_score = gameSession.paradigma.hand_tremor_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value =  gameSession.paradigma.hand_tremor_Y
//                 }
//             );
//         }
//         if (gameSession.paradigma.adapt_tremor_Z){ 
//             max_diff_score += gameSession.paradigma.hand_tremor_diff; 
//             adaptableItems.Add(new AdaptableItems() {
//                 name = "adapt_tremor_Z", 
//                 diff_score = gameSession.paradigma.hand_tremor_diff, 
//                 is_active = false,
//                 default_value = 0.0f,
//                 current_value =  gameSession.paradigma.hand_tremor_Z
//                 }
//             );
//         }

//     }

//     public void set_difficulty_bits(float target_diff){
//         // setzten der Information ob ein Offset Anwendung findet oder nicht
//         // target_diff ist ein WErt zwischen 0 und 100 der die Zielschwierigkeit angiebt die 
//         // im Verhaeltnis zu den im Parameterfile dargestellten Interventionen auf die Hand repraesentiert
//         max_diff_score_norm = max_diff_score * target_diff / 100.0f;
//         Random rnd = new Random();
//         float current_diff_score = 0.0f;
//         List<int> item_num_list = new List<int>();
        
//         int idx = 0;
//         // erstelle eine simple Liste aus aufsteigenden Nummern welche der Anzahl von adaptableItems entspricht
//         for (int i = 0; i<adaptableItems.Count; i++){item_num_list.Add(i); }
//         // setzte alle acivity values auf false 
//         for (int i = 0; i<adaptableItems.Count; i++){adaptableItems[i].is_active = false; }

//         // setzte in adaptable Items Werte auf true die dann auf die Hand uebertragen werden 
//         //  bis die maximale Schwierigkeit erfuellt ist
//         while (current_diff_score < max_diff_score_norm && idx<adaptableItems.Count){
//             // waehle eine Zufallszahl 
//             int my_rand = Random.Range(0,item_num_list.Count-1);
//             // die item_number ist nun ein zufaelliger Index von adpatable Items der
//             // nicht wieder erneut auftaucht
//             int item_number = item_num_list[my_rand];
//             // entferne die Zahl damit sie nicht erneut auftaucht
//             item_num_list.RemoveAt(my_rand);
//             current_diff_score += adaptableItems[item_number].diff_score;
//             adaptableItems[item_number].is_active = true;
//             idx += 1;
//         }
//     }

//     public void apply_to_hand(){
//         // setzte alles auf 0
//         // das ist notwendig da in der adaptableItems liste nur die stehen die wirklich adaptiert werden und nicht unbedingt alle
//         offset_hand_pos_X_cur = 0.0f;
//         offset_hand_pos_Y_cur =0.0f;
//         offset_hand_pos_Z_cur= 0.0f; 
//         offset_hand_vel_X_cur= 1.0f; 
//         offset_hand_vel_Y_cur= 1.0f; 
//         offset_hand_vel_Z_cur= 1.0f; 
//         hand_invert_X_cur = 0.0f; 
//         hand_invert_Y_cur = 0.0f; 
//         hand_invert_Z_cur = 0.0f; 
//         hand_tremor_X_cur = 0.0f;
//         hand_tremor_Y_cur = 0.0f;
//         hand_tremor_Z_cur = 0.0f;
//         hand_tremor_freq_cur = 0.0f;


//         for (int i = 0; i<adaptableItems.Count; i++){
//             if (adaptableItems[i].is_active){
//                 switch (adaptableItems[i].name){
//                     case "adapt_offset_hand_pos_X":
//                         offset_hand_pos_X_cur = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max);
//                         break;
//                     case "adapt_offset_hand_pos_Y":
//                         offset_hand_pos_Y_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max);
//                         break;
//                     case "adapt_offset_hand_pos_Z":
//                         offset_hand_pos_Z_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max);
//                         break;
//                     case "adapt_offset_hand_vel_X":
//                         offset_hand_vel_X_cur = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max);
//                         break;
//                     case "adapt_offset_hand_vel_Y":
//                         offset_hand_vel_Y_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max);
//                         break;
//                     case "adapt_offset_hand_vel_Z":
//                         offset_hand_vel_Z_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max);
//                         break;
//                      case "adapt_invert_X":
//                         hand_invert_X_cur = 1.0f;
//                         break;
//                     case "adapt_invert_Y":
//                         hand_invert_Y_cur = 1.0f;
//                         break;
//                     case "adapt_invert_Z":
//                         hand_invert_Z_cur = 1.0f;
//                         break;
//                     case "adapt_tremor_X":
//                         hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
//                         hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
//                         break;
//                     case "adapt_tremor_Y":
//                         hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
//                         hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
//                         break;
//                     case "adapt_tremor_Z":
//                         hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
//                         hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;
//                         break;
                     
//                 }
//             }
//         }
//         OVRPlugin.carsten_offset_hand_pos = new Vector3(offset_hand_pos_X_cur, offset_hand_pos_Y_cur, offset_hand_pos_Z_cur);
//         OVRPlugin.carsten_offset_hand_vel = new Vector3(offset_hand_vel_X_cur, offset_hand_vel_Y_cur, offset_hand_vel_Z_cur);
//         OVRPlugin.carsten_invert = new Vector3(hand_invert_X_cur, hand_invert_Y_cur, hand_invert_Z_cur);
//         OVRPlugin.carsten_tremor = new Vector4(hand_tremor_X_cur, hand_tremor_Y_cur, hand_tremor_Z_cur, hand_tremor_freq_cur);

//             // adaptableItems[i].is_active = false; 
//             // offset_hand_pos_X_cur = Random.Range(gameSession.paradigma.offset_hand_pos_X_min, gameSession.paradigma.offset_hand_pos_X_max);
//             // offset_hand_pos_Y_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Y_min, gameSession.paradigma.offset_hand_pos_Y_max);
//             // offset_hand_pos_Z_cur = Random.Range(gameSession.paradigma.offset_hand_pos_Z_min, gameSession.paradigma.offset_hand_pos_Z_max);
//             // offset_hand_vel_X_cur = Random.Range(gameSession.paradigma.offset_hand_vel_X_min, gameSession.paradigma.offset_hand_vel_X_max);
//             // offset_hand_vel_Y_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Y_min, gameSession.paradigma.offset_hand_vel_Y_max);
//             // offset_hand_vel_Z_cur = Random.Range(gameSession.paradigma.offset_hand_vel_Z_min, gameSession.paradigma.offset_hand_vel_Z_max);
//             // if (gameSession.paradigma.hand_invert_X>0){ hand_invert_X_cur = 1.0f; }else{ hand_invert_X_cur = 0.0f; }
//             // if (gameSession.paradigma.hand_invert_Y>0){ hand_invert_Y_cur = 1.0f; }else{ hand_invert_Y_cur = 0.0f; }
//             // if (gameSession.paradigma.hand_invert_Z>0){ hand_invert_Z_cur = 1.0f; }else{ hand_invert_Z_cur = 0.0f; }
            
//             // hand_tremor_X_cur = gameSession.paradigma.hand_tremor_X;
//             // hand_tremor_Y_cur = gameSession.paradigma.hand_tremor_Y;
//             // hand_tremor_Z_cur = gameSession.paradigma.hand_tremor_Z;
//             // hand_tremor_freq_cur = gameSession.paradigma.hand_tremor_freq;

//     }


//     public class AdaptableItems{
//         public string name { get; set;}
//         public float diff_score { get; set;}
//     //    public bool is_available {get; set;}
//         public float current_value {get; set;} // aktuell nicht genutzt
//         public float default_value {get; set;} // aktuell nicht genutzt
//         public bool is_active {get; set;}
    
//     }
// }

