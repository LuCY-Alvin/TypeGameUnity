using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;

    float defauleFixedDeltaTime;
    private static bool isBulletTime = false;

    void Awake() {
        isBulletTime = false;
        defauleFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void StartBulletTime(){
        isBulletTime = true;
        Time.timeScale = bulletTimeScale;
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
    }

    public void EndBulletTime(){
        isBulletTime = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
    }

    public static bool GetIsBulletTime() {
        return isBulletTime;
    }

    // public void BulletTime(bool isTime){
    //     if(isTime){
    //         Time.timeScale = bulletTimeScale;
    //     }
    //     else{
    //         Time.timeScale = 1f;
    //     }
    //     Time.fixedDeltaTime = defauleFixedDeltaTime * Time.timeScale;
    // }

    // public void SetIsbulletTime(bool _isbulletTime){
    //     isbulletTime = _isbulletTime;
    // }
    

}
