using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Options/BuildingBehaviours/EnitityEnterBuilding")]
public class EntityEnterBuildingConfig : BuildingBehaviourOptions
{
    public EnitityEnterBuildingData[] datas;

    [Serializable]
    public class EnitityEnterBuildingData : BehaviourData
    {
        public bool checkCapability = false;
        public int maxCapabilityCount = 1;
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}