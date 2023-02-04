using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 任何浮在地块上层的静态物体（包括宝箱，资源点，统称为building，一个地块上只 能 有 一 个 building）
/// </summary>
public class BasicBuilding
{
    public List<Behaviour> behaviours;
    public GameManager.PlayerSide playerSide = GameManager.PlayerSide.NATURE;
    public bool isDestroyed = false;
    public BuildingType buildType;
    public Vector3Int pos;
    [Serializable]
    public enum BuildingType
    {
        BUILDING_FARM,
        BUILDING_POWER,
        BUILDING_MINE,
        BUILDING_HOME,
        TREASURE_FOOD,
        TREASURE_METAL,
        TREASURE_ENTITY,
        BUILDING_CAMP,
        BUILDING_WEAPON_FACTORY


    }
    [Serializable]
    public enum BuildingEvent
    {
        ON_ENTITY_ENTER,
        ON_SWITCH_PLAYER_SIDE,
        ON_BUILD_DESTROY,
        ON_CAPABILITY_ZERO
    }
    public class Behaviour
    {

        public BuildingEvent onEvent;
        public BasicBuilding owner;
        
        public virtual void OnBuildingInit(BasicBuilding building)
        {
            owner = building;
        }
        
        public virtual void DoSetData(BuildingBehaviourOptions.BehaviourData data)
        {
            
        }
        public virtual void OnEvent(BuildingEvent ev,object args = null)
        {
            
        }
        
        public virtual void OnTick()
        {
        
        }
    }

    public void OnEntityEnter(Entity entity)
    {
        if (playerSide != GameManager.PlayerSide.NATURE||isDestroyed)
        {
            //防止同一次tick里两个单位同时enter的问题，当这个建筑被第一次enter（占领/破坏）以后，以目前的设计后续所有单位enter都不会做出任何相应
            return;
        }
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnEvent(BuildingEvent.ON_ENTITY_ENTER);
        }
    }

    public void OnSwitchPlayerSide(GameManager.PlayerSide newside)
    {
        GameManager.instance.allBuildings[playerSide].Remove(this);
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnEvent(BuildingEvent.ON_SWITCH_PLAYER_SIDE);
        }
        GameManager.instance.allBuildings[newside].Add(this);
        playerSide = newside;
    }

    public void OnBuildDestroy()
    {
        //todo: 每次tick结束destroy
        GameManager.instance.allBuildings[playerSide].Remove(this);
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnEvent(BuildingEvent.ON_BUILD_DESTROY);
        }
    }

    public void MarkDestroyed()
    {
        isDestroyed = true;
    }
    public void OnTick()
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnTick();
        }
    }

    public BasicBuilding(GameManager.PlayerSide playerSide,BuildingType type,Vector3Int position)
    {
        buildType = type;
        behaviours = GameManager.instance.BuildingBehaviourDB.GetBehaviours(type);
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnBuildingInit(this);
        }
        pos = position;
    }
    
}
