using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public float leftTime = 5.0f; 
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, leftTime); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
