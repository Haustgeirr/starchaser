using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectorUI : MonoBehaviour
{
    const int NUM_POINTS = 180;
    [Header("Prefabs")]
    public GameObject detectorBarPrefab;
    public GameObject iconPrefab;
    public Sprite fuelIcon;
    public Sprite powerIcon;
    public Sprite targetIcon;

    [Header("Settings")]
    public float radius = 20f;
    public float iconRadius = 22f;
    public float maxMagnitudeScale = 20f;
    public float maxMagnitudeDistance = 100f;
    public float minMagnitudeDistance = 1000f;

    private Vector3[] linePoints = new Vector3[NUM_POINTS];
    private GameObject[] detectorBars = new GameObject[NUM_POINTS];
    private SpriteRenderer[] icons = new SpriteRenderer[Detector.NUM_TARGETS + 1];

    void ScaleBar(int index, float scale)
    {
        var currentScale = detectorBars[index].transform.localScale.z;
        if (currentScale > scale)
            return;

        detectorBars[index].transform.localScale = new Vector3(1, 1, scale);
        detectorBars[index].transform.localPosition = linePoints[index].normalized * (radius - ((scale - 1) * 0.2f) / 2);
    }

    void ResetBarScales()
    {
        for (int i = 0; i < NUM_POINTS; i++)
        {
            detectorBars[i].transform.localScale = Vector3.one;
            detectorBars[i].transform.localPosition = linePoints[i];
        }
    }

    float GetDirectionAngle(Vector3 direction)
    {
        var angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
        return angle > 0 ? 360 - angle : Mathf.Abs(angle);
    }

    void UpdateIndicator(Vector3 direction, float distance, int type, int index)
    {
        var angle = GetDirectionAngle(direction);

        var barIndex = Mathf.CeilToInt(angle / 2f);
        var inverse = Mathf.InverseLerp(minMagnitudeDistance, maxMagnitudeDistance, distance);
        var scale = Mathf.Lerp(1f, maxMagnitudeScale, inverse);

        UpdateIcon(angle, type, inverse, index);

        ScaleBar(Util.mod(barIndex - 3, NUM_POINTS), 1 + (scale * 0.1f));
        ScaleBar(Util.mod(barIndex - 2, NUM_POINTS), 1 + (scale * 0.2f));
        ScaleBar(Util.mod(barIndex - 1, NUM_POINTS), 1 + (scale * 0.5f));
        ScaleBar(Util.mod(barIndex, NUM_POINTS), 1 + scale);
        ScaleBar(Util.mod(barIndex + 1, NUM_POINTS), 1 + (scale * 0.5f));
        ScaleBar(Util.mod(barIndex + 2, NUM_POINTS), 1 + (scale * 0.2f));
        ScaleBar(Util.mod(barIndex + 3, NUM_POINTS), 1 + (scale * 0.1f));
    }

    void InitIcons()
    {
        for (int i = 0; i < Detector.NUM_TARGETS + 1; i++)
        {
            var obj = GameObject.Instantiate(iconPrefab, Vector3.zero, Quaternion.identity, this.transform);
            icons[i] = obj.GetComponentInChildren<SpriteRenderer>();
            icons[i].color = new Color(1, 1, 1, 0);
        }
    }

    void UpdateTargetIcon()
    {
        var targetPos = GameManager.instance.GetNextTargetPoint();
        var playerPos = GameManager.instance.GetPlayerTransform().position;

        var direction = targetPos - playerPos;
        UpdateIcon(GetDirectionAngle(direction), 2, 1, 8);
    }

    void UpdateIcon(float angle, int type, float opacity, int index)
    {
        var point = Quaternion.Euler(0, angle, 0) * Vector3.forward * iconRadius;
        var sprite = type == 0 ? powerIcon : type == 1 ? fuelIcon : targetIcon;

        icons[index].sprite = sprite;
        icons[index].color = new Color(1, 1, 1, opacity);
        icons[index].transform.localPosition = point + Vector3.up * 200f;
    }

    void InitDefaultLine()
    {
        Vector3 up = Vector3.up * radius;
        Vector3 right = Vector3.Cross(up, Vector3.forward);
        Vector3 forward = Vector3.forward * radius;

        for (int i = 0; i < 46; i++)
        {
            linePoints[i] = Vector3.Slerp(forward, right, i / 45.0f);
            linePoints[i + 45] = Vector3.Slerp(right, -forward, i / 45.0f);
            linePoints[i + 90] = Vector3.Slerp(-forward, -right, i / 45.0f);

            if (i == 45)
                continue;

            linePoints[i + 135] = Vector3.Slerp(-right, forward, i / 45.0f);
        }
    }

    private void OnEnable()
    {
        Detector.onUpdateBodiesEvent += UpdateUI;
    }

    private void OnDisable()
    {
        Detector.onUpdateBodiesEvent -= UpdateUI;
    }

    void Start()
    {
        InitDefaultLine();
        InitIcons();

        var parent = new GameObject("Detector UI");
        parent.transform.position += Vector3.up * 200f;
        parent.transform.SetParent(this.transform);
        this.transform.localScale = new Vector3(1.5f, 1, 1.5f);


        for (int i = 0; i < NUM_POINTS; i++)
        {
            var obj = GameObject.Instantiate(detectorBarPrefab, linePoints[i], Quaternion.Euler(0, i * 2, 0), parent.transform);
            obj.transform.name = ("Bar " + i);
            detectorBars[i] = obj;
        }
    }


    void UpdateUI(Vector3[] directions, float[] distances, int[] types)
    {
        ResetBarScales();

        for (int i = 0; i < Detector.NUM_TARGETS; i++)
        {
            UpdateIndicator(directions[i], distances[i], types[i], i);
        }

        UpdateTargetIcon();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
