using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniverseGenerator
{
    public static List<Vector3> GenerateSector(float planetRadius, float sectorRadius, ref System.Random pseudoRandom, float innerRadius = 0, int stability = 4)
    {
        // TODO generate points until minimum points met
        List<Vector2> startingPoints = new List<Vector2>();

        int tries = 0;

        while (startingPoints.Count < 20)
        {
            if (tries == 100)
            {
                Debug.Log("Failed after 100 tries.");
                break;
            }

            startingPoints = GeneratePoints(planetRadius, new Vector2(sectorRadius * 2, sectorRadius * 2), ref pseudoRandom, stability);
            tries++;

        }

        List<Vector3> finalPositions = new List<Vector3>();
        foreach (var point in startingPoints)
        {
            Vector2 testPoint = new Vector2(point.x - sectorRadius, point.y - sectorRadius);

            if (testPoint.sqrMagnitude <= innerRadius * innerRadius)
                continue;

            finalPositions.Add(new Vector3(testPoint.x, 0, testPoint.y));
        }

        return finalPositions;
    }

    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, ref System.Random pseudoRandom, int samplesBeforeRejection = 16)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];

        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        // TODO choose a random point 
        spawnPoints.Add(new Vector2(pseudoRandom.Next(0, (int)sampleRegionSize.x), pseudoRandom.Next(0, (int)sampleRegionSize.y)));

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = pseudoRandom.Next(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];

            bool candidateAccepted = false;

            for (int i = 0; i < samplesBeforeRejection; i++)
            {
                // PRD Here
                float angle = pseudoRandom.Next() * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCentre + dir * pseudoRandom.Next((int)radius, (int)radius * 2);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
                spawnPoints.RemoveAt(spawnIndex);
        }

        // Debug.Log("Generated " + points.Count + " points.");
        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            // TODO change to generate only within radius
            if (Vector2.Distance(candidate, sampleRegionSize / 2) > sampleRegionSize.x / 2)
                return false;

            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius)
                            return false;
                    }
                }
            }

            return true;
        }

        return false;
    }
}