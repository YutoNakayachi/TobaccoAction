using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceGroundControl : MonoBehaviour
{
    public GameObject parentObj; 

    private PoliceControl policeControl; 

    // Start is called before the first frame update
    void Start()
    {
        policeControl = parentObj.GetComponent<PoliceControl>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D( Collider2D col)
    {
        if(col.gameObject.tag == "ground")
        {
            policeControl.groundUpdate(true); 
        }
    }

    void OnTriggerExit2D( Collider2D col )
    {
        if(col.gameObject.tag == "ground")
        {
            policeControl.groundUpdate(false); 
        }
    }
}
