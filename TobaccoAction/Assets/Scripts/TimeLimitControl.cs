using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object
    public GameObject gameDirector; 

    ////////////////////////////////////////////
    // private object, variable
    private GameDirector gd; 

    private Vector3 gScale;

    private Vector3 gPos;  

    private float timeElapsed = 0.0f; 

    private float timeInterval = 1.0f; 

    // Start is called before the first frame update
    void Start()
    {
        this.gd = gameDirector.GetComponent<GameDirector>(); 
    }

    // Update is called once per frame
    void Update()
    {
        gScale = gameObject.transform.localScale; 
        timeElapsed += Time.deltaTime; 
        if(timeElapsed >= timeInterval)
        {
            // 100秒
            gScale.x = gScale.x + 0.01f; 

            gameObject.transform.localScale = gScale; 
            
            timeElapsed = 0.0f; 
        }

        if(gScale.x >= 1.0f)
        {
            // game Clear
            timeInterval = 10000.0f; 
            gd.gameClear(); 
        }
    }
}
