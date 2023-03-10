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
        ENTITY_ENTER_BUILDING,
        ADD_RESOURCE_IMMEDATELY,
        MARK_BUILDING_DESTROYED,
        INIT_NEW_ENTITY
    }

    public  List<BasicBuilding.Behaviour> GetBehaviours(BasicBuilding.BuildingType type)
    {
        var behaviourList = new List<BasicBuilding.Behaviour>();
        for (int i = 0; i < behaviourOptions.Length; i++)
        {
            var data = behaviourOptions[i].GetData();
            for (int j = 0; j < data.Length; j++)
            {
                if (data[j].buildType == type)
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
            case BehaviourType.ADD_RESOURCE_MODIFIER_FROM_BUILDING:
                return new AddResourceModifierFromBuilding();
            case BehaviourType.ENTITY_ENTER_BUILDING:
                return new EntityEnterBuilding();
            case BehaviourType.ADD_RESOURCE_IMMEDATELY:
                return new AddResourceImmedately();
            case BehaviourType.MARK_BUILDING_DESTROYED:
                return new MarkBuildingDestroyed();
            case BehaviourType.INIT_NEW_ENTITY:
                return new InitNewEntity();
            default:
                break;
        }

        return null;
    }
    
}