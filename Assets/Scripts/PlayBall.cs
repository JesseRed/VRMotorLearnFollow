
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
	Vector3 playarea_min;
	Vector3 playarea_max;
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
    public bool is_active = false;
    public MyGameManager myGameManager;
    public GameSession gameSession;
    private int ID;
    private float ball_size;
    private float ball_vel;
    private float time_start_moving;
    public Parameter parameter;
    private Vector3 playarea_center; 
    private float ball_duration;
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
        set_current_playarea_and_object_transform();
        
        
                
                
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
        ball_vel = gameSession.paradigma.ball_veloc_max;
        ball_duration = gameSession.paradigma.ball_duration;
        Debug.Log("PlayBall:Awake:ball_duration = "+ ball_duration);
        transform.localScale = new Vector3(ball_size, ball_size, ball_size);
        waitForStart();
    }
    void Start(){
        Debug.Log("STart PlayBall");
        
    }

    void set_current_playarea_and_object_transform(){
        // float xh = rightHand.transform.position.x;
        // float yh = rightHand.transform.position.y;
        // float zh = rightHand.transform.position.z;
        // float xe = rightEye.transform.position.x;
        // float ye = rightEye.transform.position.y;
        // float ze = rightEye.transform.position.z; 

        // playarea_min[0] = rightEye.transform.position.x-0.2f;
        // playarea_min[1] = rightEye.transform.position.y-0.3f;
        // playarea_min[2] = rightEye.transform.position.z+0.15f;
        // playarea_max[0] = rightEye.transform.position.x+0.2f;
        // playarea_max[1] = rightEye.transform.position.y-0.1f;
        // playarea_max[2] = rightEye.transform.position.z+0.4f;

        playarea_min[0] = rightEye.transform.position.x+gameSession.paradigma.playarea_min_x;
        playarea_min[1] = rightEye.transform.position.y+gameSession.paradigma.playarea_min_y;
        playarea_min[2] = rightEye.transform.position.z+gameSession.paradigma.playarea_min_z;
        playarea_max[0] = rightEye.transform.position.x+gameSession.paradigma.playarea_max_x;
        playarea_max[1] = rightEye.transform.position.y+gameSession.paradigma.playarea_max_y;
        playarea_max[2] = rightEye.transform.position.z+gameSession.paradigma.playarea_max_z;
        playarea_center = (playarea_min+playarea_max)/2.0f;
        transform.position = playarea_center;
    }

	void Update()
	{

        currentFingerBallDist = Vector3.Distance(rightHand.transform.position, transform.position);
        myline.SetPosition(0, rightHand.transform.position);
        myline.SetPosition(1, transform.position);
        // waiting that the finger comes near
        if (is_waitForStart) {
            //Debug.Log("Catmul:Update is_waitForStart() with currentFingerBallDist = " + currentFingerBallDist);
            if (currentFingerBallDist<0.01f){
                // ####################
                // Der start des Balls
                // setze die Startzeit des Balls
                time_start_moving = Time.time;
                Debug.Log("time_start_moving = " + time_start_moving);
                is_waitForStart = false;
                is_stopped = false;
                is_active = true;
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
                CatmulRom(waypointList, 0.1f);
            }
            // hier sollten noch andere Parameter
            time_ball_active = Time.time-time_start_moving;
            //Debug.Log("estimate time_ball_active = " + time_ball_active + " ball_duration = "+ ball_duration);
            gameSession.add_Ball_Hand_Position(ID, transform.position, rightHand.transform.position, Time.time, time_ball_active);
            parameter.push_infos(currentFingerBallDist);
            if ( time_ball_active>ball_duration){
                Debug.Log("time_ball_active>ball_duration");
                 StartCoroutine(save_and_destroy());
            }
        }
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
        is_stopped = true;
        is_active = false;
        gameSession.SaveIntoJson();
        yield return new WaitForSeconds(1.5f);
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
            if (waypointList.Count==0){
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
