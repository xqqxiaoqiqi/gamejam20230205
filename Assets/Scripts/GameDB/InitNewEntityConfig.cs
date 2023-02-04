using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Options/BuildingBehaviours/InitNewEntity")]
public class InitNewEntityConfig : BuildingBehaviourOptions
{
    public InitNewEntityData[] datas;

    [Serializable]
    public class InitNewEntityData : BehaviourData
    {
        public int count;
        public int coolDown;
        public bool immediately;

    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
