using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHero : MonoBehaviour
{
    [SerializeField] Transform AR_Camera;
    [SerializeField] Text H;
    [SerializeField] Text D;
    [SerializeField] Text A;
    [SerializeField] Text R;


    private void Update()
    {
        transform.LookAt(AR_Camera);
        transform.rotation = Quaternion.Euler(0, transform.parent.rotation.y, 0);
    }

    public void UpdateUI(Hero hero)
    {
        UnitData dat = hero.GetData();
        H.text = hero.GetLife() + "/" + dat.maxLife;
        D.text = dat.defense + " + " + hero.GetDefenseBoost();
        A.text = dat.attack + " + " + hero.GetAttackBoost();
        R.text = dat.range.ToString();
    }
}
