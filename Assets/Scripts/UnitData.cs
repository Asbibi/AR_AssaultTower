using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_Data_")]
public class UnitData : ScriptableObject
{ 
    [Header("Identity")]
    public string unitName;
    public Mesh mesh;
    public AnimationCategory category;
    public Color color;
    [Header("Stats")]
    public int maxLife = 25;
    public int attack = 10;
    public int defense = 10;
    public Proximity range = Proximity.Close;
    public RangeAngle angle = RangeAngle.forward;   // not used yet
    public bool hitAllies = false;                  // not used yet
}
