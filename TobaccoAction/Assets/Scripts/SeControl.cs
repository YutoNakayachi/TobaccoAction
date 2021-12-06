using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // private variable 
    private float timeElapsed = 0.0f; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip backGroundSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); 
        audioSource.PlayOneShot(backGroundSound); 
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; 
        if(timeElapsed>=70.0f)
        {
            audioSource.PlayOneShot(backGroundSound);
            timeElapsed = 0.0f; 
        }
    }
}
