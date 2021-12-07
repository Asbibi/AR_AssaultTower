using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHero : MonoBehaviour
{
    [SerializeField] Text H;
    [SerializeField] Text D;
    [SerializeField] Text A;
    [SerializeField] Text R;


    public void UpdateUI(Hero hero)
    {
        UnitData dat = hero.GetData();
        H.text = hero.GetLife() + "/" + dat.maxLife;
        D.text = dat.defense + " + " + hero.GetDefenseBoost();
        A.text = dat.attack + " + " + hero.GetAttackBoost();
        R.text = dat.range.ToString();
    }
}
