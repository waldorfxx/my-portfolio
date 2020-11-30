using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour
{

    public float timer = 0.0f;
    public static int seconds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        seconds = (int)timer % 60; 
        // Debug.Log("showtime: "+ timer +"Sec: "+ seconds);
        
    }
}
