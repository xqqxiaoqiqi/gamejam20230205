using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Options/BuildingBehaviours/MarkBuildingDestroyed")]
public class MarkBuildingDestroyedConfig : BuildingBehaviourOptions
{
    public MarkBuildingDestroyedData[] datas;

    [Serializable]
    public class MarkBuildingDestroyedData : BehaviourData
    {
        
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
