using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public static int money = 0; 

    public static int hp = 300; 

    public static int mp = 50; 

    public static int setId  = -1; // セットしてあるアイテムid 

    public static int age = 0; 

    public static bool fadeFalg = false; 

    // itemSceneControl.csとの連携に気を付けて
    // itemBox[id] = 個数; 
    public static int[] itemBox = new int[5]; 

    public static int[] balanceCount = new int[5]; 

    public GameObject player; 

    public GameObject hpGage; 

    public GameObject mpGage; 

    public GameObject gameOverPanel; 

    public GameObject GameClearPanel; 

    public GameObject hpNumber; 

    public GameObject mpNumber; 

    public GameObject menuPanel; 

    // ItemSetGrid
    public GameObject ItemImage; 

    public GameObject BalanceGrid1; 

    public GameObject BalanceGrid2;

    public GameObject BalanceGrid3;

    public GameObject ShopEnterFrame; 

    public GameObject FadePanel; 

    public Sprite marlboro_sprite; 

    public Sprite mildseven_sprite; 

    public Sprite sevenstars_sprite; 

    public Sprite lark_sprite; 

    public Sprite hope_sprite; 

    public Text moneyText; 

    public PlayerControl playerControl; 

    public int ageVal; 

    ////////////////////////////////////////////
    // private object, variable
    private Slider hpSlider; 

    private Slider mpSlider; 

    private Text hptext;  

    private Text mptext; 

    private SpriteRenderer setSpRenderer; // ItemImage

    private PlayerControl pc; 

    private Sprite[] spItemBox = new Sprite[5]; 

    private float timeInterval = 1.0f; 

    private float timeElapsed = 0.0f; 

    private bool isPause = false; 

    private bool isShop = false; 

    private bool policeGet = false; 

    private bool isClear = false; 

    private bool isFade = false; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip enterSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player"); 
        this.pc = player.GetComponent<PlayerControl>(); 

        hpSlider = hpGage.GetComponent<Slider>(); 
        mpSlider = mpGage.GetComponent<Slider>(); 
        hptext = hpNumber.GetComponent<Text>(); 
        mptext = mpNumber.GetComponent<Text>(); 
        setSpRenderer = ItemImage.GetComponent<SpriteRenderer>(); 
     
        this.audioSource = GetComponent<AudioSource>(); 
    
        for(int i=0;i<5;++i) 
        {
            itemBox[i] = 0; 
            balanceCount[i] = 0; 
        }

        spItemBox[0] = marlboro_sprite; 
        spItemBox[1] = mildseven_sprite; 
        spItemBox[2] = sevenstars_sprite; 
        spItemBox[3] = lark_sprite; 
        spItemBox[4] = hope_sprite; 

        age = ageVal; 
        Time.timeScale = 1.0f; 

        hp = 200; 
        mp = 50; 
        money = 0; 
        setId = -1; 
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; 

        if(isClear)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(ageVal==1995)
                {
                    // 次のステージへ.
                    Time.timeScale = 1.0f; 
                    SceneManager.LoadScene("MainGameScene_No2"); 
                }
                else if(ageVal==2015)
                {
                    Time.timeScale = 1.0f; 
                    SceneManager.LoadScene("MainGameScene_No3"); 
                }
                else if(ageVal==2035)
                {
                    #if UNITY_EDITOR 
                        UnityEditor.EditorApplication.isPlaying = false; 
                    #else 
                        Application.Quit(); 
                    #endif
                }
            }
        }

        if(mpZeroCheck())
        {
            if(timeElapsed>=timeInterval)
            {
                hpDecrease(1); 
                timeElapsed = 0.0f; 
            }

        }
        else 
        {
            if(timeElapsed >= timeInterval)
            {
                mpDecrease(1); 
                timeElapsed = 0.0f; 
            }
        }

        // ゲームオーバー処理
        if(hpZeroCheck())
        {
            gameOver(); 
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

        ShopEnterFrame.SetActive(false); 
        if(isShop)
        {
            ShopEnterFrame.SetActive(true); 
            if(Input.GetKeyDown(KeyCode.E))
            {
                audioSource.PlayOneShot(enterSound); 
                fadeFalg = true; 
                FadePanel.SetActive(true);
                StartCoroutine("ShopRoutine"); 
                isShop = false; 
            }
        }

    }

    public void moneyUpdate(int val)
    {
        money += val; 
        if(money<=0)
        {
            money = 0; 
        }
    }

    public void shopCheck(bool check)
    {
        if(check) isShop = true; 
        else isShop = false; 
    }

    public void hpIncrease(int val)
    {
        hp += val; 
        uiUpdate(); 
    }

    public void hpDecrease(int val)
    {
        hp -= val; 
        uiUpdate(); 
    }

    public void mpIncrease(int val) 
    {
        mp += val; 
        if(mp>=100)
        {
            mp = 100; 
        }
        uiUpdate(); 
    }

    public void mpDecrease(int val) 
    {
        mp -= val; 
        if(mp<=0)
        {
            mp = 0; 
        }
        uiUpdate(); 
    }

    public bool mpZeroCheck()
    {
        if(mp<=0)
        {
            return true; 
        }
        else 
        {
            return false; 
        }
    }

    public bool hpZeroCheck()
    {
        uiUpdate(); 
        if(hp<=0) return true; 
        else return false; 
    }

    public void policeGetUpdate(bool val)
    {
        if(val)
        {
            policeGet = true; 
        }
        else 
        {
            policeGet = false; 
        }
    }

    public bool policeGetter()
    {
        if(policeGet)
        {
            return true; 
        }
        else 
        {
            return false; 
        }
    }

    public void gameClear() 
    {
        isClear = true; 
        Time.timeScale = 0.0f; 
        GameClearPanel.SetActive(true); 
    }

    private void gameOver()
    {
        pc.deathCheck(); 
        StartCoroutine("GameOverRoutine"); 
    }

    private void uiUpdate()
    {
        if(hp>=0)
        {
            hpSlider.value = (float)hp / 200.0f; 
            hptext.text = ""+hp;
        }
        if(mp>=0)
        {
            mpSlider.value = (float)mp / 100.0f; 
            mptext.text = ""+mp; 
        }

        moneyText.text = ""+money; 

        ItemImage.SetActive(false); 
        BalanceGrid1.SetActive(false); 
        BalanceGrid2.SetActive(false); 
        BalanceGrid3.SetActive(false); 
        if(GameDirector.setId!=-1)
        {
            if(GameDirector.itemBox[GameDirector.setId]!=0)
            {
                setSpRenderer.sprite = spItemBox[GameDirector.setId]; 
                ItemImage.SetActive(true); 
            }
            balanceUpdate(); 
        }
    }

    private void balanceUpdate()
    {
        if(GameDirector.balanceCount[GameDirector.setId]>=1)
        {
            BalanceGrid1.SetActive(true); 
        }

        if(GameDirector.balanceCount[GameDirector.setId]>=2)
        {
            BalanceGrid2.SetActive(true); 
        }

        if(GameDirector.balanceCount[GameDirector.setId]>=3)
        {
            BalanceGrid3.SetActive(true); 
        }

    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(3.0f); 
        Time.timeScale = 0.0f; 
        gameOverPanel.SetActive(true); 
    }

    private IEnumerator ShopRoutine()
    {
        yield return new WaitForSeconds(2.0f); 
        Time.timeScale = 0.0f; 
        ShopControl.isEntry = true;
        SceneManager.LoadScene("Shop19xxScene", LoadSceneMode.Additive); 
    }

}
