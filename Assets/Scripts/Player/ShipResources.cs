using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipResources : MonoBehaviour
{
    [SerializeField]
    private Fuel fuel = new Fuel();
    [SerializeField]
    private Power power = new Power();

    public delegate void OnUpdateFuel(float amount, float max);
    public static OnUpdateFuel onUpdateFuel;

    public delegate void OnUpdatePower(float amount, float max);
    public static OnUpdatePower onUpdatePower;

    public delegate void OnGatherStart();
    public static OnGatherStart onGatherStart;


    public delegate void OnGatherStop();
    public static OnGatherStop onGatherStop;

    public float maxFuel = 400f;
    public float maxPower = 1000f;

    public float powerDecay = 0.02f;

    public float resourceDrainTickDelay = 0.1f;
    private float resourceDrainNextTick = Mathf.Infinity;

    public float resourceGatherTickDelay = 0.5f;
    public float resourcesGatheredPerTick = 10f;
    private float resourceGatherNextTick = Mathf.Infinity;

    public float gatherDistance = 10f;

    public void ConsumeFuel(float amount)
    {
        fuel.RemoveResource(amount);
        onUpdateFuel(fuel.amountHeld, maxFuel);
    }

    public void ConsumePower(float amount)
    {
        power.RemoveResource(amount);
        onUpdatePower(power.amountHeld, maxPower);
    }

    public float FuelAmount()
    {
        return fuel.amountHeld;
    }

    public float PowerAmount()
    {
        return power.amountHeld;
    }

    public void Init()
    {
        fuel.resourceName = "Fuel";
        fuel.amountHeld = maxFuel * 0.5f;
        fuel.maxHoldable = maxFuel;

        power.resourceName = "Power";
        power.amountHeld = maxPower * 0.8f;
        power.maxHoldable = maxPower;

        onUpdateFuel(fuel.amountHeld, maxFuel);
        onUpdatePower(power.amountHeld, maxPower);

        resourceDrainNextTick = Time.time + resourceDrainTickDelay;
        resourceGatherNextTick = Time.time + resourceGatherTickDelay;
    }

    void Update()
    {
        if (!GameManager.instance.IsPlaying())
            return;

        onUpdatePower(power.amountHeld, maxPower);
        onUpdateFuel(fuel.amountHeld, maxFuel);

        if (power.amountHeld <= 0)
        {
            GameManager.instance.GameOver("The lights went out... and very slowly the unverse came to an end.");
        }

        if (fuel.amountHeld <= 0)
        {
            GameManager.instance.GameOver("With no way to change course the ship would drift endlessly on.\nAn observer only.");
        }

        if (Time.time >= resourceDrainNextTick)
        {
            power.RemoveResource(powerDecay);
            resourceDrainNextTick = Time.time + resourceDrainTickDelay;
        }

        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        int nearestBodyIndex = -1;
        float distanceToNearestBody = Mathf.Infinity;

        for (int i = 0; i < bodies.Length; i++)
        {
            var dst = Vector3.Distance(transform.position, bodies[i].transform.position);
            if (dst < distanceToNearestBody)
            {
                distanceToNearestBody = dst;
                nearestBodyIndex = i;
            }
        }

        if (nearestBodyIndex == -1)
            return;

        CelestialBody nearestBody = bodies[nearestBodyIndex];

        // if (distanceToNearestBody > gatherDistance + nearestBody.diameter)
        if (distanceToNearestBody > nearestBody.GetGatherDistance())
        {
            onGatherStop();
            return;
        }

        if (Time.time >= resourceGatherNextTick)
        {
            if (nearestBody.GetGatherAmount() <= 0)
            {
                onGatherStop();
                return;
            }

            onGatherStart();

            if (nearestBody.GetResourceType().Equals(typeof(Fuel)))
            {
                if (fuel.amountHeld > fuel.maxHoldable - resourcesGatheredPerTick)
                    return;

                var gathered = nearestBody.Gather(resourcesGatheredPerTick);
                fuel.AddResource(gathered);
                onUpdateFuel(fuel.amountHeld, maxFuel);
            }

            if (nearestBody.GetResourceType().Equals(typeof(Power)))
            {
                if (power.amountHeld > power.maxHoldable - resourcesGatheredPerTick)
                    return;

                var gathered = nearestBody.Gather(resourcesGatheredPerTick);
                power.AddResource(gathered);
                onUpdatePower(power.amountHeld, maxPower);
            }

            resourceGatherNextTick = Time.time + resourceGatherTickDelay;
        }
    }
}
