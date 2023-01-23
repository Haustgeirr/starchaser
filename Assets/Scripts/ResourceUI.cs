using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI powerText;

    public float resourceWarningLevel = 0.5f;
    public float resourceDangerLevel = 0.2f;

    public Color safeLevelColour = new Color(1, 1, 1, 1);
    public Color dangerLevelColour = new Color(1, 0, 0, 1);

    string MakeResourceString(float amount, float max)
    {
        return (Mathf.Round((amount / max) * 10000) / 100f).ToString() + "%";
    }

    private void UpdateFuelUI(float amount, float max)
    {
        fuelText.text = MakeResourceString(amount, max);
        if (amount / max <= 0.5f)
        {
            fuelText.color = Vector4.Lerp(safeLevelColour, dangerLevelColour, Mathf.InverseLerp(0.5f, 0.2f, amount / max));
        }
        else
        {
            fuelText.color = safeLevelColour;
        }
    }

    private void UpdatePowerUI(float amount, float max)
    {
        powerText.text = MakeResourceString(amount, max);

        if (amount / max <= 0.5f)
        {
            powerText.color = Vector4.Lerp(safeLevelColour, dangerLevelColour, Mathf.InverseLerp(0.5f, 0.2f, amount / max));
        }
        else
        {
            powerText.color = safeLevelColour;
        }
    }

    private void OnEnable()
    {
        ShipResources.onUpdateFuel += UpdateFuelUI;
        ShipResources.onUpdatePower += UpdatePowerUI;
    }

    private void OnDisable()
    {
        ShipResources.onUpdateFuel -= UpdateFuelUI;
        ShipResources.onUpdatePower -= UpdatePowerUI;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
