using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Torappu/Options/TileBehaviourDB")]
public class TileBehaviourDB : ScriptableObject
{

    public TileBehaviourOptions[] behaviourOptions;
    public enum BehaviourType
    {
        ADD_RESOURCE_MODIFIER_FROM_TILE,
        MODIFY_ENTITY_MOVE_SPEED
    }

    public  List<BasicTile.Behaviour> GetBehaviours(BasicTile.TileType type)
    {
        var behaviourList = new List<BasicTile.Behaviour>();
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

    public BasicTile.Behaviour GetBehaviour(BehaviourType type)
    {
        switch (type)
        {
            case BehaviourType.ADD_RESOURCE_MODIFIER_FROM_TILE:
                return new AddResourceModifierFromTile();
            default:
                break;
        }

        return null;
    }
    
}
