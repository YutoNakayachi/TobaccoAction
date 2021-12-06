using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class ShopControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public static variable
    public static bool isExit = false; 

    public static bool isEntry = false;

    ////////////////////////////////////////////
    // public object
    public GameObject BuyGrid; 

    public GameObject SellGrid; 

    public GameObject ExitGrid; 
    
    public GameObject MoneyText; 

    public GameObject FadePanel; 

    ////////////////////////////////////////////
    // private object, variable
    private SpriteRenderer buySprite; 

    private SpriteRenderer sellSprite; 

    private SpriteRenderer exitSprite; 

    private Text moneyText; 

    private int state = 0; 

    private int state_count = 3; 

    private int axis = 0; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip moveSound; 

    public AudioClip enterSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        isEntry = true; 
        buySprite = BuyGrid.GetComponent<SpriteRenderer>(); 
        sellSprite = SellGrid.GetComponent<SpriteRenderer>(); 
        exitSprite = ExitGrid.GetComponent<SpriteRenderer>(); 
        moneyText = MoneyText.GetComponent<Text>(); 
        this.audioSource = GetComponent<AudioSource>(); 

        buySprite.color = new Color32(0, 0, 0, 255); 
        moneyText.text = "" + GameDirector.money; 
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////
        // 入力処理
        if(Input.GetKeyDown(KeyCode.UpArrow) | Input.GetKeyDown(KeyCode.RightArrow))
        {
            axis = -1; 
            audioSource.PlayOneShot(moveSound); 
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow) | Input.GetKeyDown(KeyCode.LeftArrow))
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

        if(state==0)
        {
            buySprite.color = new Color32(0, 0, 0, 255); 
            buySprite.color = Color.red; 
            sellSprite.color = new Color32(180, 180, 180, 255); 
            exitSprite.color = new Color32(180, 180, 180, 255); 
        }
        else if(state == 1) 
        {
            buySprite.color = new Color32(180, 180, 180, 255); 
            sellSprite.color = new Color32(0, 0, 0, 255); 
            sellSprite.color = Color.red; 
            exitSprite.color = new Color32(180, 180, 180, 255);
        }
        else if(state == 2) 
        {
            buySprite.color = new Color32(180, 180, 180, 255); 
            sellSprite.color = new Color32(180, 180, 180, 255); 
            exitSprite.color = new Color32(0, 0, 0, 255); 
            exitSprite.color = Color.red; 
        }

        if(state==0)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
                //StartCoroutine("sceneLoad"); 
                SceneManager.LoadScene("Buy19xxScene", LoadSceneMode.Additive); 
                SceneManager.UnloadSceneAsync("Shop19xxScene");
            }
        }

        if(state==2)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
                isExit = true; 
                SceneManager.UnloadSceneAsync("Shop19xxScene"); 
            }
        }
    }

}
