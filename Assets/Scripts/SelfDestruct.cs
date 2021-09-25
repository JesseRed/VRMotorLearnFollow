using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SelfDestruct : MonoBehaviour {
    // analog zu https://learn.unity.com/tutorial/create-a-simple-messaging-system-with-events?signup=true#5cf5960fedbc2a281acd21fa
    
    //public GameObject explosion;

    //private AudioSource audioSource;

    void Awake () 
    {
        //audioSource = GetComponent <AudioSource>();
    }

    void OnEnable () 
    {
        //Debug.Log("D++++++++++++++++ onEnable Register Destroy for the current ball");

        EventManager.StartListening ("Destroy", Destroy);
    }

    void OnDisable () 
    {
//        Debug.Log("D---------------- onDisable Delete Destroy Listener from the EventManager for the current ball");

        EventManager.StopListening ("Destroy", Destroy);
        
    }

    void Destroy () 
    {
        Debug.Log("We are now destroying the current Ball");
        //EventManager.StopListening ("Destroy", Destroy);
        //MyGameManager.numActiveBalls -=1;
        StartCoroutine (DestroyNow());
        //Destroy (gameObject);
        MyGameManager.numActiveBalls --;
    }

    IEnumerator DestroyNow() 
    {
        yield return new WaitForSeconds (2.0f);
        // yield return new WaitForSeconds (Random.Range (0.0f, 1.0f));
        // audioSource.pitch = Random.Range (0.75f, 1.75f);
        // audioSource.Play ();
        // float startTime = 0;
        // float shakeTime = Random.Range (1.0f, 3.0f);
        // while (startTime < shakeTime) 
        // {
        //     transform.Translate (Random.Range (-shake, shake), 0.0f, Random.Range (-shake, shake));
        //     transform.Rotate ( 0.0f, Random.Range (-shake * 100, shake * 100), 0.0f);
        //     startTime += Time.deltaTime;
        //     yield return null;
        // }
        // Instantiate (explosion, transform.position, transform.rotation);
        Destroy (gameObject);
    }
}