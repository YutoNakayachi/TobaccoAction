using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class TitleSceneManager : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object
    public GameObject pressStart; 

    public GameObject titleImage; 

    ////////////////////////////////////////////
    // private object, variable
    private TitleImageControl imageControl;

    private Text pressText; 

    private float timeElapsed = 0.0f; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip enterSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        pressText = pressStart.GetComponent<Text>();    
        imageControl = titleImage.GetComponent<TitleImageControl>(); 
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enterSound);
            imageControl.startGame(); 
            StartCoroutine("StartMainScene"); 
        }

        timeElapsed += Time.deltaTime; 
        int hax = (int)(timeElapsed * 200.0f);
        hax = hax % 200; 
        pressText.color = new Color32(255, 77, 77, (byte)hax); 
    }

    private IEnumerator StartMainScene()
    {
        yield return new WaitForSeconds(1.0f); 
        SceneManager.LoadScene("MainGameScene"); 
    }
}
