using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class FadeSceneControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public variable
    public float timeInterval; 

    public int count; 

    ////////////////////////////////////////////
    // private object, variable
    private Image image; 

    private float timeElapsed = 0.0f; 

    private int hax = 0; 

    private bool flag = true; 

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>(); 
        flag = true;  
    }

    // Update is called once per frame
    void Update()
    {
        if(flag)
        {
            timeElapsed = 0.0f; 
        }

        if(GameDirector.fadeFalg)
        {
            flag = false; 
            hax += count; 
        
            if(hax>=255)
            {   
                GameDirector.fadeFalg = false; 
                hax = 0; 
                return; 
            }

            image.color = new Color32(0, 0, 0, (byte)hax); 
        }
        else
        {
            flag = true; 
            timeElapsed = 0.0f; 
        }
    }
}
