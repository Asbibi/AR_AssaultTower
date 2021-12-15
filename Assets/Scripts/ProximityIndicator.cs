using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ProximityIndicator : MonoBehaviour
{
    static Color[] proximityColors = { Color.red,       // min point count
        new Color(1, 0.167f, 0, 1),
        new Color(1, 0.677f, 0, 1),
        new Color(0.52f, 1, 0, 1),
        Color.green,
        new Color(0, 1, 0.6f, 1)                        // max point count
    };
    // make sure its lenght is equal to hero.maxBoostPoint + 1 (=6)

    MeshRenderer meshRenderer;

    [Header("To Specific Hero")]
    [SerializeField] GameObject lineHoster;
    List<Hero> linkedHero = new List<Hero>();
    List<LineRenderer> lines = new List<LineRenderer>();

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    
    public void SetColor(int bonusPointCount)
    {
        Color interpColor = proximityColors[bonusPointCount];
        meshRenderer.material.SetColor("_EmissionColor", interpColor);
    }
}
