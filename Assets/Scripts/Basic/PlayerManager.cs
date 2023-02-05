using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    public int initModifierTotalCount = 10;
    public int maxBuildingLevel = 3;
    public int maxContributeValue = 2000;
    public int currContributeValue = 0;

    public Dictionary<GameManager.PlayerSide, int[]> initReSourcesData = new Dictionary<GameManager.PlayerSide, int[]>();

    protected override void OnInit()
    {
        base.OnInit();
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_A,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_B,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_C,new int[(int)GameManager.ResourceType.ENUM]);

    }

    public enum PlayerEvent
    {
        INIT_NEW_ENTITY,
        INIT_BUILDING,
        //todo:信号旗！
        ENUM
    }

    public enum GameEvent
    {
        DESTROYENTITYBYCAPABILITY,
        CHECKRESOURCES,
        CHECKHOMELEVEL,
    }
    
    public enum WinReason
    {
        
    }

    public void DoPlayerEvent(PlayerEvent playerEvent, GameManager.PlayerSide playerSide, Vector3Int position, Object args)
    {
        switch (playerEvent)
        {
            case PlayerEvent.INIT_NEW_ENTITY:
                GameManager.instance.InitEntity(playerSide, position);
                break;
            case PlayerEvent.INIT_BUILDING:
                //todo: init building
                var buildingType =(BasicBuilding.BuildingType) args;
                GameManager.instance.buildings.Add(position, new BasicBuilding(playerSide, buildingType, position));
                TileManager.Instance.buildingMap.SetTile(position, GameManager.instance.buildingSource[(int)buildingType]);
                break;
            default:
                break;

        }
    }

    public void DoGameEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.DESTROYENTITYBYCAPABILITY:
                //TODO:GET ALL ENTITIES AND CHECK THEM
                foreach(var entity in GameManager.instance.Entities)
                {
                    if (entity.capability >= 20)
                        entity.MarkFinish();
                }

                break;
            case GameEvent.CHECKHOMELEVEL:
            //todo: CHECK HOME LEVEL
                break;
            case GameEvent.CHECKRESOURCES:
                break;
            default:
                break;

        }
    }

    public void WinGame(WinReason winReason)
    {
        
    }

    public void CalculateContributeValue()
    {
        //todo:计算公式
    }

    public bool CheckPlayerEventValid(PlayerEvent playerEvent)
    {
        return true;
        //todo:
    }
}
