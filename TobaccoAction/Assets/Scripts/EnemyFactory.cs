using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject badBoyPrefab; 

    public GameObject badBoyGoldPrefab; 

    public GameObject enemy1; 

    public GameObject enemy2; 

    public GameObject enemy3; 

    public GameObject parentObj; 

    public float timeInterval; 

    ////////////////////////////////////////////
    // private object, variable
    private float xmax = 21.0f; 

    private float xmin = -21.0f; 

    private float timeElapsed1 = 0.0f; 

    private float timeElapsed2 = 0.0f; 

    private float timeElapsed3 = 0.0f; 

    private int enemy1Count = 0;

    private int enemy2Count = 0; 

    private int enemy3Count = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var parent = parentObj.transform; 

        ////////////////////////////////////////////
        // エネミーの生成処理
        if(enemy1Count==0)
        {
            timeElapsed1 += Time.deltaTime; 
            if(timeElapsed1>=timeInterval)
            {
                Instantiate(enemy1, new Vector3(-16.0f, -3.67f, 0.0f), Quaternion.identity, parent); 
                enemy1Count += 1; 
                timeElapsed1 = 0.0f; 
            }
        }

        if(enemy2Count==0)
        {
            timeElapsed2 += Time.deltaTime; 
            if(timeElapsed2>=timeInterval*2)
            {
                Instantiate(enemy2, new Vector3(16.0f, -3.67f, 0.0f), Quaternion.identity, parent); 
                enemy2Count += 1; 
                timeElapsed2 = 0.0f; 
            }
        }

        if(enemy3Count==0)
        {
            timeElapsed3 += Time.deltaTime; 
            if(timeElapsed3>=timeInterval)
            {
                Instantiate(enemy3, new Vector3(35.0f, -3.67f, 0.0f), Quaternion.identity, parent); 
                enemy3Count += 1; 
                timeElapsed3 = 0.0f; 
            }
        }

    }

    public void enemyCountUpdate(int num)
    {
        if(num==1) enemy1Count -= 1; 
        else if(num==2) enemy2Count -= 1; 
        else if(num==3) enemy3Count -= 1; 

        if(enemy1Count<0)
        {
            enemy1Count = 0;
        }
        
        if(enemy2Count<0)
        {
            enemy2Count = 0; 
        }

        if(enemy3Count<0)
        {
            enemy3Count = 0; 
        }
    }
}
