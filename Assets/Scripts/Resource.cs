[System.Serializable]
public class Resource
{
    public string resourceName;
    public float amountHeld;
    public float maxHoldable;

    public void RemoveResource(float amount)
    {
        if (amountHeld - amount < 0)
        {
            amountHeld = 0;
            return;
        }

        amountHeld -= amount;
    }

    public void AddResource(float amount)
    {
        if (amountHeld + amount > maxHoldable)
        {
            amountHeld = maxHoldable;
            return;
        }
        amountHeld += amount;
    }

}

[System.Serializable]
public class Fuel : Resource
{ }

[System.Serializable]
public class Power : Resource
{ }

[System.Serializable]
public class Oxygen : Resource
{ }

[System.Serializable]
public class Organic : Resource
{ }

[System.Serializable]
public class Water : Resource
{ }