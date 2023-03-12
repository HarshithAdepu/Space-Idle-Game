using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public string profileName;
    public float totalIridium;
    public float iridiumPerClickPercent;
    public float upgradeClick_BaseCost;
    public List<Troop> ownedStructures = new List<Troop>();
}
