
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayBall : MonoBehaviour {

	// Use the transforms of GameObjects in 3d space as your points or define array with desired points
	//public Transform[] points;

    List<Vector3> waypointList = new List<Vector3>();
	// Store points on the Catmull curve so we can visualize them
	List<Vector3> catmullRomSplinePoints = new List<Vector3>();
	
	// How many points you want on the curve
	//uint numberOfPoints = 300;
	private Vector3 playarea_min;
	private Vector3 playarea_max;
	// Parametric constant: 0.0 for the uniform spline, 0.5 for the centripetal spline, 1.0 for the chordal spline
	public float alpha = 0.5f;
    // Start is called before the first frame update
    int idx = 0;
   // public Paradigma paradigma;
    private GameObject leftHand, rightHand, rightEye;
    private LineRenderer myline;
    private float time_ball_active = 0.0f;
    public float currentFingerBallDist;
    public bool is_waitForStart = false;
    public bool is_stopped = true;
    public bool is_invert_applied = false;
    public bool is_active = false;
    public MyGameManager myGameManager;
    public GameSession gameSession;
    private int ID;
    private float ball_size;
    private float ball_vel;
    private float time_start_moving;
    public Parameter parameter;
    private float cur_ball_vel;
    private Vector3 playarea_center; 
    private float ball_duration;
    private bool is_vel_adaptable;
    private float ball_vel_max;
    private int punkte_in_frame;
    public PunkteHistory punkteHistory;
    
    // es wird hier nicht immer ein neuer Ball gespawned sondern wir
    // verwenden hier den gleichen Ball

    void Awake(){
        Debug.Log("Awake PlayBall");
         Application.targetFrameRate = 60;
        //rightHand = GameObject.Find("RightHandAnchor");
        myGameManager = FindObjectOfType<MyGameManager>();
        gameSession = FindObjectOfType<GameSession>();
        parameter = FindObjectOfType<Parameter>();
        rightHand = GameObject.Find("hands:b_r_index_ignore");
        
        ID = parameter.current_ball_ID;
        // registriere den Ball ... this adds the ball by ID to the playerData
        gameSession.register_new_Ball(ID);
        rightEye = GameObject.Find("RightEyeAnchor");
        // die Bounding Box wird bei jedem Ball neu gesetzt falls der Player sich mit der Zeit 
        // anders setzt

        // moved to touch of ball 04.07.2021
        // current difficulty wird angewendet (nicht berechnet)
        // parameter.apply_current_difficulty_to_hand();
                
        // setting the playarea in which the object travels
        // Vector3 min = new Vector3(0.0f, -1.0f, 0.2f);
        // Vector3 max = new Vector3(0.2f, -0.5f, 0.5f);
        // set_bounding_box(min, max);
        //LineRenderer tmpline = gameObject.AddComponent<LineRenderer>();
        //myline = tmpline;
        myline = gameObject.AddComponent<LineRenderer>();
//        myline.AddComponent<LineRenderer>();
        myline.SetWidth(0.0005f, 0.0005f);
        //myline.SetPosition(0,min);
        //myline.SetPosition(0,max);
        ball_size = gameSession.paradigma.ball_size_max;
        //ball_vel = gameSession.paradigma.ball_veloc_max;
        ball_duration = gameSession.paradigma.ball_duration;
        Debug.Log("PlayBall:Awake:ball_duration = "+ ball_duration);
        transform.localScale = new Vector3(ball_size, ball_size, ball_size);
        cur_ball_vel = gameSession.paradigma.ball_veloc_std;
        is_vel_adaptable = gameSession.paradigma.adapt_veloc_in_trial;
        ball_vel_max = gameSession.paradigma.ball_veloc_in_trial_max;
        //target_diff = parameter.cur_target_difficulty;
        punkteHistory = new PunkteHistory();
        Debug.Log("gameSession.paradigma.adapt_veloc_in_trial_based_on_last_framenum"+gameSession.paradigma.adapt_veloc_in_trial_based_on_last_framenum.ToString());
        punkteHistory.setHistoryLength(Mathf.RoundToInt(gameSession.paradigma.adapt_veloc_in_trial_based_on_last_framenum));
        waitForStart();
    }
    void Start(){
        Debug.Log("STart PlayBall");
        
    }
  

    IEnumerator reinitiate_ball_with_invert()
    {
        // der Ball ist bereits in einer Position initiiert worden
        // es wurde dann darauf gewartet, dass der Finger den Ball beruehrt
        // diese Coroutine wurde dann gestartet um die moegliche Invertierung
        // auf die Hand vorzunehmen
            Debug.Log("before parameter.apply_current_difficulty_to_hand()");
            Debug.Log("rightHand.transform.position="+ rightHand.transform.position.ToString());
            Debug.Log("Ball.transform.position="+ transform.position.ToString());
            parameter.apply_current_difficulty_to_hand();
            yield return new WaitForSeconds(0.05f);
            Debug.Log("after1 ... rightHand.transform.position="+ rightHand.transform.position.ToString());
            Debug.Log("after1 ... Ball.transform.position="+ transform.position.ToString());

            playarea_center = parameter.GetSpawnPosition2();
            transform.position = playarea_center;
            playarea_min = parameter.get_playarea_min();
            playarea_max = parameter.get_playarea_max();
            Debug.Log("after2 ... rightHand.transform.position="+ rightHand.transform.position.ToString());
            Debug.Log("after2 ... Ball.transform.position="+ transform.position.ToString());
            // die berechnete initiale Ballgeschwindigkeit
            cur_ball_vel = parameter.ballDifficulty.new_ball_veloc;
            // setze die Startzeit des Balls
            time_start_moving = Time.time;
            Debug.Log("time_start_moving = " + time_start_moving);
            is_waitForStart = false;
            is_stopped = false;
            is_active = true;

    }


	void Update()
	{

        currentFingerBallDist = Vector3.Distance(rightHand.transform.position, transform.position);
        myline.SetPosition(0, rightHand.transform.position);
        myline.SetPosition(1, transform.position);
        // waiting that the finger comes near
        if (is_waitForStart && !is_invert_applied) {
            //Debug.Log("Catmul:Update is_waitForStart() with currentFingerBallDist = " + currentFingerBallDist);
            if (currentFingerBallDist<0.01f){
                // ####################
                // Der start des Balls
                // current difficulty wird angewendet (nicht berechnet)
                is_invert_applied = true;
                StartCoroutine(reinitiate_ball_with_invert());


            }
        }
        if (!is_stopped){
            while (waypointList.Count<5){
                addwaypoint(playarea_min, playarea_max);
            }
            if (idx<catmullRomSplinePoints.Count){
                transform.position = new Vector3(catmullRomSplinePoints[idx][0], catmullRomSplinePoints[idx][1], catmullRomSplinePoints[idx].z);
                idx++;
            }else{
                idx = 0;
                waypointList.RemoveAt(idx);
                // in trial velocity adaptation
                if (is_vel_adaptable){
                    cur_ball_vel = adapt_vel_in_trial(cur_ball_vel);
                }
                CatmulRom(waypointList, cur_ball_vel);
            }
            // hier sollten noch andere Parameter
            time_ball_active = Time.time-time_start_moving;
            Debug.Log("estimate time_ball_active = " + time_ball_active + " ball_duration = "+ ball_duration);
            gameSession.add_Ball_Hand_Position(ID, transform.position, rightHand.transform.position, Time.time, time_ball_active);
            //Debug.Log("Ball Position = " + transform.position);
            //Debug.Log("Hand Position = " + rightHand.transform.position);
            // gebe die Infos an parameter Klasse und an die punkteHistory Klasse weiter

            punkteHistory.add_punkte(currentFingerBallDist, ball_size, time_ball_active);
            Debug.Log("punkteHistorz.getHitRAte = " +  punkteHistory.get_hit_rate());
            parameter.push_infos(currentFingerBallDist, punkteHistory.get_hit_rate());
            if ( time_ball_active>ball_duration){
                parameter.register_finished_block();
                parameter.lastBallEndVeloc = cur_ball_vel;
                parameter.reset_hand();
                is_stopped = true;
                is_active = false;
                gameSession.SaveIntoJson();
                Debug.Log("time_ball_active>ball_duration");
                StartCoroutine(save_and_destroy());
            }
        }
	}
    
    public float adapt_vel_in_trial(float cur_ball_vel){
        // passe die Ballgeschwindigkeit an
        // die neue Ballgeschwindigkeit
        float ball_v = cur_ball_vel;
        float previous_hit_rate = punkteHistory.get_hit_rate();
        // wie stark soll die Ballgeschwindigkeit in einem Frame maximal erhoeht werden?
        // berechne erst die Varianz
        float vel_var = gameSession.paradigma.ball_veloc_in_trial_max-gameSession.paradigma.ball_veloc_min;
        // 180 waeren 3 sekunden mit 60 frames d.h. volle Geschwindigkeitsaenderung waere ueber
        // einen Bereich von 3 Sekunden moeglich
        float vel_var_per_frame = vel_var/80.0f;
//        Debug.Log("hit rate = " + punkteHistory.get_hit_rate());
        if (punkteHistory.get_hit_rate()<gameSession.paradigma.desired_hit_rate){
            ball_v -= vel_var_per_frame;
        }else{
            ball_v += vel_var_per_frame;
        }
        if (ball_v > gameSession.paradigma.ball_veloc_in_trial_max){
            ball_v = gameSession.paradigma.ball_veloc_in_trial_max;
        }
        if (ball_v <gameSession.paradigma.ball_veloc_in_trial_min){
            ball_v = gameSession.paradigma.ball_veloc_in_trial_min;
        }
        //target_diff = parameter.cur_target_difficulty;
        return (ball_v);
    }

    public void waitForStart(){
        Debug.Log("Catmul:waeitForStart()");
        is_waitForStart = true;
        is_stopped = true;
        is_active = true;
    }

    public void start_moving(){
        Debug.Log("Catmul:start_moving()");
        is_waitForStart = true;
        is_stopped = false;
        is_active = true;
    }
    public void stop_moving(){
        Debug.Log("Catmul:stop_moving()");
        is_stopped = true;
        is_active = false;
    }

    IEnumerator save_and_destroy(){
        Debug.Log("PlayBall:save_and_destroy()");
        // setzte alle veraenderungen der Handrepraesentation zurueck

        yield return new WaitForSeconds(0.5f);
        Debug.Log("PlayBall:save_and_destroy: Now Destroy ball Nr " + ID);
        Destroy(gameObject);
    }

    public float get_time_ball_active(){
        return time_ball_active;
    }


    // public void set_bounding_box(Vector3 min, Vector3 max){
    //     playarea_min = min;
    //     playarea_max = max;
    // }
    void addwaypoint(Vector3 min, Vector3 max){
            if (waypointList.Count<3){
                waypointList.Add(transform.position);
            }
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            float z = Random.Range(min.z, max.z);
            waypointList.Add(new Vector3(x, y, z));
    }
	void CatmulRom( List<Vector3> waypoints, float velocity)
	{
		catmullRomSplinePoints.Clear();

		Vector3 p0 = waypoints[0]; // Vector3 has an implicit conversion to Vector2
		Vector3 p1 = waypoints[1];
		Vector3 p2 = waypoints[2];
		Vector3 p3 = waypoints[3];

		float t0 = 0.0f;
		float t1 = GetT(t0, p0, p1);
		float t2 = GetT(t1, p1, p2);
		float t3 = GetT(t2, p2, p3);

        // velocity ... a velocity of 1 are 120 frames per travel unit
        float dist = Vector3.Distance(p1, p2);
        float numberOfPoints = dist * (1.0f/velocity) * 120.0f;
		for (float t=t1; t<t2; t+=((t2-t1)/numberOfPoints))
		{
		    Vector3 A1 = (t1-t)/(t1-t0)*p0 + (t-t0)/(t1-t0)*p1;
		    Vector3 A2 = (t2-t)/(t2-t1)*p1 + (t-t1)/(t2-t1)*p2;
		    Vector3 A3 = (t3-t)/(t3-t2)*p2 + (t-t2)/(t3-t2)*p3;
		    
		    Vector3 B1 = (t2-t)/(t2-t0)*A1 + (t-t0)/(t2-t0)*A2;
		    Vector3 B2 = (t3-t)/(t3-t1)*A2 + (t-t1)/(t3-t1)*A3;
		    
		    Vector3 C = (t2-t)/(t2-t1)*B1 + (t-t1)/(t2-t1)*B2;
		    
		    catmullRomSplinePoints.Add(C);
		}

	}

	float GetT(float t, Vector3 p0, Vector3 p1)
	{
        float a = Mathf.Pow((p1.x-p0.x), 2.0f) + Mathf.Pow((p1.y-p0.y), 2.0f) + Mathf.Pow((p1.z-p0.z), 2.0f);
//	    float a = Mathf.Pow((p1[0]-p0[0]), 2.0f) + Mathf.Pow((p1[1]-p0[1]), 2.0f) + Mathf.Pow((p1[2]-p0[2]), 2.0f); //Mathf.Pow((p1.x-p0.x), 2.0f) + Mathf.Pow((p1.y-p0.y), 2.0f);
	    float b = Mathf.Pow(a, alpha * 0.5f);
	   
	    return (b + t);
	}
	
    // public void initialize_new_ball(int newID){
    //     set_ID(newID);
    //     ball_size = gameSession.paradigma.ball_size_max;
    //     ball_vel = gameSession.paradigma.ball_veloc_max;
    //     ball_duration = gameSession.paradigma.ball_duration;
    //     transform.localScale = new Vector3(ball_size, ball_size, ball_size);
               
    // }
    public void set_ID(int newID){
        ID = newID;
    }
}

public class PunkteHistory{
    public List<float> fingerBallDist = new List<float>();
    public List<float> treffer = new List<float>();
    public List<float> timepoint = new List<float>();
    public float treffersum = 0.0f;
    private int list_length = 120;

    // ich muss hier bei der init noch den parameter uebergeben
    // init mit adapt_veloc_in_trial_based_on_last_framenum

    public void setHistoryLength(int l){
        Debug.Log("newlistlength = "+ l.ToString());
        list_length = l;
    }
    public void add_punkte(float dist, float ball_size, float t){
        fingerBallDist.Add(dist);
        timepoint.Add(t);
        if (dist<ball_size/2){treffer.Add(1.0f);}else{treffer.Add(0.0f);}
        treffersum += treffer[treffer.Count-1];
        if (fingerBallDist.Count>list_length){
            treffersum -= treffer[0];
            fingerBallDist.RemoveAt(0);
            timepoint.RemoveAt(0);
            treffer.RemoveAt(0);
        }       
        //if ((timepoint[timepoint.Count-1] - timepoint[0])>2.0f){

        
    }

    public float get_hit_rate(){

        return (treffersum/treffer.Count);
    }
}