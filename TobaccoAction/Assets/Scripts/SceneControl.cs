using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject ActionObject; 

    public GameObject FadePanel; 

    ////////////////////////////////////////////
    // private object, variable
    private bool isPause = false; 

    public AudioClip pauseSound; 

    private AudioSource audioSource; 

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(ShopControl.isEntry)
        {
            ActionObject.SetActive(false); 
            FadePanel.SetActive(false); 
            if(ShopControl.isExit)
            {
                ShopControl.isEntry = false; 
                ShopControl.isExit  = false; 
                FadePanel.SetActive(false); 
                ActionObject.SetActive(true); 
                Time.timeScale = 1.0f; 
            }
        }

        ////////////////////////////////////////////
        // Pause処理
        if(Input.GetKeyDown(KeyCode.M) & !isPause)
        {
            audioSource.PlayOneShot(pauseSound); 
            
            isPause = true; 
            ActionObject.SetActive(false); 
            SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive); 
            Time.timeScale = 0.0f; 
        }
        else if(Input.GetKeyDown(KeyCode.M) & isPause)
        {
            audioSource.PlayOneShot(pauseSound); 
            isPause = false; 
            ActionObject.SetActive(true); 
            Time.timeScale = 1.0f; 
            SceneManager.UnloadSceneAsync("PauseScene"); 
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f); 
        Time.timeScale = 0.0f; 
        isPause = true; 
        ActionObject.SetActive(false); 
        FadePanel.SetActive(false); 
        GameDirector.fadeFalg = false; 
        SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive); 
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1.0f); 

        isPause = true; 
        ActionObject.SetActive(true); 
        FadePanel.SetActive(false); 
        GameDirector.fadeFalg = false;
        SceneManager.UnloadSceneAsync("PauseScene"); 
    }

}
