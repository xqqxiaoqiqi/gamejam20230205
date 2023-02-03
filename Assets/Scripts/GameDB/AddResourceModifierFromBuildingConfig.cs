using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Torappu/Options/AddResourceModifierFromBuildingConfig")]
public class AddResourceModifierFromBuildingConfig : BuildingBehaviourOptions
{
    public AddResourceModifierFromBuildingData[] datas;

    [Serializable]
    public class AddResourceModifierFromBuildingData : BehaviourData
    {
        public GameManager.ResourceType type;
        public int value;
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}