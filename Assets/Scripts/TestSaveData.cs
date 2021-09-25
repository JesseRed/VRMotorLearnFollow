using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestSaveData : MonoBehaviour

{

    public MyGameManager myGameManager;
    
    public TestMyBallCollection testMyBallCollection = new TestMyBallCollection();
    public int counter = 0;
    void Start()
    {
        myGameManager = FindObjectOfType<MyGameManager>();
        testMyBallCollection.balls.Add(new TestXCurrentBall());
    }

    public void add_BallPosition(float x, float y, float z, float t)
    {
        Debug.Log("add not to Ballinfos in saveData x = " + x);
        //_BallInfos.Add(new BallInfo(x,y,z,t) );
        
    }

    public void SaveIntoJson(){
        string fullfilename = Path.Combine(myGameManager.dataFolderName, myGameManager.datafilename);
//        string data_string = JsonUtility.ToJson(_SubjectData);
        string data_string = JsonUtility.ToJson(testMyBallCollection);
        Debug.Log("datastring = " + data_string);
        System.IO.File.WriteAllText(fullfilename, data_string);
    }

    void Update()
    {
        counter ++;
        Debug.Log(counter);
        testMyBallCollection.balls[testMyBallCollection.balls.Count-1].addentry(counter);
        if (counter>100)
        {
            Debug.Log("saving");
            SaveIntoJson();
        }
    }
    
}


// [System.Serializable]
// public class BallPosition{
//     public float x;
//     public float y;
//     public float z;
    
// }



[System.Serializable]
public class TestBallInfo
{
    public float x;
    public float y;
    public float z;
    public float t;

    public TestBallInfo(float new_x, float new_y, float new_z, float new_t)
    {
        //time
        t = new_t;
        x = new_x;
        y = new_y;
        z = new_z;
    }
}

[System.Serializable]
public class TestXCurrentBall
{
    public string somestring;
    public List<int> liste;
    public List<TestBallInfo> _TestBallInfo;
    public TestXCurrentBall(){
         somestring = "yyy";
         liste = new List<int>();
         _TestBallInfo = new List<TestBallInfo>();
    }

    public void addentry(int x){
        float t = Time.time;
        liste.Add(x);
        _TestBallInfo.Add(new TestBallInfo(t, t, t, t));
    }

}

[System.Serializable]
public class TestMyBallCollection
{
    public List<TestXCurrentBall> balls;// = new List<CurrentBall>(); 
    // public TestMyBallCollection()
    // {
    //     balls = new List<TestXCurrentBall>();
    // }
}