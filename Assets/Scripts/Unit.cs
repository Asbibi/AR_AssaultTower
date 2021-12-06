using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    UnitData data = null;

    public void SetData(UnitData unitData)
    {
        data = unitData;
    }

    // Update is called once per frame
    void Update()
    {
        if (data == null)
            return;
    }
}
