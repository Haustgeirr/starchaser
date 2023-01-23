using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public const int SECTORS = 5;
    public int seed = 0;

    public GameObject planetPrefab;
    public GameObject targetPrefab;
    public float basePlanetDiameter = 0.127f; // 12,742 km diameter of Earth
    public float displayRadius = 1;

    private List<Vector3> points;
    private Vector3[] targetPoints = new Vector3[SECTORS];

    public Vector3 GetNextTargetPoint(int index)
    {
        return targetPoints[index];
    }

    public Vector3[] GetTargetPoints()
    {
        return targetPoints;
    }

    public void StartGeneration(int seed)
    {
        System.Random pseudoRandom = new System.Random(seed);
        GenerateUniverse(ref pseudoRandom);
        GeneratePlanets(ref pseudoRandom);
        GenerateTargets(ref pseudoRandom);
    }

    Resource RollResource(int roll)
    {
        Resource chosenResource = new Fuel();

        if (roll < 20)
        {
            chosenResource = new Power();
        }

        return chosenResource;
    }

    void SelectSectorTarget(ref List<Vector3> sectorPoints, int index)
    {
        int targetIndex = -1;
        var maxDistance = Mathf.NegativeInfinity;

        for (int i = 0; i < sectorPoints.Count; i++)
        {
            var dst = sectorPoints[i].sqrMagnitude;
            if (dst > maxDistance)
            {
                targetIndex = i;
                maxDistance = dst;
            }
        }

        if (targetIndex > -1)
        {
            targetPoints[index] = sectorPoints[targetIndex];
            sectorPoints.RemoveAt(targetIndex);
        }
    }

    void GenerateUniverse(ref System.Random pseudoRandom)
    {
        points = new List<Vector3>();

        List<Vector3> sectorOnePoints = UniverseGenerator.GenerateSector(360, 1500, ref pseudoRandom, 100, 4);
        List<Vector3> sectorTwoPoints = UniverseGenerator.GenerateSector(480, 2500, ref pseudoRandom, 1750, 3);
        List<Vector3> sectorThreePoints = UniverseGenerator.GenerateSector(720, 5000, ref pseudoRandom, 2550, 2);
        List<Vector3> sectorFourPoints = UniverseGenerator.GenerateSector(960, 9000, ref pseudoRandom, 5050, 2);
        List<Vector3> sectorFivePoints = UniverseGenerator.GenerateSector(1800, 18000, ref pseudoRandom, 9050, 2);

        SelectSectorTarget(ref sectorOnePoints, 0);
        SelectSectorTarget(ref sectorTwoPoints, 1);
        SelectSectorTarget(ref sectorThreePoints, 2);
        SelectSectorTarget(ref sectorFourPoints, 3);
        SelectSectorTarget(ref sectorFivePoints, 4);

        points.AddRange(sectorOnePoints);
        points.AddRange(sectorTwoPoints);
        points.AddRange(sectorThreePoints);
        points.AddRange(sectorFourPoints);
        points.AddRange(sectorFivePoints);
    }

    void GenerateTargets(ref System.Random pseudoRandom)
    {
        var parent = new GameObject("Targets");
        foreach (var point in targetPoints)
        {
            var obj = GameObject.Instantiate(targetPrefab, point, Quaternion.Euler(0, pseudoRandom.Next(-90, 90), 0), parent.transform);
            obj.transform.localScale = Vector3.one * pseudoRandom.Next(50, 100);
        }
    }

    void GeneratePlanets(ref System.Random pseudoRandom)
    {
        var parent = new GameObject("Bodies");
        foreach (var point in points)
        {
            var obj = GameObject.Instantiate(planetPrefab, point, Quaternion.identity, parent.transform);
            var body = obj.GetComponent<CelestialBody>();

            var planetRoll = pseudoRandom.Next(1, 100);

            var selectedResource = RollResource(pseudoRandom.Next(1, 100));

            if (planetRoll <= 5)
            {
                body.Init(pseudoRandom.Next(50, 70) * basePlanetDiameter, selectedResource, pseudoRandom.Next(20, 80));
            }
            else if (planetRoll > 5 && planetRoll <= 15)
            {
                body.Init(pseudoRandom.Next(70, 100) * basePlanetDiameter, selectedResource, pseudoRandom.Next(60, 120));
            }
            else if (planetRoll > 15 && planetRoll <= 30)
            {
                body.Init(pseudoRandom.Next(100, 140) * basePlanetDiameter, selectedResource, pseudoRandom.Next(80, 160));
            }
            else if (planetRoll > 30 && planetRoll <= 80)
            {
                body.Init(pseudoRandom.Next(140, 280) * basePlanetDiameter, selectedResource, pseudoRandom.Next(120, 320));
            }
            else if (planetRoll > 80 && planetRoll <= 94)
            {
                body.Init(pseudoRandom.Next(280, 400) * basePlanetDiameter, selectedResource, pseudoRandom.Next(120, 320));
            }
            else if (planetRoll > 94 && planetRoll <= 99)
            {
                body.Init(pseudoRandom.Next(400, 570) * basePlanetDiameter, selectedResource, pseudoRandom.Next(160, 480));
            }
            else if (planetRoll > 99)
            {
                body.Init(pseudoRandom.Next(570, 800) * basePlanetDiameter, selectedResource, pseudoRandom.Next(240, 800));
            }
        }
    }

    void OnValidate()
    {
        System.Random pseudoRandom = new System.Random(seed);
        GenerateUniverse(ref pseudoRandom);
    }

    void OnDrawGizmos()
    {
        if (points != null)
        {
            foreach (Vector3 point in points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point, 100);
            }
        }

        if (targetPoints != null)
        {
            foreach (Vector3 point in targetPoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(point, 200);
            }
        }
    }
}
