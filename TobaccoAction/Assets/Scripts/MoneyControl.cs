using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y - 0.3f, 
            transform.position.z 
        ); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "player")
        {
            
        }
    }
}
