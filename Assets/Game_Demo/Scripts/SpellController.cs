using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    
    public AudioClip spell;
    AudioSource audioSource;
    // Json data reader
    string loadData;
    public Spells spellList;
    public Animator animator;
    public static Spell[] supportSpellsList;

    public Transform firePoint;
    public Transform healPoint;
    public Transform farPoint;

    public GameObject prefabFirebolt;
    public GameObject prefabHeal;
    public GameObject prefabBlast;
    public GameObject prefabIcespear;
    public GameObject prefabEarthbump;
    public GameObject prefabThunder;
    public GameObject prefabTornado;
    public GameObject prefabHolyBuff;
    
    public GameObject bookBox;
    public Shield _shield;
    public PlayerMovement _playerMovement;

    public HealthBar _healthBar;

    void Start() {
        //讀取指定路徑的Json檔案並轉成字串
        // loadData = File.ReadAllText("./Assets/Game_Demo/spells.json");
        string loadData = "{\"spells\":[{\"name\":\"firebolt\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"icespear\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"earthbump\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"thunder\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"speedup\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":10,\"enabled\":true},{\"name\":\"tornado\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"heal\",\"type\":\"active\",\"effect\":\"heal\",\"point\":40,\"cost\":20,\"enabled\":true},{\"name\":\"blast\",\"type\":\"active\",\"effect\":\"attack\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"shield\",\"type\":\"active\",\"effect\":\"buff\",\"point\":20,\"cost\":20,\"enabled\":true},{\"name\":\"extend\",\"type\":\"support\",\"effect\":\"buff\",\"point\":1,\"cost\":1,\"enabled\":true},{\"name\":\"multi\",\"type\":\"support\",\"effect\":\"buff\",\"point\":1,\"cost\":1,\"enabled\":true},{\"name\":\"snapshot\",\"type\":\"support\",\"effect\":\"buff\",\"point\":1,\"cost\":1,\"enabled\":true},{\"name\":\"multiple\",\"type\":\"support\",\"effect\":\"buff\",\"point\":1,\"cost\":1,\"enabled\":true},{\"name\":\"intensify\",\"type\":\"support\",\"effect\":\"buff\",\"point\":50,\"cost\":50,\"enabled\":true},{\"name\":\"super\",\"type\":\"support\",\"effect\":\"buff\",\"point\":2,\"cost\":3,\"enabled\":true}]}";

        //把字串轉換成Data物件
        spellList = JsonUtility.FromJson<Spells>(loadData);
        audioSource = GetComponent<AudioSource>();
    }

    public static Spell[] getSupportSpells() {
        return supportSpellsList;
    }

    public void Spell(string[] inputList) {
        var theInputSpellName = inputList[^1];
        Debug.Log($"theInputSpellName: {theInputSpellName}");

        if (theInputSpellName == String.Empty) {
            return;
        }

        Spell theSpell = Array.Find(
            spellList.spells,
            item => checkSpell(item, theInputSpellName, "active")
        );

        if (theSpell == null) {
            return;
        }

        int supportSpellCount = 0;

        List<Spell> supportSpells = new List<Spell> {};
        foreach (string spellName in inputList) {
            Spell theSupportSpell = Array.Find(
                spellList.spells,
                item => checkSpell(item, spellName, "support")
            );

            if (theSupportSpell == null || supportSpellCount >= 3) {
                continue;
            }

            supportSpellCount++;

            supportSpells.Add(theSupportSpell);
        }
        Debug.Log("1");
        StartCoroutine(SpellHandler(theSpell, supportSpells.ToArray()));
    }

    IEnumerator SpellHandler(Spell theSpell, Spell[] supportSpells) {
            supportSpellsList = supportSpells;

            // Reset dialogBox
            bookBox.SetActive(false);

            if (theSpell.effect == "buff") {
                _shield.CallShield(supportSpells);
                yield break;
            }

            if (theSpell.enabled == false) {
                _playerMovement.CallTeleport(supportSpells);
                yield break;
            }
            
            // 血魔，當下修改用
            var currentMp = _healthBar.GetCurrentMp();
            var currentHp = _healthBar.GetCurrentHp();

            // Handle status
            int nPoint = 1;
            int nCost = 1;
            
            // For Debug
            foreach(var item in supportSpells)
            {
                Console.WriteLine(item.name.ToString());
            }

            // Super Spell 處理
            var superSupport = Array.Find(
                supportSpells,
                item => checkSpell(item, "super", "support")
            );

            if (superSupport != null) {
                nPoint *= superSupport.point;
                nCost *= superSupport.cost;
            }

            // Multi Spell 處理
            var multiSupport = Array.Find(
                supportSpells,
                item => checkSpell(item, "multi", "support")
            );

            if (multiSupport != null) {
                nCost *= multiSupport.cost;
            }

            // multiple
            var multipleSupport = Array.Find(
                supportSpells,
                item => checkSpell(item, "multiple", "support")
            );

            if (multipleSupport != null) {
                nCost *= multipleSupport.cost;
            }
            
            // 耗魔許可檢查
            // if (theSpell.cost * nCost >= (currentMp - 3)) {
            //     Debug.Log("Spell Fail!");
            //     yield break;
            // }

            if (theSpell.effect == "heal") {
                _healthBar.SetValue(currentHp + (theSpell.point * nPoint), "hp");
            }
            
            // _healthBar.SetValue(currentMp - (theSpell.cost * nCost), "mp");

            // 動畫處理
            // 先預設火球
            GameObject thePrefab = prefabFirebolt;
            Transform theTransform = firePoint;

            if (theSpell.name == "firebolt") {
                thePrefab = prefabFirebolt;
            } else if (theSpell.name == "speedup") {
                thePrefab = prefabHolyBuff;
                theTransform.position = new Vector3(firePoint.position.x, firePoint.position.y - 0.2f, firePoint.position.z);
            } else if (theSpell.name == "heal") {
                thePrefab = prefabHeal;
                theTransform = healPoint;
            } else if (theSpell.name == "blast") {
                thePrefab = prefabBlast;
                theTransform = farPoint;
            } else if (theSpell.name == "icespear") {
                thePrefab = prefabIcespear;
            } else if (theSpell.name == "earthbump") {
                thePrefab = prefabEarthbump;
                theTransform.position = new Vector3(farPoint.position.x, farPoint.position.y, farPoint.position.z);
            } else if (theSpell.name == "thunder") {
                thePrefab = prefabThunder;
                theTransform.position = new Vector3(farPoint.position.x, farPoint.position.y + 0.3f, farPoint.position.z);
            } else if (theSpell.name == "tornado") {
                thePrefab = prefabTornado;
                theTransform.position = new Vector3(farPoint.position.x, farPoint.position.y + 1.2f, farPoint.position.z);
            }

            // 施放區
            int spellCount = 1;
            float waitTime = 0.6f;

            if (multiSupport != null) {
                spellCount = 3;
            }

            if (multipleSupport != null) {
                spellCount = 6;
                waitTime = 0.4f;
            }
            
            audioSource.PlayOneShot(spell, 0.7F);
            for (int i = 0; i < spellCount; i++) {
                GameObject newObject = Instantiate(thePrefab, theTransform.position, theTransform.rotation) as GameObject;

                animator.SetTrigger("Combat");
                
                // 有 Super 時會放大
                if (superSupport != null) {
                    newObject.transform.localScale += new Vector3(1,1,0);
                }
                yield return new WaitForSeconds(waitTime);
            }
    }

    // 確認技能類型
    public bool checkSpell(Spell item, string name, string type) {
        return item.name == name && item.type == type;
    }
}
