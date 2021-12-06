using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Enemy1Control : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject player; 

    public GameObject hpGage; 

    public GameObject moneyPrefab; 

    public GameObject parentObj; 

    public GameObject enemyFactory; 

    public GameObject damagePrefab; 

    public GameObject beamDamagePrefab; 

    public GameObject deathPrefab; 

    public GameObject damageTextPrefab; 

    public GameObject attackCollider; 

    public float speed = 1; 

    public int hp = 100; 

    public int damageVal = 0;

    ////////////////////////////////////////////
    // private object, variable
    private Animator anim; 

    private Rigidbody2D rb2d; 

    private BoxCollider2D bc2d; 

    private SpriteRenderer spRenderer; 

    private SpriteRenderer pSpRenderer; 

    private Slider enemy1HpSlider;

    private EnemyFactory ef; 

    private bool isDamaged = false;

    private bool isDamageEffect = false; 

    private bool isGround = false;  

    private bool isDead = false; 

    private bool deadEffect = true; 

    ////////////////////////////////////////////
    // Audio Object
    public AudioClip punchSound; 

    public AudioClip deadSound; 

    private AudioSource audioSource; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player"); 
        parentObj = GameObject.Find("MainGameSceneObject"); 
        enemyFactory = GameObject.Find("EnemyFactory"); 

        this.ef = enemyFactory.GetComponent<EnemyFactory>(); 
        this.anim = GetComponent<Animator>(); 
        this.rb2d = GetComponent<Rigidbody2D>(); 
        this.bc2d = GetComponent<BoxCollider2D>(); 
        this.spRenderer = GetComponent<SpriteRenderer>(); 
        pSpRenderer = player.GetComponent<SpriteRenderer>(); 
        this.enemy1HpSlider = hpGage.GetComponent<Slider>(); 
        this.audioSource = GetComponent<AudioSource>(); 

        enemy1HpSlider.value = (float)hp / 100.0f; 
    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////////////////
        // エネミーの動きを止める処理
        if(Time.timeScale == 0 | GameDirector.fadeFalg)
        {
            return ; 
        }

        if(hp<=0)
        {
            isDead = true; 
        }

        ////////////////////////////////////////////
        // 死亡処理
        if(isDead)
        {
            anim.SetBool("isDead", true); 
            if(deadEffect)
            {
                audioSource.PlayOneShot(deadSound);
                var parent = parentObj.transform; 
                Instantiate(deathPrefab, transform.position, transform.rotation, parent); 
                StartCoroutine("MoneyInst"); 

                if(gameObject.tag=="enemy1")
                {
                    ef.enemyCountUpdate(1); 
                }
                else if(gameObject.tag=="enemy2")
                {
                    ef.enemyCountUpdate(2); 
                }
                else if(gameObject.tag=="enemy3")
                {
                    ef.enemyCountUpdate(3); 
                }

                deadEffect = false; 
            }
            Destroy(gameObject, 1.0f); 
            return ; 
        }

        ////////////////////////////////////////////
        // ダメージ処理
        if(isDamaged)
        {
            if(isDamageEffect)
            {
                var parent = parentObj.transform; 
                Instantiate(damagePrefab, transform.position, transform.rotation, parent); 
                isDamageEffect = false; 
            }

            anim.SetBool("isDamaged", true); 
            anim.SetBool("isRun", false); 
            StartCoroutine("Damaged"); 
        }
        else 
        {
            anim.SetBool("isDamaged", false); 
        }

        ////////////////////////////////////////////
        // 地面から離れているならば何もしない.
        if(!isGround)
        {
            anim.SetBool("isRun", false); 
            return; 
        }

        ////////////////////////////////////////////
        // 移動処理
        Vector2 pPos = player.transform.position; 
        Vector2 myPos = this.transform.position; 

        float velX = rb2d.velocity.x; 
        float velY = rb2d.velocity.y; 

        float dif = myPos.x - pPos.x; 
        float distance = Mathf.Abs(dif); 

        if(dif>0)
        {
            spRenderer.flipX = true; 
            attackCollider.transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else
        {
            spRenderer.flipX = false; 
            attackCollider.transform.rotation = Quaternion.Euler(0, 180, 0); 
        }

        if(distance <= 1.0f)
        {
            if(Random.value < 0.02f)
            {
                audioSource.PlayOneShot(punchSound);
                anim.SetTrigger("TrgAttack"); 
                if(Random.value < 0.2)
                {
                    anim.SetTrigger("TrgAttack2"); 
                }
            }

            anim.SetBool("isRun", false); 
        }
        else 
        {
            anim.SetBool("isRun", true); 

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
        // 最大速度以下にする.
        velX = rb2d.velocity.x; 
        velY = rb2d.velocity.y; 

        if(Mathf.Abs(velX) > 5)
        {
            if(velX>5.0f) rb2d.velocity = new Vector2( 5.0f, velY); 
            if(velX<-5.0f) rb2d.velocity = new Vector2( -5.0f, velY); 
        }

    }

    void OnCollisionEnter2D( Collision2D col)
    {
        if(col.gameObject.tag == "beam") 
        {
            //anim.SetTrigger("TrgDamaged"); 
            isDamaged = true;
            var parent = parentObj.transform; 
            //Instantiate(damageTextPrefab, transform.position, transform.rotation, parent);
            Instantiate(beamDamagePrefab, transform.position, transform.rotation, parent);  
            hpDecrease(70); 
        }
    }

    void OnTriggerEnter2D( Collider2D col)
    {
        if(col.gameObject.tag == "punch")
        {
            isDamaged = true; 
            isDamageEffect = true; 

            if(pSpRenderer.flipX)
            {
                rb2d.AddForce( Vector2.left * 1000.0f ); 
            }
            else 
            {
                rb2d.AddForce( Vector2.right * 1000.0f ); 
            }

            rb2d.AddForce( Vector2.up * 100.0f ); 
            damageVal = 30; 
            hpDecrease(damageVal); 
            var parent = parentObj.transform; 
            Instantiate(damageTextPrefab, transform.position, transform.rotation, parent);
        }
    }

    public int damageGetter()
    {
        return damageVal; 
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
    

    private void uiUpdate()
    {
        if(gameObject.tag=="enemy2")
        {
            enemy1HpSlider.value = (float)hp / 200.0f; 
        }
        else 
        {
            enemy1HpSlider.value = (float)hp / 100.0f; 
        }
    }

    private void hpDecrease(int val)
    {
        hp -= val; 
        uiUpdate(); 
    }

    private IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.25f); 
        isDamaged = false; 
    }

    private IEnumerator MoneyInst()
    {
        yield return new WaitForSeconds(0.25f); 
        var parent = parentObj.transform; 
        Instantiate(moneyPrefab, transform.position , transform.rotation, parent);
    }

}
