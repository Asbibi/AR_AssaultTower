using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaker : MonoBehaviour
{
    public Unit[] units;
    public UnitData[] datas;


    void Start()
    {
        //GameManager.IncreaseMaxFloor();
        for (int i = 0; i < units.Length; i++)
        {
            units[i].gameObject.SetActive(true);
            units[i].Setup(datas[i]);
        }
    }

}
