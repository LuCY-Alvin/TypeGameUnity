using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.5f;

    float defauleFixedDeltaTime;

    void Awake() {
    
        defauleFixedDeltaTime = Time.fixedDeltaTime;
    }

    void BulletTime(){
        Time.timeScale = bulletTimeScale;
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
        Debug.Log("Slow");
    }

    void BulletTimeEnd(){
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
        Debug.Log("Ori");
    }


    void Update() {
        if( Input.GetKeyDown(KeyCode.Z)){
            BulletTime();
        }

        if( Input.GetKeyDown(KeyCode.X)){
            BulletTimeEnd();
        }
        
    }
}
