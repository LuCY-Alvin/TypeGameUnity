using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{   
    public TimeController btController;
    public CastController castController;
    public RectMask2D _hpMask;
    public RectMask2D _mpMask;
    public RectTransform _mpSpellMask;
    public GameOver _gameOver;

    private float _maxRightMask;
    private float _initRightMask;

    public int currentHp = 100;
    public int currentMp = 100;
    public int initMax = 200;

    public TMP_Text _hpText;
    public TMP_Text _mpText;
    public TMP_Text _mpSpellText;

    // Start is called before the first frame update
    void Start()
    {
        SetValue(currentHp, "hp");
        SetValue(currentMp, "mp");
        
        changeInvoke(false);
    }

    public void changeInvoke(bool isBulletTime) {
        CancelInvoke("ManaHandler");

        if (isBulletTime) {
            InvokeRepeating ("ManaHandler", 0f, 0.01f); 
        } else {
            InvokeRepeating ("ManaHandler", 0f, 0.5f); 
        }
    }

    public int GetCurrentHp() {
        return currentHp;
    }

    public int GetCurrentMp() {
        return currentMp;
    }

    public void SetValue(int newValue, string type)
    {
        if (newValue > initMax) {
            return;
        }

        if (newValue <= 0) {
            // TODO: Die here
            _gameOver.Setup();
        }

        if (type == "hp") {
            var padding =  _hpMask.padding;
            padding.z = initMax - newValue;
            _hpMask.padding = padding;
            currentHp = newValue;

            _hpText.text = $"{currentHp} / {initMax}";
        } else {
            var padding =  _mpMask.padding;
            padding.z = initMax - newValue;
            _mpMask.padding = padding;
            currentMp = newValue;

            Vector3 spellMaskVector = _mpSpellMask.localScale;
            spellMaskVector.x = (float)currentMp / 200;

            _mpSpellMask.localScale = spellMaskVector;

            _mpSpellText.text = $"{currentMp} / {initMax}";
            _mpText.text = $"{currentMp} / {initMax}";
        }
    }

    void ManaHandler() {
        var isBulletTime = TimeController.GetIsBulletTime();

        var unit = 1;
        if (currentMp <= 0) {
            print("Stop BT");
            btController.EndBulletTime();
            castController.EndCast();
            //btController.BulletTime(false);
        }

        if (currentMp <= initMax && currentMp >= 0) {
            if (isBulletTime) {
                currentMp -= (2 * unit);
                if (currentMp <= 0) {
                    currentMp = 0;
                }
            } else {
                currentMp += unit;
                if (currentMp >= initMax) {
                    currentMp = initMax;
                }
            }

            SetValue(currentMp, "mp");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
