using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObjects/ShipParts", fileName = "ShipParts", order = 2)]
[System.Serializable]
public class ShipPartsScriptableObject : ScriptableObject
{
    public ShipPart.eShipPartType   type;
    public ShipPartInfo[]           partInfos;
}

[System.Serializable]
public class ShipPartInfo
{
    public string       name;
    public GameObject   prefab;
}
