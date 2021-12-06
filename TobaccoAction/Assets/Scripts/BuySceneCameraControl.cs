using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySceneCameraControl : MonoBehaviour
{
    private Camera _camera; 

    private float camera_y = 0.0f; 

    private int state = 0 ; 

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main; 
        camera_y = transform.position.y;    
    }

    // Update is called once per frame
    void Update()
    {
        // カメラの移動処理
        transform.position = new Vector3(
            transform.position.x, 
            camera_y - 2.50f * (float)state, 
            transform.position.z
        ); 
    }

    public void updateState(int val)
    {
        state = val; 
    }

    public int getState()
    {
        return state; 
    }
}
