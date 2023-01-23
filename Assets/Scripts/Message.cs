using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Message : MonoBehaviour
{
    private float appearDuration = 0.5f;
    private float displayDuration = 5f;
    private float displayTime = 0;
    private float destroyTime;

    private Image[] images;
    private TextMeshProUGUI text;

    public void Init(float displayDuration)
    {
        this.displayDuration = displayDuration;
        destroyTime = Time.time + displayDuration + 0.5f;
    }

    // Start is called before the first frame update
    void Awake()
    {
        images = GetComponentsInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > destroyTime)
            Destroy(this.gameObject);

        var opacity = 0f;

        if (displayTime < appearDuration)
        {
            opacity = Util.CubicEaseOut(displayTime, appearDuration);
        }
        else
        {
            opacity = 1 - Util.CubicEaseIn(displayTime, displayDuration - appearDuration);
        }


        foreach (var image in images)
        {
            image.color = new Color(1, 1, 1, opacity);
        }

        text.color = new Color(1, 1, 1, opacity);

        displayTime += Time.deltaTime;
    }
}
