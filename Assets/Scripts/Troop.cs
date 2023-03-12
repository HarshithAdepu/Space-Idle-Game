[System.Serializable]
public class Troop
{
    public string structureName = "Name";
    public int structureLevel = 1;
    public int structureOwned = 0;
    public float structureBaseCost = 0;
    public float structureCurrentCost = 0;
    public float structureBaseIridiumPerSecond = 0;
    public float structureIridiumMultiplier = 1; //TODO: Make this dependant on the level of the building
    public float structureCostMultiplier = 1.25f;
    public float structureCostMultiplierMultiplier = 1;

    public Troop(string name, int baseCost, int baseIridiumPerSecond, float iridiumMultiplier, float costMultiplier, float costMultiplierMultiplier)
    {
        structureName = name;
        structureOwned = 0;
        structureBaseCost = baseCost;
        structureBaseIridiumPerSecond = baseIridiumPerSecond;
        structureIridiumMultiplier = iridiumMultiplier;
        structureCostMultiplier = costMultiplier;
        structureCostMultiplierMultiplier = costMultiplierMultiplier;
    }

    public Troop(TroopSO so)
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
        float x = structureBaseIridiumPerSecond * structureIridiumMultiplier * (1.0f / GameManager.ticksPerSecond);
        return x;
    }
}
