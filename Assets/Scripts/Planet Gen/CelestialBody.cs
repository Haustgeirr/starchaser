using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour, IGatherable
{
    public Resource resource;
    public Transform resourceRange;
    public float diameter;

    public float gatherDistance;

    void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeFatalDamage("Stars are hot. And surprisingly hard.");
    }

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeDamage();
    }

    void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.StopTakeDamage();
    }

    public System.Type GetResourceType()
    {
        return resource.GetType();
    }

    public float GetGatherAmount()
    {
        return resource.amountHeld;
    }

    public float GetGatherDistance()
    {
        return gatherDistance;
    }

    public float Gather(float amount)
    {
        if (resource.amountHeld < amount)
        {
            amount = resource.amountHeld;
        }

        resource.RemoveResource(amount);
        return amount;
    }

    public void Init(float diameter, Resource resource, float resourceAmount)
    {
        this.diameter = diameter;

        transform.localScale = Vector3.one * this.diameter;
        this.resource = resource;
        resource.amountHeld = resourceAmount;
        resource.maxHoldable = resourceAmount;
        resource.resourceName = resource.GetType().ToString();

        var rangeScale = (1 + 50f / diameter);
        resourceRange.localScale = Vector3.one * rangeScale;

        gatherDistance = (rangeScale * diameter) * 0.5f;
    }

    void Update()
    {
        if (resource.amountHeld <= 0)
        {
            resourceRange.gameObject.SetActive(false);
        }
    }
}