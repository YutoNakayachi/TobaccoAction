using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class PauseSceneControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject ItemFrame; 

    public GameObject QuitFrame; 

    public GameObject HpGage; 

    public GameObject MpGage; 

    public Text moneyText; 

    public Text descriptionText; 

    public Text hpText; 

    public Text mpText; 

    ////////////////////////////////////////////
    // private object, variable
    private Slider hpSlider; 

    private Slider mpSlider; 

    private int axis = 0; 

    private int state = 0;

    private int state_count = 2; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip moveSound; 

    public AudioClip enterSound; 

    private AudioSource audioSource; 

    enum State 
    {
        Item, 
        Quit 
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemFrame.SetActive(true); 
        hpSlider = HpGage.GetComponent<Slider>(); 
        mpSlider = MpGage.GetComponent<Slider>(); 
        moneyText.text = "" + GameDirector.money; 
        hpSlider.value = (float)GameDirector.hp / 100.0f; 
        hpText.text = "" + GameDirector.hp; 
        mpSlider.value = (float)GameDirector.mp / 100.0f; 
        mpText.text = "" + GameDirector.mp; 
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            axis = -1; 
            audioSource.PlayOneShot(moveSound); 
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            axis = 1; 
            audioSource.PlayOneShot(moveSound); 
        }
        else 
        {
            axis = 0; 
        }

        state += axis; 

        if(state < 0) 
        {
            state = 0; 
        }
        else if(state >= state_count)
        {
            state = state_count - 1; 
        }

        if(state == (int)State.Item)
        {
            ItemFrame.SetActive(true); 
            QuitFrame.SetActive(false); 

            descriptionText.text = "アイテムの使用 / 確認"; 
            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
                SceneManager.LoadScene("ItemScene", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("PauseScene");  
            }
        }
        else if(state == (int)State.Quit)
        {
            ItemFrame.SetActive(false); 
            QuitFrame.SetActive(true); 

            descriptionText.text = "ゲームを終了する"; 

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
                #if UNITY_EDITOR 
                    UnityEditor.EditorApplication.isPlaying = false; 
                #else 
                    Application.Quit(); 
                #endif
            }
        }
    }
}
