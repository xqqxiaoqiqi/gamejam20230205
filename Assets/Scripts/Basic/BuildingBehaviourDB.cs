using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Torappu/Options/BuildingBehaviourDB")]
public class BuildingBehaviourDB : ScriptableObject
{

    public BuildingBehaviourOptions[] behaviourOptions;
    public enum BehaviourType
    {
        ADD_RESOURCE_MODIFIER_FROM_BUILDING,
        DISABLE_ENTITY,
        ADD_RESOURCE_IMMEDATELY
    }

    public  List<BasicBuilding.Behaviour> GetBehaviours(BasicBuilding.BuildingType type)
    {
        var behaviourList = new List<BasicBuilding.Behaviour>();
        for (int i = 0; i < behaviourOptions.Length; i++)
        {
            var data = behaviourOptions[i].GetData();
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].tileType == type)
                {
                    var behaviour = GetBehaviour(behaviourOptions[i].behaviourType);
                    behaviour.DoSetData(data[j]);
                    behaviourList.Add(behaviour);
                }
            }
        }

        return behaviourList;
    }

    public BasicBuilding.Behaviour GetBehaviour(BehaviourType type)
    {
        switch (type)
        {
            case BuildingBehaviourDB.BehaviourType.ADD_RESOURCE_MODIFIER_FROM_BUILDING:
                return new AddResourceModifierFromBuilding();
            default:
                break;
        }

        return null;
    }
    
}