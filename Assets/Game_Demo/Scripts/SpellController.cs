using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpellController : MonoBehaviour
{
    // Json data reader
    string loadData;
    Spells spellList;

    public Transform firePoint;
    public Transform healPoint;
    public GameObject prefabFirebolt;
    public GameObject prefabHeal;
    public GameObject prefabBlast;

    public HealthBar _healthBar;

    void Start() {
        //讀取指定路徑的Json檔案並轉成字串
        loadData = File.ReadAllText("./Assets/Game_Demo/spells.json");
        
        //把字串轉換成Data物件
        spellList = JsonUtility.FromJson<Spells>(loadData);
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

        StartCoroutine(SpellHandler(theSpell, supportSpells.ToArray()));
    }

    IEnumerator SpellHandler(Spell theSpell, Spell[] supportSpells) {
            // 血魔，當下修改用
            var currentMp = _healthBar.GetCurrentMp();
            var currentHp = _healthBar.GetCurrentHp();

            // Handle status
            int nPoint = 1;
            int nCost = 1;
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
            var multipleSupport = Array.Find(
                supportSpells,
                item => checkSpell(item, "multi", "support")
            );

            if (multipleSupport != null) {
                nCost *= multipleSupport.cost;
            }

            
            // 耗魔許可檢查
            if (theSpell.cost * nCost >= (currentMp - 3)) {
                Debug.Log("Spell Fail!");
                yield return new WaitForSeconds(0.2f);
            }

            if (theSpell.effect == "heal") {
                _healthBar.SetValue(currentHp + (theSpell.point * nPoint), "hp");
            }
            
            _healthBar.SetValue(currentMp - (theSpell.cost * nCost), "mp");

            // 動畫處理
            // 先預設火球
            GameObject thePrefab = prefabFirebolt;
            Transform theTransform = firePoint;

            if (theSpell.name == "firebolt") {
                thePrefab = prefabFirebolt;
                theTransform = firePoint;
            } else if (theSpell.name == "heal") {
                thePrefab = prefabHeal;
                theTransform = healPoint;
            } else if (theSpell.name == "blast") {
                thePrefab = prefabBlast;
            }

            // 施放區
            int spellCount = 1;
            if (multipleSupport != null) {
                spellCount = 3;
            }
            
            float waitTime = 0.3f;
            for (int i = 0; i < spellCount; i++) {
                GameObject newObject = Instantiate(thePrefab, theTransform.position, theTransform.rotation) as GameObject;

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
