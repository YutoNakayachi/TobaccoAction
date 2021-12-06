using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class playerDamageTextControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public variable
    public float moveSpeed = 0.3f; 

    ////////////////////////////////////////////
    // private object, variable
    private Text damageText; 

    private float timeInterval = 1.5f; 

    private float timeElapsed = 0.0f; 

    // Start is called before the first frame update
    void Start()
    {
        damageText = GetComponentInChildren<Text>(); 
        damageText.text = "10"; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime; 

        timeElapsed += Time.deltaTime; 
        if(timeElapsed >= timeInterval)
        {
            Destroy(gameObject); 
        }
    }
}
