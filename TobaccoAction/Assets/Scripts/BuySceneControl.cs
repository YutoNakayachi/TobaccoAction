using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class BuySceneControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject CameraControl; 

    public GameObject BackFrame; 

    public GameObject Item1Frame; 

    public GameObject Item2Frame; 

    public GameObject Item3Frame; 

    public GameObject Item4Frame; 

    public GameObject Item5Frame; 

    public Scrollbar scrollbar; 

    public Text descriptionText; 

    public Text moneyText; 

    public Text haveNumberText; 

    public Text marlboroText; 

    public Text mildsevenText; 

    public Text sevenstarsText; 

    public Text larkText; 

    public Text hopeText; 

    ////////////////////////////////////////////
    // private object, variable
    private Transform cameraTrans; 

    private Transform initialTrans; 

    private BuySceneCameraControl bscc; 

    private bool isExit = false; 

    private int state = 0; 

    private int state_count = 5; 

    private int show_count = 4; 

    private int axis = 0; 

    private int camera_state = 0; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip moveSound; 

    public AudioClip enterSound; 

    private AudioSource audioSource; 

    enum Item 
    {
        marlboro = 260, 
        mildseven = 255, 
        sevenstars = 245, 
        lark = 230, 
        hope = 130
    }

    // Start is called before the first frame update
    void Start()
    {
        Item1Frame.SetActive(true);    
        bscc = CameraControl.GetComponent<BuySceneCameraControl>(); 
        this.audioSource = GetComponent<AudioSource>(); 
        moneyText.text = "" + GameDirector.money; 

        // 年代別　値段の表示更新
        if(GameDirector.age==1995)
        {
            marlboroText.text = "" + (int)Item.marlboro; 
            mildsevenText.text = "" + (int)Item.mildseven; 
            sevenstarsText.text = "" + (int)Item.sevenstars; 
            larkText.text = "" + (int)Item.lark; 
            hopeText.text = "" + (int)Item.hope; 
        }
        else if(GameDirector.age==2015)
        {
            marlboroText.text = "" + (int)Item.marlboro*2; 
            mildsevenText.text = "" + (int)Item.mildseven*2; 
            sevenstarsText.text = "" + (int)Item.sevenstars*2; 
            larkText.text = "" + (int)Item.lark*2; 
            hopeText.text = "" + (int)Item.hope*2; 
        }
        else if(GameDirector.age==2035)
        {
            marlboroText.text = "" + (int)Item.marlboro*4; 
            mildsevenText.text = "" + (int)Item.mildseven*4; 
            sevenstarsText.text = "" + (int)Item.sevenstars*4; 
            larkText.text = "" + (int)Item.lark*4; 
            hopeText.text = "" + (int)Item.hope*4; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////
        // 「isExit」グリッドを選択しているかどうかのチェック
        if(isExit)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isExit = false; 
            }
        }
        else 
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                isExit = true; 
            }
        }

        BackFrame.SetActive(isExit); 

        ////////////////////////////////////////////
        // 選択位置の更新処理
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

        ////////////////////////////////////////////
        // 現在の選択位置に応じて, 説明文などを変更していく
        if(state == 0 & !isExit)
        {
            Item1Frame.SetActive(true); 
            Item2Frame.SetActive(false); 
            Item3Frame.SetActive(false); 
            Item4Frame.SetActive(false); 
            Item5Frame.SetActive(false); 

            bscc.updateState(0);

            descriptionText.text = @"
Marlboroは
フィリップモリス社が
製造するタバコの銘柄.
Marlboroという商品名
はイングランド南部の
にある町Marlborough
が由来とされている.";

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 

                if(GameDirector.age==1995)
                {
                    if(GameDirector.money>=(int)Item.marlboro)
                    {
                        GameDirector.money -= (int)Item.marlboro; 
                        GameDirector.itemBox[0] += 1; 
                    }
                } 
                else if(GameDirector.age==2015)
                {
                    if(GameDirector.money >= (int)Item.marlboro*2)
                    {
                        GameDirector.money -= (int)Item.marlboro*2; 
                        GameDirector.itemBox[0] += 1; 
                    }
                }
                else if(GameDirector.age==2035)
                {
                    if(GameDirector.money >= (int)Item.marlboro*4)
                    {
                        GameDirector.money -= (int)Item.marlboro*4; 
                        GameDirector.itemBox[0] += 1; 
                    }
                }

            }
        }
        else if(state == 1 & !isExit)
        {
            Item1Frame.SetActive(false); 
            Item2Frame.SetActive(true); 
            Item3Frame.SetActive(false); 
            Item4Frame.SetActive(false); 
            Item5Frame.SetActive(false); 

            descriptionText.text = @"
MILD SEVENは
日本たばこ産業(JT)が
製造するタバコの銘柄.
Seven Starsの
マイルドバージョン
として発売された.";

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 

                if(GameDirector.age==1995)
                {
                    if(GameDirector.money>=(int)Item.mildseven)
                    {
                        GameDirector.money -= (int)Item.mildseven; 
                        GameDirector.itemBox[1] += 1; 
                    }
                } 
                else if(GameDirector.age==2015)
                {
                    if(GameDirector.money >= (int)Item.mildseven*2)
                    {
                        GameDirector.money -= (int)Item.mildseven*2; 
                        GameDirector.itemBox[1] += 1; 
                    }
                }
                else if(GameDirector.age==2035)
                {
                    if(GameDirector.money >= (int)Item.mildseven*4)
                    {
                        GameDirector.money -= (int)Item.mildseven*4; 
                        GameDirector.itemBox[1] += 1; 
                    }
                }
            }

        }
        else if(state == 2 & !isExit)
        {
            Item1Frame.SetActive(false); 
            Item2Frame.SetActive(false); 
            Item3Frame.SetActive(true); 
            Item4Frame.SetActive(false); 
            Item5Frame.SetActive(false); 

            descriptionText.text = @"
SEVEN STARSは
日本たばこ産業(JT)が
製造するタバコの銘柄.
愛称は「セッタ」, 
「セスタ」,「セッター」
「ブンタ」,「七つ星」
など. ";

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 

                if(GameDirector.age==1995)
                {
                    if(GameDirector.money>=(int)Item.sevenstars)
                    {
                        GameDirector.money -= (int)Item.sevenstars; 
                        GameDirector.itemBox[2] += 1; 
                    }
                } 
                else if(GameDirector.age==2015)
                {
                    if(GameDirector.money >= (int)Item.sevenstars*2)
                    {
                        GameDirector.money -= (int)Item.sevenstars*2; 
                        GameDirector.itemBox[2] += 1; 
                    }
                }
                else if(GameDirector.age==2035)
                {
                    if(GameDirector.money >= (int)Item.sevenstars*4)
                    {
                        GameDirector.money -= (int)Item.sevenstars*4; 
                        GameDirector.itemBox[2] += 1; 
                    }
                }
            }

        }
        else if(state == 3 & !isExit)
        {
            Item1Frame.SetActive(false); 
            Item2Frame.SetActive(false); 
            Item3Frame.SetActive(false); 
            Item4Frame.SetActive(true); 
            Item5Frame.SetActive(false); 

            descriptionText.text = @"
LARKは
フィリップモリス社が
製造するタバコの銘柄.
店頭などのポスター広告
のキャラクターに
高倉健を起用した時期
があった.";

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
               
                if(GameDirector.age==1995)
                {
                    if(GameDirector.money>=(int)Item.lark)
                    {
                        GameDirector.money -= (int)Item.lark; 
                        GameDirector.itemBox[3] += 1; 
                    }
                } 
                else if(GameDirector.age==2015)
                {
                    if(GameDirector.money >= (int)Item.lark*2)
                    {
                        GameDirector.money -= (int)Item.lark*2; 
                        GameDirector.itemBox[3] += 1; 
                    }
                }
                else if(GameDirector.age==2035)
                {
                    if(GameDirector.money >= (int)Item.lark*4)
                    {
                        GameDirector.money -= (int)Item.lark*4; 
                        GameDirector.itemBox[3] += 1; 
                    }
                }
            } 
        }
        else if(state == 4 & !isExit)
        {
            Item1Frame.SetActive(false); 
            Item2Frame.SetActive(false); 
            Item3Frame.SetActive(false); 
            Item4Frame.SetActive(false); 
            Item5Frame.SetActive(true); 

            descriptionText.text = @"
HOPEは
日本たばこ産業(JT)が
製造するタバコの銘柄.
日本初のフィルター付き
タバコとして発売された.";
           
            bscc.updateState(1); 

            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
            
                if(GameDirector.age==1995)
                {
                    if(GameDirector.money>=(int)Item.hope)
                    {
                        GameDirector.money -= (int)Item.hope; 
                        GameDirector.itemBox[4] += 1; 
                    }
                } 
                else if(GameDirector.age==2015)
                {
                    if(GameDirector.money >= (int)Item.hope*2)
                    {
                        GameDirector.money -= (int)Item.hope*2; 
                        GameDirector.itemBox[4] += 1; 
                    }
                }
                else if(GameDirector.age==2035)
                {
                    if(GameDirector.money >= (int)Item.hope*4)
                    {
                        GameDirector.money -= (int)Item.hope*4; 
                        GameDirector.itemBox[4] += 1; 
                    }
                }
            }
        }
        else if(isExit)
        {
            Item1Frame.SetActive(false); 
            Item2Frame.SetActive(false); 
            Item3Frame.SetActive(false); 
            Item4Frame.SetActive(false); 
            Item5Frame.SetActive(false); 

            descriptionText.text = ""; 
            haveNumberText.text = ""; 
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.UnloadSceneAsync("Buy19xxScene"); 
                SceneManager.LoadScene("Shop19xxScene", LoadSceneMode.Additive); 
            }
        }

        uiUpdate(); 

    }

    ////////////////////////////////////////////
    // 現在の所持金やアイテムの所持数などの表示を更新
    private void uiUpdate()
    {
        scrollbar.value = (float)(bscc.getState()) / (float)(state_count - show_count); 
        moneyText.text = "" + GameDirector.money; 
        if(!isExit)
        {
                haveNumberText.text = "" + GameDirector.itemBox[state]; 
        }
    }
}
