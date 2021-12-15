using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Unit : MonoBehaviour
{
    static float proximityCloseLimit = 0.085f;
    static float proximityNearLimit = 0.15f;

    UnitData data = null;
    [SerializeField] SkinnedMeshRenderer mesh;
    protected Animator animator;

    float life;
    bool alreadySetUp = false;


    public virtual void Setup(UnitData unitData)
    {
        if (alreadySetUp)
            return;

        data = unitData;
        life = unitData.maxLife;
        mesh.sharedMesh = unitData.mesh;
        animator = GetComponent<Animator>();
        animator.SetInteger("Category", (int)unitData.category);
        alreadySetUp = true;
    }

    public void Active()
    {
        gameObject.SetActive(true);
        animator.SetInteger("Category", (int)data.category);
    }

    // Update is called once per frame
    void Update()
    {
        if (data == null)
            return;
    }

    public UnitData GetData()
    {
        return data;
    }   
    public virtual float GetDefense()
    {
        return data.defense;
    }
    public virtual float GetAttack()
    {
        return data.attack;
    }
    public float GetLife()
    {
        return life;
    }


    public float Attack()
    {
        animator.SetTrigger("Attack");
        return GetAttack();
    }
    public virtual bool Hurt(float attack)  // returns if the unit died from the attack (true) or not (false)
    {
        float damage = attack - GetDefense() * 0.5f;
        if (damage < 0)
            damage = 1; // minimum damage is 1

        life -= damage;
        bool die = life < 0;
        animator.SetTrigger(die ? "Death" : "Hit");
        return die;
    }
    public Proximity GetProximity(Unit other)
    {
        float dist = Vector3.Distance(transform.position, other.transform.position);
        if (dist < proximityCloseLimit)
            return Proximity.Close;
        else if(dist < proximityNearLimit)
            return Proximity.Near;
        else
            return Proximity.Far;
    }
    public bool IsInRange(Unit other)
    {
        Proximity proxi = GetProximity(other);
        return (int)proxi >= (int)data.range;
    }

    public int GetClosestUnit(Unit[] others)
    {
        if (others.Length == 0)
            return -1;

        int closestUnit = 0;
        for (int i = 1; i < others.Length; i++)
        {
            if (Vector3.Distance(others[i].transform.position, transform.position) < Vector3.Distance(others[closestUnit].transform.position, transform.position))
                closestUnit = i;
        }

        return closestUnit;
    }



    static public float getProximityDistance(Proximity proxi)
    {
        switch(proxi)
        {
            case Proximity.Close:
                return proximityCloseLimit;
            case Proximity.Near:
                return proximityNearLimit;
            default:
                return -1;
        }
    }
}
