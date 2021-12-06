using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data_")]
public class UnitData : ScriptableObject
{
    public enum RangeAngle
    {
        forward,
        d45,
        d90,
        d180
    };

    public int maxLife = 25;
    public int attack = 10;
    public int defense = 10;
    public int range = 10;
    public RangeAngle angle = RangeAngle.forward;
    public bool hitAllies = false;
    public Mesh mesh;
}
