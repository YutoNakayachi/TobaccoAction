using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject player; 

    public GameObject gameDirector; 

    public GameObject Silen; 

    public GameObject effectPrefab; 

    public float speed = 1.5f; 

    ////////////////////////////////////////////
    // private object, variable
    private Animator anim; 

    private Rigidbody2D rb2d; 

    private BoxCollider2D bc2d; 

    private Transform policeTrans; 

    private SpriteRenderer spRenderer; 

    private SpriteRenderer pSpRenderer; 

    private GameDirector gd; 

    private Vector3 initialPos; 

    private float timeInterval = 1.0f; 

    private float timeElapsed = 0.0f; 

    private bool isGround = false; 

    private bool isSave = true; 

    private bool isGet = false; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); 
        this.anim = GetComponent<Animator>(); 
        this.rb2d = GetComponent<Rigidbody2D>(); 
        this.bc2d = GetComponent<BoxCollider2D>(); 
        this.policeTrans = GetComponent<Transform>(); 
        this.spRenderer = GetComponent<SpriteRenderer>(); 
        this.gd = gameDirector.GetComponent<GameDirector>(); 
        pSpRenderer = player.GetComponent<SpriteRenderer>();   
        initialPos = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0.0f)
        {
            return; 
        }

        if(isSave)
        {
            Silen.SetActive(false); 
            timeElapsed = 0.0f; 
            anim.SetBool("isGet", false); 
            anim.SetBool("isRun", false); 
            gd.policeGetUpdate(false); 
            return ;
        }

        ////////////////////////////////////////////
        // 以下 not isSave 
        Silen.SetActive(true); 
        
        Vector2 pPos = player.transform.position; 
        Vector2 myPos = this.transform.position; 

        float velX = rb2d.velocity.x; 
        float velY = rb2d.velocity.y; 

        float dif = myPos.x - pPos.x; 
        float distance = Mathf.Abs(dif);

        ////////////////////////////////////////////
        // 向きの決定
        if(dif>0)
        {
            spRenderer.flipX = true; 
        }
        else
        {
            spRenderer.flipX = false; 
        }

        ////////////////////////////////////////////
        // 移動処理
        if(distance <= 1.0f)
        {
            anim.SetBool("isRun", false); 
            anim.SetBool("isGet", true); 

            // timeInterval秒間, isGetが続くか確認
            timeElapsed += Time.deltaTime; 
            if(timeElapsed >= timeInterval)
            {
                // playerから罰金を徴収する処理
                Instantiate(effectPrefab, transform.position, transform.rotation); 
                isSave = true; 
                gd.moneyUpdate(-1000); 
                initialPosition(); 
            }
        }
        else 
        {
            // timeElapsedの初期化
            timeElapsed = 0.0f; 

            anim.SetBool("isRun", true); 
            anim.SetBool("isGet", false); 
            if(dif>0)
            {
                rb2d.AddForce( Vector2.left * speed ); 
            }
            else 
            {
                rb2d.AddForce( Vector2.right * speed); 
            }
        }

        ////////////////////////////////////////////
        // 最大速度以内に収める
        velX = rb2d.velocity.x; 
        velY = rb2d.velocity.y; 

        if(Mathf.Abs(velX) > 5)
        {
            if(velX>5.0f) rb2d.velocity = new Vector2( 5.0f, velY); 
            if(velX<-5.0f) rb2d.velocity = new Vector2( -5.0f, velY); 
        }

        if(isGet)
        {
            // 周りの敵も攻撃してこない状態をつくる.
            gd.policeGetUpdate(true); 
        }
        else 
        {
            gd.policeGetUpdate(false); 
        }

    }

    public void groundUpdate(bool val)
    {
        if(val)
        {
            isGround = true; 
        }
        else 
        {
            isGround = false; 
        }
    }

    public void saveCheck(bool val)
    {
        if(val)
        {
            isSave = true; 
        }
        else 
        {
            isSave = false; 
        }
    }

    private void initialPosition()
    {
        transform.position = initialPos; 
        groundUpdate(true); 
    }
}
