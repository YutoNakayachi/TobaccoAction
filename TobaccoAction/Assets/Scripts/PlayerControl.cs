using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable 
    public GameObject parentObj; 

    public GameObject beamPrefab; 

    public GameObject beamEffectPrefab; 

    public GameObject tabakoEffectPrefab; 

    public GameObject gameDirector; 

    public GameObject policePrefab; 

    public GameObject damagePrefab; 

    public float speed = 5.0f; 

    public float jumpForce = 300.0f; 

    ////////////////////////////////////////////
    // private object, variable
    private SpriteRenderer spRenderer; 

    private Animator anim; 

    private Rigidbody2D rb2d; 

    private Transform shootPoint; 

    private Vector2 pos; 

    private GameDirector gd; 

    private GameObject punch; 

    private PoliceControl policeControl; 

    private bool isGround = true; 

    private bool combFlag = false; 

    private bool beamEnabled = true; 

    private bool isSmoking = false; 

    private bool isBeam = false; 

    private bool isDead = false; 

    private bool isSave = false; 

    private bool tabakoEnabled = false; 

    private bool moneyFlag = true; 

    private float combTime = 0.0f; 

    private float velX; 

    private float velY; 

    private int[] npBox = new int[5]; 

    private int npVal = 0; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip punchSound; 

    public AudioClip punch2Sound; 

    public AudioClip beamSound; 

    public AudioClip jumpSound; 

    public AudioClip fallSound; 

    public AudioClip healSound; 

    public AudioClip moneySound; 

    public AudioClip alarmSound; 

    private AudioSource audioSource; 

    void Start()
    {
        this.spRenderer = GetComponent<SpriteRenderer>(); 
        this.rb2d = GetComponent<Rigidbody2D>(); 
        this.anim = GetComponent<Animator>(); 
        this.gd = gameDirector.GetComponent<GameDirector>(); 
        this.punch = GameObject.Find("PlayerAttackCollider"); 
        this.policeControl = policePrefab.GetComponent<PoliceControl>(); 
        this.audioSource = GetComponent<AudioSource>(); 

        npBox[0] = 20; 
        npBox[1] = 25; 
        npBox[2] = 15; 
        npBox[3] = 20; 
        npBox[4] = 10; 
    }

    void Update()
    {
        ////////////////////////////////////////////
        // プライヤーの入力を受け付けない処理
        if(Time.timeScale==0 | gd.hpZeroCheck() | GameDirector.fadeFalg)
        {
            if(gd.hpZeroCheck())
            {
                anim.SetBool("isDead", true); 
            }
            return; 
        }

        if(isDead)
        {
            anim.SetBool("isDead", true); 
            return; 
        }

        ////////////////////////////////////////////
        // 移動処理
        // x -- 左:-1, 右:+1, other:0 
        float x = 0.0f; 
        if(!isBeam & !isSmoking)
        {
            x = Input.GetAxisRaw("Horizontal");
        }

        anim.SetFloat("Speed", Mathf.Abs( x*speed )); 

        if(x<0)
        {
            spRenderer.flipX = true; 
            punch.transform.rotation = Quaternion.Euler(0, 180, 0); 
        }
        else if(x>0)
        {
            spRenderer.flipX = false; 
            punch.transform.rotation = Quaternion.Euler(0, 0, 0); 
        }

        rb2d.AddForce( Vector2.right * x * speed); 

        ////////////////////////////////////////////
        // ジャンプ処理
        if( Input.GetKeyDown(KeyCode.Space) & isGround)
        {
            audioSource.PlayOneShot(jumpSound); 
            anim.SetBool("isJump", true); 
            rb2d.AddForce( Vector2.up * jumpForce ); 
            isGround = false; 
        }

        ////////////////////////////////////////////
        // 攻撃処理
        combTime += Time.deltaTime;
        if( Input.GetKeyDown(KeyCode.K) & isGround ) 
        {
            if(combFlag)
            {
                if(combTime<0.5f)
                {
                    audioSource.PlayOneShot(punch2Sound); 
                    anim.SetTrigger("TrgAttack2"); 
                    combFlag = false; 
                }
                else
                {
                    audioSource.PlayOneShot(punchSound); 
                    anim.SetTrigger("TrgAttack"); 
                }
            }
            else
            {
                audioSource.PlayOneShot(punchSound); 
                anim.SetTrigger("TrgAttack"); 
                combFlag = true; 
            }
            combTime = 0.0f; 
        }

        ////////////////////////////////////////////
        // ビーム発射処理
        if(beamEnabled & !checkRun() & GameDirector.mp >= 5)
        {
            if(Input.GetKeyDown(KeyCode.B) & isGround)
            {
                audioSource.PlayOneShot(beamSound); 
                beamEnabled = false; 
                isBeam = true; 
                anim.SetTrigger("TrgBeam"); 
                shootPoint = this.transform; 
                pos = shootPoint.position; 
                if(spRenderer.flipX)
                {
                    pos.x -= 0.5f; 
                }
                else 
                {
                    pos.x += 0.5f; 
                }
                Instantiate(beamEffectPrefab, transform.position, transform.rotation); 
                StartCoroutine("Beam"); 
                gd.mpDecrease(5); 
            }
        }

        ////////////////////////////////////////////
        // 喫煙処理
        tabacoCheck(); 
        if( Input.GetKeyDown(KeyCode.T) & isGround & !checkRun() & tabakoEnabled) 
        {
            
            if(!isSave)
            {
                //policePrefab.SetActive(true); 
                policeControl.saveCheck(false); 
                audioSource.PlayOneShot(alarmSound);
            }
            audioSource.PlayOneShot(healSound); 
            isSmoking = true; 
            npVal = npBox[GameDirector.setId]; 
            Instantiate(tabakoEffectPrefab, transform.position, transform.rotation); 
            GameDirector.balanceCount[GameDirector.setId] -= 1; 
            if(GameDirector.balanceCount[GameDirector.setId]==0)
            {
                GameDirector.itemBox[GameDirector.setId] -= 1; 
                GameDirector.setId = -1; 
            }
            anim.SetTrigger("TrgSmoking");
            StartCoroutine("Smoking"); 
        }

        ////////////////////////////////////////////
        // 地面と接地していたら
        if(isGround)
        {
            anim.SetBool("isJump", false); 
            anim.SetBool("isFall", false); 
        }

        velX = rb2d.velocity.x; 
        velY = rb2d.velocity.y; 

        ////////////////////////////////////////////
        // 落下処理
        if( velY < -0.1f ) anim.SetBool("isFall", true); 

        ////////////////////////////////////////////
        // 速度の上限を決める
        if(Mathf.Abs(velX) > 5)
        {
            if(velX>5.0f) rb2d.velocity = new Vector2( 5.0f, velY ); 
            if(velX<-5.0f) rb2d.velocity = new Vector2( -5.0f, velY ); 
        }

    }


    void OnCollisionEnter2D( Collision2D col)
    {
        if(col.gameObject.tag == "ground") 
        {
            isGround = true; 
            audioSource.PlayOneShot(fallSound); 
        }
    }

 
    void OnTriggerEnter2D( Collider2D col ) 
    {
        if(col.gameObject.tag == "enemy")
        {
            anim.SetTrigger("TrgDamaged"); 
            var parent = parentObj.transform; 
            Instantiate(damagePrefab, transform.position, transform.rotation, parent); 
            gd.hpDecrease(10); 
        }

        if(col.gameObject.tag == "shop")
        {
            gd.shopCheck(true); 
        }

        if(col.gameObject.tag == "notShop")
        {
            gd.shopCheck(false); 
        }

        if(col.gameObject.tag == "money100")
        {
            // お金処理
            if(moneyFlag)
            {
                moneyFlag = false; 
                gd.moneyUpdate(100);  
                audioSource.PlayOneShot(moneySound);
                Destroy(col.gameObject); 
            }
            moneyFlag = true; 
        }

        if(col.gameObject.tag == "money1000")
        {
            // お金処理
            if(moneyFlag)
            {
                moneyFlag = false; 
                gd.moneyUpdate(500);  
                audioSource.PlayOneShot(moneySound);
                Destroy(col.gameObject); 
            }
            moneyFlag = true; 
        }

        if(col.gameObject.tag == "saveArea")
        {
            isSave = true; 
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "saveArea")
        {
            isSave = false; 
        }
    }

    public void deathCheck()
    {
        isDead = true; 
    }

    private bool checkRun()
    {
        velX = rb2d.velocity.x; 
        if(Mathf.Abs(velX)<=0.5f)
        {
            return false; 
        }
        else
        {
            return true; 
        }
    }

    private void tabacoCheck()
    {
        tabakoEnabled = false; 
        if(GameDirector.setId!=-1)
        {
            if(GameDirector.itemBox[GameDirector.setId]!=0)
            {
                if(GameDirector.balanceCount[GameDirector.setId]==0)
                {
                    balanceInitial(GameDirector.setId); 
                }
                tabakoEnabled = true; 
            }
        }
    }

    private void balanceInitial(int id)
    {
        if(id==0 | id==2)
        {
            GameDirector.balanceCount[id] = 3; 
        }
        else 
        {
            GameDirector.balanceCount[id] = 2; 
        }

    }

    private IEnumerator Beam()
    {
        yield return new WaitForSeconds(0.5f); 
        Instantiate(beamPrefab, pos, transform.rotation);
        yield return new WaitForSeconds(0.5f); 
        beamEnabled = true; 
        isBeam = false; 
    }

    private IEnumerator Damaged()
    {
        yield return new WaitForSeconds(1); 
        anim.SetTrigger("TrgDamaged"); 
    }

    private IEnumerator Smoking()
    {
        yield return new WaitForSeconds(2.2f); 
        gd.mpIncrease(npVal); 
        isSmoking = false; 
    }
}
