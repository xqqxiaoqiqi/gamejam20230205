using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviourOptions : ScriptableObject
{
    public BuildingBehaviourDB.BehaviourType behaviourType;
    [Serializable]
    public class BehaviourData
    {
        public BasicBuilding.BuildingEvent onEvent;
        public BasicBuilding.BuildingType buildType;
    }

    public virtual BehaviourData[] GetData()
    {
        return null;
    }
}
