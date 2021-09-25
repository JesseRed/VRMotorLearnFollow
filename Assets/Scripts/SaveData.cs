using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveData : MonoBehaviour

{

    //public MyGameManager myGameManager;
    //public List<BallInfo> _BallInfos = new List<BallInfo>();
    // public MyBallCollection myBallCollection = new MyBallCollection();
    //public Data myData ;
    //public Ball _currentBall;
    public GameSession gameSession;


    
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        //myGameManager = FindObjectOfType<MyGameManager>();
        // at this point the player Data should be entered
        //myData = new Data("asdf", "87", "datum about here");
    }
}
//     public void add_empty_Ball(){
//         float t = Time.time;
//         myData._Balls.Add(new OneBall(t));
//     }

//     public void set_Hit(int ID, int x){
//         // if the current ball has hit the target 

//         myData._Balls[ID].is_Hit = x;
//     }

//     public void register_new_Ball(int ID){
//         while (ID>myData._Balls.Count-1){
//             add_empty_Ball();
//         }
//     }
//     public void set_pick_up_time(int ID, float t){
//         myData._Balls[ID].pick_up_Time = t;
//     }
//     //[SerializeField] private SubjectData _SubjectData = new SubjectData();
//     public void set_leave_the_Hand_Time(int ID, float t){
//         myData._Balls[ID].leave_the_Hand_Time = t;
//     }

//     public void add_Ball_Hand_Position(int ID, float ball_X, float ball_Y, float ball_Z, 
//         float hand_X, float hand_Y, float hand_Z, float t)
//     {
//         BallPositionInfo _mycurrentBallPositionInfo = new BallPositionInfo(ball_X, ball_Y, ball_Z, hand_X, hand_Y, hand_Z, t);
//         myData._Balls[ID].add_BallPostionInfo(_mycurrentBallPositionInfo);
//     }

    
//     public void SaveIntoJson(){
//         // nun speichern wir den aktuellen Stand ab ... dies 
//         // geschiet nach jedem Ball um einen Zwischenstand zu haben
//         string fullfilename = Path.Combine(myGameManager.dataFolderName, myGameManager.datafilename);
//         string data_string = JsonUtility.ToJson(myData);
        
//         System.IO.File.WriteAllText(fullfilename, data_string);
//     }
    
// }


// [System.Serializable]
// public class Data
// {
//     public List<OneBall> _Balls;

//     public string id;
//     public string age;
//     public string date;


//     public Data(string new_id, string new_age, string new_date){
//         id = new_id;
//         age = new_age;
//         date = new_date;
//         _Balls = new List<OneBall>();
//     }
//     public void add_Ball(OneBall new_Ball){
//         _Balls.Add(new_Ball);
//     }
// }


// [System.Serializable]
// public class OneBall
// {
//     public int is_Hit;
//     public float creation_Time;
//     public float pick_up_Time;
//     public float leave_the_Hand_Time;
//     public List<BallPositionInfo> _BallPositionInfoList;
//     public OneBall(float t)
//     {
//         _BallPositionInfoList = new List<BallPositionInfo>();
//         is_Hit = 0;
//         creation_Time = t;
//     }
//     public void add_BallPostionInfo(BallPositionInfo new_BallPositionInfo)
//     {
//         _BallPositionInfoList.Add(new_BallPositionInfo);
//     }
// }



// [System.Serializable]
// public class BallPositionInfo
// {
//     public float ball_x;
//     public float ball_y;
//     public float ball_z;
//     public float hand_x;
//     public float hand_y;
//     public float hand_z;
//     public float t;

//     public BallPositionInfo(float new_x, float new_y, float new_z, float new_hand_x, float new_hand_y, float new_hand_z, float new_t)
//     {
//         //time
//         t = new_t;
//         ball_x = new_x;
//         ball_y = new_y;
//         ball_z = new_z;
//         hand_x = new_hand_x;
//         hand_y = new_hand_y;
//         hand_z = new_hand_z;
//     }
// }



// [System.Serializable]
// public class MyBallCollection
// {
//     public List<BallInfo> balls;
    
//     public MyBallCollection(){
//         balls = new List<BallInfo>(); 
//     }
//     public void add_spawned_Ball(BallInfo _BallInfo){
//         balls.Add(_BallInfo);
//     }
// }