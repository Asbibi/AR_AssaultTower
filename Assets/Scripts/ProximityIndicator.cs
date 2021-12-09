using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProximityIndicator : MonoBehaviour
{
    [Header("Global")]
    MeshRenderer renderer;
    [SerializeField] Color colorMin = Color.red;
    [SerializeField] Color colorMax = Color.green;

    [Header("To Specific Hero")]
    [SerializeField] GameObject lineHoster;
    List<Hero> linkedHero = new List<Hero>();
    List<LineRenderer> lines = new List<LineRenderer>();

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    
    public void SetColor(int bonusPointCount)
    {
        float t = ((float)bonusPointCount) / ((float)Hero.maxBoostPoint);
        Color interpColor = Color.Lerp(colorMin, colorMax, t);
        renderer.material.SetColor("_EmissionColor", interpColor);
    }
}
