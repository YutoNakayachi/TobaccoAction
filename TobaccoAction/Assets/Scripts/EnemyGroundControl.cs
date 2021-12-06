using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundControl : MonoBehaviour
{
    public GameObject parentObj; 

    private Enemy1Control enemyControl; 
    
    // Start is called before the first frame update
    void Start()
    {
        enemyControl = parentObj.GetComponent<Enemy1Control>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D( Collider2D col )
    {
        if(col.gameObject.tag == "ground")
        {
            enemyControl.groundUpdate(true); 
        }
    }

    void OnTriggerExit2D( Collider2D col )
    {
        if(col.gameObject.tag == "ground") 
        {
            enemyControl.groundUpdate(false); 
        }
    }
}
