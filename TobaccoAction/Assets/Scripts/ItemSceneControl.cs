using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class ItemSceneControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    // grid
    public GameObject Item1; 

    public GameObject Item2; 

    public GameObject Item3; 

    // frame 
    public GameObject BackFrame; 

    public GameObject Item1Frame; 

    public GameObject Item2Frame; 

    public GameObject Item3Frame; 

    // SetGrid
    public GameObject SetGrid1; 

    public GameObject SetGrid2; 

    public GameObject SetGrid3; 

    // Image
    public GameObject Item1Image; 

    public GameObject Item2Image; 

    public GameObject Item3Image; 

    public Scrollbar scrollbar; 

    public Text descriptionText; 

    // name 
    public Text item1NameText; 

    public Text item2NameText; 

    public Text item3NameText;

    // number
    public Text item1NumberText;

    public Text item2NumberText; 

    public Text item3NumberText;

    // set text
    public Text setText1; 

    public Text setText2; 

    public Text setText3; 

    // sprite
    public Sprite marlboro_sprite; 

    public Sprite mildseven_sprite; 

    public Sprite sevenstars_sprite; 

    public Sprite lark_sprite; 

    public Sprite hope_sprite; 

    ////////////////////////////////////////////
    // private object, variable
    // spriteRenderer
    private SpriteRenderer item1Sp; 

    private SpriteRenderer item2Sp; 

    private SpriteRenderer item3Sp; 

    private Sprite[] spriteBox = new Sprite[5]; 

    private string[] nameBox = new string[5]; 

    private int[] numberBox = new int[5]; 

    private bool[] itemCheck = new bool[5]; 

    private string[] descriptionBox = new string[5]; 

    private int axis = 0; 

    private int state = 0; 

    private int state_count = 3; 

    private int show_count = 3; 

    private int setid = -1; 

    private bool isExit = false; 

    private bool noItem = false; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip moveSound; 

    public AudioClip enterSound; 

    public AudioClip setSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        item1Sp = Item1Image.GetComponent<SpriteRenderer>();
        item2Sp = Item2Image.GetComponent<SpriteRenderer>();
        item3Sp = Item3Image.GetComponent<SpriteRenderer>();

        // sprite 
        spriteBox[0] = marlboro_sprite; 
        spriteBox[1] = mildseven_sprite; 
        spriteBox[2] = sevenstars_sprite; 
        spriteBox[3] = lark_sprite; 
        spriteBox[4] = hope_sprite; 

        // text 
        nameBox[0] = "Marlboro"; 
        nameBox[1] = "Mild Seven"; 
        nameBox[2] = "Seven Stars"; 
        nameBox[3] = "Lark"; 
        nameBox[4] = "Hope"; 

        // description 
        descriptionBox[0] = "3回使える. NPを20回復."; 
        descriptionBox[1] = "2回使える. NPを25回復."; 
        descriptionBox[2] = "3回使える. NPを15回復."; 
        descriptionBox[3] = "2回使える. NPを20回復."; 
        descriptionBox[4] = "2回使える. NPを10回復."; 

        this.audioSource = GetComponent<AudioSource>(); 

        // number 
        for(int i=0;i<5;++i)
        {
            numberBox[i] = GameDirector.itemBox[i]; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        initialActive(); 

        for(int i=0;i<5;++i)
        {
            if(numberBox[i]!=0)
            {
                itemCheck[i] = true; 
            }
        }

        state_count = checkStateCount(); 
        if(state_count==0)
        {
            noItem = true; 
        }

        ////////////////////////////////////////////
        // 入力処理
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.UnloadSceneAsync("ItemScene"); 
        }

        ////////////////////////////////////////////
        // 選択変更処理
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
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isExit = true; 
                audioSource.PlayOneShot(moveSound);
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                isExit = false; 
                audioSource.PlayOneShot(moveSound);
            }
        }

        if(isExit)
        {
            BackFrame.SetActive(true); 
            if(Input.GetKeyDown(KeyCode.Return))
            {
                audioSource.PlayOneShot(enterSound); 
                SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive); 
                SceneManager.UnloadSceneAsync("ItemScene"); 
            }
        }
        else 
        {
            BackFrame.SetActive(false); 
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
        // アイテム画面の表示処理
        int offset = 0; 
        int frame_state = 0; 
        if(state >= show_count)
        {
            offset = state - (show_count - 1); 
        }
        
        int ob = 0; 
        int[] idBox = new int[3]; 
        for(int i=0;i<3;++i)
        {
            idBox[i] = -1; 
        }

        for(int i = offset; i<5; ++i)
        {
            if(ob>=3)
            {
                break; 
            }
            
            if(itemCheck[i])
            {
                uiUpdate(ob, i); 
                idBox[ob] = i; 
                ob++; 
            }
        }

        ////////////////////////////////////////////
        // 選択フレームの表示処理
        frame_state = state - offset; 
        if(frame_state==0 & !isExit)
        {
            Item1Frame.SetActive(true); 

            if(idBox[0]!=-1)
            {
                descriptionText.text = descriptionBox[idBox[0]]; 
 
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    audioSource.PlayOneShot(setSound); 
                    GameDirector.setId = idBox[0]; 
                }

            }
        }
        else if(frame_state == 1 & !isExit)
        {
            Item2Frame.SetActive(true); 
            
            if(idBox[1]!=-1)
            {
                descriptionText.text = descriptionBox[idBox[1]]; 
                
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    audioSource.PlayOneShot(setSound);
                    GameDirector.setId = idBox[1]; 
                }

            }
        }
        else if(frame_state == 2 & !isExit)
        {
            Item3Frame.SetActive(true); 
        
            if(idBox[2]!=-1)
            {
                descriptionText.text = descriptionBox[idBox[2]]; 
    
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    audioSource.PlayOneShot(setSound);
                    GameDirector.setId = idBox[2]; 
                }

            }
        }

        if(state_count > show_count)
        {
            scrollbar.value = (float)offset / (float)(state_count - show_count); 
            scrollbar.size = 1.0f / (float)(state_count - show_count + 1); 
        }
        else 
        {
            scrollbar.value = 0; 
            scrollbar.size = 1.0f; 
        }

        if(!isExit)
        {
            if(noItem)
            {
                descriptionText.text = ""; 
            }
        }
        else 
        {
            descriptionText.text = ""; 
        } 
        
    }

    private void setInitial()
    {
        SetGrid1.SetActive(false); 
        SetGrid2.SetActive(false); 
        SetGrid3.SetActive(false); 

        setText1.text = ""; 
        setText2.text = ""; 
        setText3.text = ""; 
    }
    
    private void initialActive()
    {
        setInitial(); 
        Item1Frame.SetActive(false); 
        Item2Frame.SetActive(false); 
        Item3Frame.SetActive(false); 

        Item1.SetActive(false); 
        Item2.SetActive(false); 
        Item3.SetActive(false); 

        // いらないかも
        for(int i=0;i<5;++i)
        {
            itemCheck[i] = false; 
        }

    }

    private int checkStateCount()
    {
        int cnt = 0; 
        for(int i=0;i<5;++i)
        {
            if(itemCheck[i])
            {
                cnt++; 
            }
        }

        return cnt; 
    }

    // obに対してidを入れるという処理
    private void uiUpdate(int ob, int id)
    {
        // 1つ目の型に対して
        if(ob==0)
        {
            Item1.SetActive(true); 
            item1Sp.sprite = spriteBox[id]; 
            item1NameText.text = nameBox[id]; 
            item1NumberText.text = "" + numberBox[id]; 
            if(id==GameDirector.setId)
            {
                SetGrid1.SetActive(true); 
                setText1.text = "SET"; 
            }
        }
        else if(ob==1)
        {
            Item2.SetActive(true); 
            item2Sp.sprite = spriteBox[id]; 
            item2NameText.text = nameBox[id]; 
            item2NumberText.text = "" + numberBox[id]; 
            if(id==GameDirector.setId)
            {
                SetGrid2.SetActive(true); 
                setText2.text = "SET"; 
            }
        }
        else if(ob==2)
        {
            Item3.SetActive(true); 
            item3Sp.sprite = spriteBox[id]; 
            item3NameText.text = nameBox[id]; 
            item3NumberText.text = "" + numberBox[id]; 
            if(id==GameDirector.setId)
            {
                SetGrid3.SetActive(true); 
                setText3.text = "SET"; 
            }
        }
    }
}
