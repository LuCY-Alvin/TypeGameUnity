using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{   
    public TimeController btController;
    public RectMask2D _hpMask;
    public RectMask2D _mpMask;

    private float _maxRightMask;
    private float _initRightMask;

    public int currentHp = 100;
    public int currentMp = 100;
    public int initMax = 200;
    // Start is called before the first frame update
    void Start()
    {
    	// hpText.text = '123';     
        // x = left, z = right
        // _maxRightMask = _barRect.rect.width - _mask.padding.x - _mask.padding.z;
        // _hpIndicator.SetText($"{_health.Hp}/{_health.MaxHp}");
        // _initRightMask = _mask.padding.z;
        SetValue(currentHp, "hp");
        SetValue(currentMp, "mp");

        InvokeRepeating ("ManaHandler", 1, 1); 
    }

    public void SetValue(int newValue, string type)
    {
    	// var targetW = newValue * _maxRightMask / _health.MaxHp;
    	// var newRightMask = _maxRightMask + _initRightMask - targetW;
        
        if (type == "hp") {
            var padding =  _hpMask.padding;
            padding.z = initMax - newValue;
            _hpMask.padding = padding;
        } else {
            var padding =  _mpMask.padding;
            padding.z = initMax - newValue;
            _mpMask.padding = padding;
        }
    	
    	//_hpIndicator.SetText($"{newValue}/{_health.MaxHp}");
    }

    void ManaHandler() {
        var isbulletTime = TimeController.GetIsbulletTime();
        var unit = 10;
        if (currentMp < 50) {
            print("Stop BT");
            btController.BulletTime(false);
        }
        print(isbulletTime);
        if (currentMp + unit  <= initMax && currentMp - (2 * unit) >= 0) {
            if (isbulletTime) {
                currentMp -= (2 * unit);
            } else {
                currentMp += unit;
            }

            SetValue(currentMp, "mp");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}