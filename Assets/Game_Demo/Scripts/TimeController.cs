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

    public void BulletTime(bool isTime){
        if(isTime){
            Time.timeScale = bulletTimeScale;
        }
        else{
            Time.timeScale = 1f;
        }
        
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
    }

    // void Update() {
    //     if( Input.GetKeyDown(KeyCode.Z)){
    //         BulletTime();
    //     }

    //     if( Input.GetKeyDown(KeyCode.X)){
    //         BulletTimeEnd();
    //     }
        
    // }
}
