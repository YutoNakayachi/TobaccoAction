using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenControl : MonoBehaviour
{
    public GameObject player; 

    private Transform pTrans; 
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); 
        pTrans = player.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float x = pTrans.position.x;  
        if(x<=-26.0f)
        {
            x = -26.0f; 
        }
        else if(x >= 29.0f)
        {
            x = 29.0f; 
        }
        x = x - 0.5f;
        transform.position = new Vector3(x, transform.position.y, transform.position.z); 
    }
}
