using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    //标记已经被一个士兵选中，不会再被下一个士兵当做目标。
    public bool targeted = false;
    public int buildingLevel = 1;
    [Serializable]
    public enum BuildingType
    {
        BUILDING_FOOD,
        BUILDING_METAL,
        BUILDING_POWER,
        BUILDING_FACTORY,
        BUILDING_FOODCHEST,
        BUILDING_METALCHEST,
        BUILDING_ENTITYCHEST,
        BUILDING_ENEMY,
        BUILDING_BASE,
        BUILDING_WONDER,
        FOOD1,
        FOOD2,
        METAL1,
        METAL2,
        POWER1,
        FACTORY1,
        FACTORY2,
        BASE1,
        BASE2
    }
    [Serializable]
    public enum BuildingEvent
    {
        ON_ENTITY_ENTER,
        ON_SWITCH_PLAYER_SIDE,
        ON_BUILD_DESTROY,
        ON_CAPABILITY_ZERO,
        ON_BUILD_UPGRADE
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
            behaviours[i].OnEvent(BuildingEvent.ON_ENTITY_ENTER,entity);
        }
    }

    public void OnSwitchPlayerSide(GameManager.PlayerSide newside)
    {
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnEvent(BuildingEvent.ON_SWITCH_PLAYER_SIDE);
        }
        playerSide = newside;
    }

    public void SetWorkingStatus(bool value)
    {
        if (value)
        {
            if(buildType==BuildingType.BUILDING_FOOD|| buildType == BuildingType.FOOD1|| buildType == BuildingType.FOOD2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.FOOD1]);
            else if (buildType == BuildingType.BUILDING_METAL || buildType == BuildingType.METAL1 || buildType == BuildingType.METAL2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.METAL1]);
            else if (buildType == BuildingType.BUILDING_POWER || buildType == BuildingType.POWER1)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.POWER1]);
            else if (buildType == BuildingType.BUILDING_FACTORY || buildType == BuildingType.FACTORY1 || buildType == BuildingType.FACTORY2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.FACTORY1]);
        }
        else
        {
            if (buildType == BuildingType.BUILDING_FOOD || buildType == BuildingType.FOOD1 || buildType == BuildingType.FOOD2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.FOOD2]);
            else if (buildType == BuildingType.BUILDING_METAL || buildType == BuildingType.METAL1 || buildType == BuildingType.METAL2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.METAL2]);
            else if (buildType == BuildingType.BUILDING_FACTORY || buildType == BuildingType.FACTORY1 || buildType == BuildingType.FACTORY2)
                TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.FACTORY2]);
        }
    }

    public void UpgradeBase(int i)
    {
        if (i == 1)
        {
            TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.BASE1]);
        }
        else if (i == 2)
        {
            TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BuildingType.BASE2]);
        }
    }

    public void OnBuildDestroy()
    {
        //todo: 每次tick结束destroy
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

    public void OnBuildUpgrade()
    {
        if (buildingLevel < PlayerManager.instance.maxBuildingLevel)
        {
            buildingLevel++;
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].OnEvent(BuildingEvent.ON_BUILD_UPGRADE);
            }
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
        this.playerSide = playerSide;
        //接一下on entity enter
        //build init &destroyed on tile map
        //entity init &destroyed on tile map
        pos = position;
    }
    

}
