using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameSession : MonoBehaviour
{
    public PlayerData playerData;
    
    public bool isTutorial = false;
    public bool isInitialized = false;
    public string relativeSavePath = "Data";
    public string relativeReadPath = "Paradigms";
    // public string fileDesignName = "Experiment1_Day1.csv";
    public string fileDesignName = "Paradigma";
    private string fullSaveFileName;
    
    public Paradigma paradigma;
    
    public PlayBall _currentBall;

    private void Awake()
    {
        SetUpSingleton();
        //Debug.Log("Awake");
    }

    private void SetUpSingleton()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void register_new_Ball(int ID){
        float t = Time.time;
        playerData._Balls.Add(new OneBall(ID, t));
    }

    public void add_Ball_Hand_Position(int ID, Vector3 ball_pos, Vector3 hand_pos, float t, float time_ball_active)
    {
        BallPositionInfo _mycurrentBallPositionInfo = new BallPositionInfo(ball_pos.x, ball_pos.y, ball_pos.z, hand_pos.x, hand_pos.y, hand_pos.z, t, time_ball_active);
        playerData._Balls[ID].add_BallPostionInfo(_mycurrentBallPositionInfo);

    }
    public void SaveIntoJson(){
        // nun speichern wir den aktuellen Stand ab ... dies 
        // geschiet nach jedem Ball um einen Zwischenstand zu haben
        //string fullfilename = Path.Combine(Application.streamingAssetsPath, relativeSavePath, save_name);

        string data_string = JsonUtility.ToJson(playerData);
        
        System.IO.File.WriteAllText(fullSaveFileName, data_string);
    }

    public void InitializePlayerDataStructure()
    {
        // hole mir die Daten und initialisiere die Classe mit den PlayerData
        //GameObject tmp = FindObjectOfType<VornameText>;
        //GameObject tmp = GameObject.Find("VornameText");
        //TextMeshProUGUI tmp2 = tmp.GetComponent<TextMeshProUGUI>();
        fileDesignName = "Paradigm";
        string vorname = GameObject.Find("VornameInputField").GetComponent<TMP_InputField>().text;
        string nachname = GameObject.Find("NachnameInputField").GetComponent<TMP_InputField>().text;
        string gebDat = GameObject.Find("GebDatumInputField").GetComponent<TMP_InputField>().text;
        string trainingsDaystring = GameObject.Find("BehandlungsgruppeInputField").GetComponent<TMP_InputField>().text;
        int trainingsDay = int.Parse(trainingsDaystring);
        
        string vpNummerstring = GameObject.Find("RandomNummerInputField").GetComponent<TMP_InputField>().text;
        int vpNummer = int.Parse(vpNummerstring);



        string appdatapath = Application.streamingAssetsPath;
        print("appdatapath = " + appdatapath);
        
        string relative_path_file = Path.Combine(relativeReadPath, fileDesignName);
        print("relativeSavePath = " + relativeSavePath);
        //string jsonString = Path.Combine(appdatapath, relative_path_file);
        char x = Path.DirectorySeparatorChar;
        string jsonFileName = Application.streamingAssetsPath + x + relativeReadPath + x + fileDesignName + trainingsDay + ".json"; //Path.DirectorySeparatorChar Combine(appdatapath, relative_path_file);
                                                                                                //        string jsonString = "G:\\Unity\\Elisabeth_Scheiben_game\\Elisabeth_Scheiben\\Assets\\ExperimentalDesign\\Paradigm1.json";
                                                                                                        //string jsonString = "G:/Unity/Elisabeth_Scheiben_game/Elisabeth_Scheiben/Assets/ExperimentalDesign/Paradigm2.json";
        string jsonString = File.ReadAllText(jsonFileName);
        playerData = new PlayerData(vorname, nachname, gebDat, trainingsDay, vpNummer, jsonString);
        isInitialized = true;
        string saveFileName = "VP_" + vpNummer + "_TD_" + trainingsDay.ToString() + "_" + vorname + "_" + nachname + "_" + gebDat.Replace('.','-') + ".json";
        fullSaveFileName = Path.Combine(Application.streamingAssetsPath, relativeSavePath, saveFileName);
        //paradigma = Paradigma(jsonString);
        Debug.Log("fillSaveFileName="+ fullSaveFileName);
        paradigma = JsonUtility.FromJson<Paradigma>(jsonString);
        //playerData.PrintPlayerData();
        //Paradigma paradigma = Paradigma(jsonString);
    }
}
    [System.Serializable]
    public class PlayerData
    {
        public string vorname;
        public string nachname;
        public string gebDatum;
        public int trainingsDay;
        public int vpNummer;
        public Paradigma paradigma;
//        public int num_hits;
        public List<OneBall> _Balls;


        // construktor .... ohne die persoenlichen Infos geht nix
        public PlayerData(string vn, string nn, string gd, int td, int vpnum, string jsonString)
        {
            vorname = vn;
            nachname = nn;
            gebDatum = gd;
            trainingsDay = td;
            vpNummer = vpnum;
            //paradigma = Paradigma(jsonString);
            //print(jsonString);
            //paradigma = Paradigma.CreateFromJSON(jsonString);
            paradigma = JsonUtility.FromJson<Paradigma>(jsonString);
            _Balls = new List<OneBall>();
            //num_hits = 0;
        }

    public void add_Ball(OneBall new_Ball){
        _Balls.Add(new_Ball);
    }

    }

//  public void register_new_Ball(int ID, float radius, float mass, Vector3 grav, Vector3 force, Vector3 offset_pos, Vector3 offset_vel, Vector3 invert, Vector4 tremor){
//         float t = Time.time;
//         playerData._Balls.Add(new OneBall(ID, t, radius, mass, grav, force, offset_pos, offset_vel, invert, tremor);
      


[System.Serializable]
public class OneBall
{
    public float creation_Time;
    public int ID;
//    public Vector3 offset_hand_pos;
//    public Vector3 offset_hand_vel;
//    public Vector3 hand_invertation;
//    public Vector4 hand_tremor;
    public List<BallPositionInfo> _BallPositionInfoList;
    public OneBall(int _ID, float t)
    {
        _BallPositionInfoList = new List<BallPositionInfo>();
        creation_Time = t;
        ID = _ID;
    }
    public void add_BallPostionInfo(BallPositionInfo new_BallPositionInfo)
    {
        _BallPositionInfoList.Add(new_BallPositionInfo);
    }
}



[System.Serializable]
public class BallPositionInfo
{
    public float ball_x;
    public float ball_y;
    public float ball_z;
    public float hand_x;
    public float hand_y;
    public float hand_z;
    public float t;
    public float time_ball_active;

    public BallPositionInfo(float new_x, float new_y, float new_z, float new_hand_x, float new_hand_y, float new_hand_z, float new_t, float new_time_ball_active)
    {
        //time seit application start
        t = new_t;
        ball_x = new_x;
        ball_y = new_y;
        ball_z = new_z;
        hand_x = new_hand_x;
        hand_y = new_hand_y;
        hand_z = new_hand_z;
        // wie lange der Ball bisher bereits aktiv war
        time_ball_active = new_time_ball_active;
        // hier muessen dann noch fuer den jeweiligen Zeitpunkt di
        // Infos rein ueber inverted und force und offset das wir sie ja dynamisch veraendern wollen
    }
}






    [System.Serializable]
    public class Paradigma
    {

    public int numBalls;
    public float target_size;
    public float ball_duration;
    public float no_ball_duration;
    
    public float ball_size_max;
    public float ball_size_min;
    
    public float ball_veloc_min;
    public float ball_veloc_std;
    public float ball_veloc_max;
    public float ball_veloc_in_trial_min;
    public float ball_veloc_in_trial_max;
    public float ball_mass;
    public float playarea_min_x;
    public float playarea_min_y;
    public float playarea_min_z;
    public float playarea_max_x;
    public float playarea_max_y;
    public float playarea_max_z;
    
    
    public float gravity_X_min;
    public float gravity_X_max;
    public float gravity_Y_min;
    public float gravity_Y_max;
    public float gravity_Z_min;
    public float gravity_Z_max;
    public float force_X_min;
    public float force_X_max;
    public float force_Y_min;
    public float force_Y_max;
    public float force_Z_min;
    public float force_Z_max;
    public float offset_hand_pos_X_min;
    public float offset_hand_pos_X_max;
    public float offset_hand_pos_Y_min;
    public float offset_hand_pos_Y_max;
    public float offset_hand_pos_Z_min;
    public float offset_hand_pos_Z_max;
    public float offset_hand_vel_X_min;
    public float offset_hand_vel_X_max;
    public float offset_hand_vel_Y_min;
    public float offset_hand_vel_Y_max;
    public float offset_hand_vel_Z_min;
    public float offset_hand_vel_Z_max;
    public float hand_invert_X;
    public float hand_invert_Y;
    public float hand_invert_Z;
    public float hand_tremor_X;
    public float hand_tremor_Y;
    public float hand_tremor_Z;
    public float hand_tremor_freq;
    public int Adaptive;
    public float ball_veloc_diff;
    public float desired_hit_rate;
    public float offset_hand_pos_diff;
    public float offset_hand_vel_diff;
    public float hand_invert_diff;
    public float hand_tremor_diff;
    public bool adapt_ball_size;
    public bool adapt_veloc_in_trial;
    public int adapt_veloc_in_trial_based_on_last_framenum;
    public bool adapt_veloc;
    public bool adapt_offset_hand_pos_X;
    public bool adapt_offset_hand_pos_Y;
    public bool adapt_offset_hand_pos_Z;
    public bool adapt_offset_hand_vel_X;
    public bool adapt_offset_hand_vel_Y;
    public bool adapt_offset_hand_vel_Z;
    public bool adapt_invert_X;
    public bool adapt_invert_Y;
    public bool adapt_invert_Z;
    public bool adapt_tremor_X;
    public bool adapt_tremor_Y;
    public bool adapt_tremor_Z;
    public int num_invert_axis_min;
    public float spiegelpunkt_der_invertierung_x;
    public float spiegelpunkt_der_invertierung_y;
    public float spiegelpunkt_der_invertierung_z;
    


    }

