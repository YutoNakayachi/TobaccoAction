using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class damageTextControl : MonoBehaviour
{

    public float moveSpeed = 0.3f; 

    private Text damageText; 

    private float timeInterval = 1.5f; 

    private float timeElapsed = 0.0f; 

    // Start is called before the first frame update
    void Start()
    {
        damageText = GetComponentInChildren<Text>(); 
        damageText.text = "30"; 
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

    public void uiUpdate(int val)
    {
        damageText.text = "" + val; 
    }
}
