using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamControl : MonoBehaviour
{
    ////////////////////////////////////////////
    // public object, variable
    public GameObject player; 

    public GameObject beamEffect; 

    public float speed = 1.0f;
    
    ////////////////////////////////////////////
    // private object, variable 
    private SpriteRenderer spRenderer; 

    private Rigidbody2D rb2d; 

    private bool dir_flag; 

    private float timeElapsed = 0.0f; 

    private float timeInterval = 0.2f; 

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.0f; 
        player = GameObject.Find("Player"); 
        rb2d = GetComponent<Rigidbody2D>(); 
        spRenderer = player.GetComponent<SpriteRenderer>(); 

        if(spRenderer.flipX) dir_flag = true; 
        else dir_flag = false;     
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; 

        ////////////////////////////////////////////
        // 移動処理
        if(timeElapsed>=timeInterval)
        {
            Instantiate(beamEffect, transform.position, transform.rotation); 
            timeElapsed = 0.0f; 
        }

        ////////////////////////////////////////////
        // 発射方向の決定
        if(dir_flag)
        {
            // 左
            rb2d.AddForce( Vector2.right * -1 * speed ); 
        }
        else 
        {
            // 右
            rb2d.AddForce( Vector2.right * speed ); 
        }

        float velX = rb2d.velocity.x; 
        float velY = rb2d.velocity.y; 

        // 速度の上限を決める
        if(Mathf.Abs(velX)>20)
        {
            if(velX>15.0f) rb2d.velocity = new Vector2( 20.0f, velY ); 
            if(velY<-15.0f) rb2d.velocity = new Vector2( -20.0f, velY); 
        }
    }

    // 何らかのオブジェクトにあたったら消える
    void OnCollisionEnter2D(Collision2D col) 
    {
        Destroy(gameObject); 
    }
}
