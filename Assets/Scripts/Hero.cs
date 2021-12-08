using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    static public readonly int maxBoostPoint = 5;
    static public readonly float defenseMultiplier = 1;
    static public readonly float attackMultiplier = 1;

    int proximityBoostPoint = 0;    // from that, we'll get the defense and attack boosts
    
    [SerializeField] ProximityIndicator proxiIndicator;
    [SerializeField] UIHero UI;


    public override void Setup(UnitData unitData)
    {
        base.Setup(unitData);
        GameManager.RegisterHero(this);
    }



    public void ComputeProximityScore(List<Hero> allHeroes)
    {
        proximityBoostPoint = 0;
        foreach (var hero in allHeroes)
        {
            if (hero == this)
                continue;
            proximityBoostPoint +=  (int)GetProximity(hero);
        }
        if (proximityBoostPoint > maxBoostPoint)
            proximityBoostPoint = maxBoostPoint;
        proxiIndicator.SetColor(proximityBoostPoint);
        UI.UpdateUI(this);
    }



    public float GetDefenseBoost()
    {
        return proximityBoostPoint * defenseMultiplier;
    }
    public float GetAttackBoost()
    {
        return (maxBoostPoint - proximityBoostPoint) * attackMultiplier;
    }
    public override float GetDefense()
    {
        return base.GetDefense() + GetDefenseBoost();
    }
    public override float GetAttack()
    {
        return base.GetAttack() + GetAttackBoost();
    }
    public override bool Hurt(float attack)
    {
        bool res = base.Hurt(attack);
        if (res)
            UI.gameObject.SetActive(false);
        else
            UI.UpdateUI(this);
        return res;
    }


    public void DisplayUI(bool show)
    {
        UI.gameObject.SetActive(show);
        UI.UpdateUI(this);
    }

    public void Celebrate()
    {
        animator.SetTrigger("Victory");
    }
}
