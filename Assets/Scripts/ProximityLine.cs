using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProximityLine : MonoBehaviour
{
    static List<ProximityLine> allLines = new List<ProximityLine>();

    private LineRenderer LRenderer;
    [SerializeField] private Transform heroStart = null;
    [SerializeField] private Transform heroEnd = null;

    private void Update()
    {
        Vector3 Direction = (heroEnd.position - heroStart.position).normalized * 0.03f;
        LRenderer.SetPosition(0, heroStart.position + Direction);
        LRenderer.SetPosition(1, heroEnd.position - Direction);
    }
    private bool IsConnection(Hero hero1, Hero hero2)
    {
        return (hero1.gameObject == heroStart.gameObject && hero2.gameObject == heroEnd.gameObject) || (hero2.gameObject == heroStart.gameObject && hero1.gameObject == heroEnd.gameObject);
    }
    private void Setup(Hero hero1, Hero hero2)
    {
        LRenderer = GetComponent<LineRenderer>();
        heroStart = hero1.transform;
        heroEnd = hero2.transform;
    }
    private void SetProximity(Proximity proxi)
    {
        if (proxi == Proximity.Close)
        {
            LRenderer.startWidth = 0.007f;
            LRenderer.endWidth = 0.007f;
        }
        else
        {
            LRenderer.startWidth = 0.002f;
            LRenderer.endWidth = 0.002f;
        }
    }


    public static void Connect(Hero hero1, Hero hero2, Proximity proxi)
    {
        ProximityLine line = GetLine(hero1, hero2);
        if (proxi == Proximity.Far)
        {
            if (line != null && line.gameObject.activeSelf)
                line.gameObject.SetActive(false);
        }
        else
        {
            if (line == null)
            {
                line = Instantiate(GameManager.GetProximityLinePrefab(), Vector3.zero, Quaternion.identity).GetComponent<ProximityLine>();
                line.Setup(hero1, hero2);
                allLines.Add(line);
            }
            else if (!line.gameObject.activeSelf)
                line.gameObject.SetActive(true);

            line.SetProximity(proxi);
            line.Update();
        }
    }
    private static ProximityLine GetLine(Hero hero1, Hero hero2)
    {
        foreach (var line in allLines)
            if (line.IsConnection(hero1, hero2))
                return line;
        return null;
    }

    public static void Disconnect(Hero hero, List<Hero> others) // to use when the hero dies
    {
        foreach (var h in others)
        {
            ProximityLine line = GetLine(hero, h);
            if (line != null)
            {
                allLines.Remove(line);
                Destroy(line.gameObject);
            }
        }
    }
}
