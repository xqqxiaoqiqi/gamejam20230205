using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    public int initModifierTotalCount = 10;
    public int maxBuildingLevel = 3;

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
    
}
