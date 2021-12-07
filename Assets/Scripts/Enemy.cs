using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public override void Setup(UnitData unitData)
    {
        base.Setup(unitData);
        GameManager.RegisterEnemy(this);
    }
}
