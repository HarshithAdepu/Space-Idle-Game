using UnityEngine;

[System.Serializable]
public class IridiumGenerator
{
    public string structureName = "Name";
    public int structureOwned = 0;
    public float structureBaseCost = 0;
    public float structureCurrentCost = 0;
    public float structureBaseIridiumPerSecond = 0;
    public float structureIridiumMultiplier = 1;
    public float structureCostMultiplier = 1.25f;
    public float structureCostMultiplierMultiplier = 1;

    public IridiumGenerator(string name, int baseCost, int baseIridiumPerSecond, float iridiumMultiplier, float costMultiplier, float costMultiplierMultiplier)
    {
        structureName = name;
        structureOwned = 0;
        structureBaseCost = baseCost;
        structureBaseIridiumPerSecond = baseIridiumPerSecond;
        structureIridiumMultiplier = iridiumMultiplier;
        structureCostMultiplier = costMultiplier;
        structureCostMultiplierMultiplier = costMultiplierMultiplier;
    }

    public IridiumGenerator(IridiumGeneratorSO so)
    {
        structureName = so.structureName;
        structureOwned = so.structureOwned;
        structureBaseCost = so.structureBaseCost;
        structureBaseIridiumPerSecond = so.structureBaseIridiumPerSecond;
        structureIridiumMultiplier = so.structureIridiumMultiplier;
        structureCostMultiplier = so.structureCostMultiplier;
        structureCostMultiplierMultiplier = so.structureCostMultiplierMultiplier;
    }

    public float GetIridiumPerTick()
    {
        float x = structureBaseIridiumPerSecond * structureOwned * structureIridiumMultiplier * (1.0f / GameManager.ticksPerSecond);
        return x;
    }
}
