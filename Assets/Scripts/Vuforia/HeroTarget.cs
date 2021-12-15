using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTarget : MonoBehaviour
{
    [SerializeField] Hero hero;
    [SerializeField] UnitData heroData;

    public void OnFound()
    {
        hero.Setup(heroData);
        GameManager.RegisterHero(hero);
    }
    public void OnLost()
    {
        Debug.Log("Lost");
    }
}
