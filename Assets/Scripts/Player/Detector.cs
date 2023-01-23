using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public const int NUM_TARGETS = 8;

    public delegate void OnUpdateBodies(Vector3[] directions, float[] distances, int[] types);
    public static OnUpdateBodies onUpdateBodiesEvent;

    private CelestialBody[] allBodies;
    private CelestialBody[] bodies = new CelestialBody[NUM_TARGETS];

    public float detectBodiesTickDelay = 0.1f;
    private float detectBodiesNextTick = Mathf.Infinity;

    public CelestialBody GetNearestBody()
    {
        return bodies[0];
    }

    public CelestialBody[] GetBodies()
    {
        return bodies;
    }

    void UpdateUI()
    {
        if (bodies.Length == 0)
            return;

        Vector3[] directions = new Vector3[NUM_TARGETS];
        float[] distances = new float[NUM_TARGETS];
        int[] types = new int[NUM_TARGETS];

        for (int i = 0; i < NUM_TARGETS; i++)
        {
            directions[i] = bodies[i].transform.position - transform.position;
            distances[i] = Vector3.Distance(transform.position, bodies[i].transform.position);
            types[i] = bodies[i].resource.resourceName == "Power" ? 0 : 1;
        }

        onUpdateBodiesEvent(directions, distances, types);
    }

    void DetectBodies()
    {
        if (allBodies.Length == 0)
            allBodies = FindObjectsOfType<CelestialBody>();

        List<CelestialBody> bodiesWithResources = new List<CelestialBody>();
        var orderedBodies = allBodies.OrderBy(b => (b.transform.position - transform.position).sqrMagnitude).ToArray();

        for (int i = 0; i < orderedBodies.Length; i++)
        {
            if (bodiesWithResources.Count == NUM_TARGETS)
                break;

            if (orderedBodies[i].resource.amountHeld == 0)
                continue;
            bodiesWithResources.Add(orderedBodies[i]);

        }

        bodies = bodiesWithResources.ToArray();
    }

    public void Init()
    {
        bodies = new CelestialBody[NUM_TARGETS];
        allBodies = new CelestialBody[0];
        allBodies = FindObjectsOfType<CelestialBody>();
        DetectBodies();
        detectBodiesNextTick = Time.time + detectBodiesTickDelay;
    }

    void Update()
    {
        if (!GameManager.instance.IsPlaying())
            return;

        UpdateUI();

        if (Time.time < detectBodiesNextTick)
            return;

        DetectBodies();
        detectBodiesNextTick = Time.time + detectBodiesTickDelay;
    }
}