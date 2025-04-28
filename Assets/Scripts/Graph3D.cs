using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class Graph3D : MonoBehaviour
{
    [Header("Fond de graphe")]
    public GameObject backgroundQuad;

    [Header("Affichage du prix actuel")]
    public TextMeshProUGUI prixText;
    public TextMeshProUGUI minValeurText;
    public TextMeshProUGUI maxValeurText;

    [Header("Réglages de la courbe")]
    public int pointCount = 100;
    public float logicalMinY = 0f;
    public float logicalMaxY = 10f;

    [Header("Réglages dynamiques")]
    [Range(0.001f, 0.2f)] public float scrollSpeed = 0.02f;         // Vitesse de défilement (points/sec)
    [Range(0.1f, 10f)] public float amplitudeMultiplier = 1f;       // Intensité des variations

    private LineRenderer lineRenderer;
    private List<float> values = new List<float>();
    private float graphWidth;
    private float graphHeight;
    private float xSpacing;

    // Contrôle du temps pour le défilement fluide
    private float scrollTimer = 0f;
    private float scrollThreshold = 0.05f; // Durée "de base" avant ajout d’un point
    public float prixActuel { get; private set; }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupRenderer();
        SetupGraphDimensions();

        // Valeur initiale fixe pour toute la courbe (plate au début)
        float initialValue = Random.Range(logicalMinY, logicalMaxY);
        for (int i = 0; i < pointCount; i++)
        {
            values.Add(initialValue);
        }

        if (minValeurText != null)
            minValeurText.text = $"${logicalMinY:F2}";
        if (maxValeurText != null)
            maxValeurText.text = $"${logicalMaxY:F2}";
        if (prixText != null)
            prixText.text = $"${initialValue:F2}";

        lineRenderer.positionCount = pointCount;
        UpdateLinePositions();
    }

    void Update()
    {
        
        if (prixText == null)
        {
            Debug.LogWarning("[DEBUG] prixText est NULL dans Graph3D");
        }
        else if (prixText.IsDestroyed())
        {
            Debug.LogWarning("[DEBUG] prixText est DETRUIT dans Graph3D");
        }

        scrollTimer += Time.deltaTime;

        if (scrollTimer >= scrollThreshold / scrollSpeed)
        {
            scrollTimer = 0f;

            for (int i = 0; i < values.Count - 1; i++)
            {
                values[i] = values[i + 1];
            }

            float lastValue = values[values.Count - 2];
            float direction = Random.Range(-0.5f, 0.5f);
            float noise = Random.Range(-1f, 1f) * 0.3f;
            float newY = lastValue + (direction + noise) * amplitudeMultiplier;
            newY = Mathf.Clamp(newY, logicalMinY, logicalMaxY);

            if (prixText != null && !prixText.IsDestroyed())
            {
                prixText.text = $"${newY:F2}";
            }
            prixActuel = newY;
            values[values.Count - 1] = newY;

            UpdateLinePositions();
        }
    }

    void SetupRenderer()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
    }

    void SetupGraphDimensions()
    {
        if (backgroundQuad != null)
        {
            Vector3 scale = backgroundQuad.transform.localScale;
            graphWidth = scale.x * 10f;   // Quad : 1 unité scale = 10 unités monde
            graphHeight = scale.y * 10f;
            xSpacing = graphWidth / (pointCount - 1);
        }
        else
        {
            Debug.LogError("Le Quad de fond n'est pas assigné !");
        }
    }

    void UpdateLinePositions()
    {
        Vector3[] positions = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float x = -graphWidth / 2f + i * xSpacing;
            float normalizedY = Mathf.InverseLerp(logicalMinY, logicalMaxY, values[i]);
            float y = -graphHeight / 2f + normalizedY * graphHeight;

            positions[i] = new Vector3(x, y, 0f);
        }

        lineRenderer.SetPositions(positions);
    }
}
