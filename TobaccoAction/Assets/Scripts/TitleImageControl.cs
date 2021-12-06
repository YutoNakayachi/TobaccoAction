using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleImageControl : MonoBehaviour
{
    private bool flag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flag)
        {
            Vector3 kero = gameObject.transform.localScale; 
            kero.x += 0.08f; 
            kero.y += 0.08f; 
            gameObject.transform.localScale = kero; 
        }
    }

    public void startGame()
    {
        flag = true; 
    }
}
