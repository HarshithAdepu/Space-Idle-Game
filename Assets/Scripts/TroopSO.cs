using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Iridium Generator", menuName = "Iridium Generator")]
public class TroopSO : ScriptableObject
{
    public string structureName = "Name";
    public int structureLevel = 1;
    public int structureOwned = 0;
    public float structureBaseCost = 0;
    public float structureBaseIridiumPerSecond = 0;
    public float structureIridiumMultiplier = 1;
    public float structureCostMultiplier = 1.25f;
    public float structureCostMultiplierMultiplier = 1;
}
