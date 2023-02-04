using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Options/BuildingBehaviours/AddResourceImmedately")]
public class AddResourceImmedatelyConfig : BuildingBehaviourOptions
{
    public AddResourceImmedatelyData[] datas;

    [Serializable]
    public class AddResourceImmedatelyData : BehaviourData
    {
        public GameManager.ResourceType resourceType;
        public int Count = 0;
        public bool needCompareCapability = false;
        public int capabilityCount = 0;
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
