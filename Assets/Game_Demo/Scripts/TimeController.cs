using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    float defauleFixedDeltaTime;
    private static bool isbulletTime = false;

    void Awake() {
    
        defauleFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void BulletTime(bool isTime){
        if(isTime){
            Time.timeScale = bulletTimeScale;
            isbulletTime = true;
        }
        else{
            Time.timeScale = 1f;
            isbulletTime = false;
        }
        
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
    }
    
    public static bool GetIsbulletTime() {
        return isbulletTime;
    }
}
