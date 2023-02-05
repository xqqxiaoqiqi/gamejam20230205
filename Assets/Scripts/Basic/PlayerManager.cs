using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = System.Object;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    public int initModifierTotalCount = 10;
    public int maxBuildingLevel = 3;
    public int maxContributeValue = 2000;
    public int currContributeValue = 0;

    public Dictionary<GameManager.PlayerSide, int[]> initReSourcesData = new Dictionary<GameManager.PlayerSide, int[]>();
    public Dictionary<PlayerEvent, PlayerEventData> playerEventDatas = new Dictionary<PlayerEvent, PlayerEventData>();
    public List<GameEventData> gameEventDatas = new List<GameEventData>();
    public List<BasicBuilding> allBases = new List<BasicBuilding>();
    public CoolDownTimer gameEventTimer = new CoolDownTimer(0);
    public int currEventIndex;

    protected override void OnInit()
    {
        base.OnInit();
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_A,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_B,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_C,new int[(int)GameManager.ResourceType.ENUM]);
        playerEventDatas.Add(PlayerEvent.INIT_BUILDING, new PlayerEventData(20,PlayerEvent.INIT_BUILDING,"创建一个新的什么玩意"));
        gameEventDatas.Add(new GameEventData(GameEvent.DESTROYENTITYBYCAPABILITY,1200,"严酷环境下弱者难以生还,摧毁所有战斗力低于20的单位","军备竞赛"));
        gameEventDatas.Add(new GameEventData(GameEvent.CHECKRESOURCES,1200,"巨大的寒潮将席卷世界,需要食物超过100","极度深寒"));
        gameEventDatas.Add(new GameEventData(GameEvent.CHECKHOMELEVEL,1200,"文明的进步不容停滞,需要基地等级到达lv.2","发展迟滞"));
        gameEventTimer.Reset(1200);

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
        CHECKHOMELEVEL
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

    public bool DoGameEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.DESTROYENTITYBYCAPABILITY:

                var value = 0;
                for (int i = 0; i < GameManager.instance.Entities.Count; i++)
                {
                    var entity = GameManager.instance.Entities[i];
                    if (entity.capability < 0)
                    {
                        entity.MarkFinish();
                        value++;
                    }
                }

                if (value == GameManager.instance.Entities.Count)
                {
                    return false;
                }

                break;
            case GameEvent.CHECKHOMELEVEL:
                for (int i = 0; i < allBases.Count; i++)
                {
                    if (allBases[i].buildingLevel < 2)
                    {
                        return false;
                    }
                }
                break;
            case GameEvent.CHECKRESOURCES:
                var final = 0;
                foreach (var playerSideData  in  GameManager.instance.allPlayerSideDatas.Values)
                {
                    var data = playerSideData.resourcesData[(int) GameManager.ResourceType.FOOD];
                    final += data;
                }

                if (final < 100)
                {
                    return false;
                }
                break;
            default:
                break;

        }

        return true;
    }

    public void WinGame(WinReason winReason)
    {
        
    }

    public void OnTick()
    {
        CalculateContributeValue();
        gameEventTimer.OnTick();
        if (gameEventTimer.isReady)
        {
            if (!DoGameEvent((GameEvent) currEventIndex))
            {
                LoseGame((GameEvent) currEventIndex);
            }else
            {
                currEventIndex++;
                if (currEventIndex < gameEventDatas.Count)
                {
                    gameEventTimer.Reset(300);
                }
            }
        }

    }

    public void LoseGame(GameEvent gameEvent)
    {
        Debug.Log("LOSE!"+ gameEvent);
    }
    public void CalculateContributeValue()
    {
        var value = 0;
        foreach (var playerSideData in GameManager.instance.allPlayerSideDatas)
        {
            var resValue = playerSideData.Value.resourcesData;
            for (int i = 0; i < resValue.Length; i++)
            {
                value += resValue[i];
            }
        }
        currContributeValue = value;
    }

    public bool CheckPlayerEventValid(PlayerEvent playerEvent)
    {
        if (playerEventDatas.ContainsKey(playerEvent))
        {
            return playerEventDatas[playerEvent].cost<=currContributeValue;
        }

        return false;
    }

    public void OnSelectingTile(PlayerEvent playerEvent)
    {
        
    }

    public class PlayerEventData
    {
        public int cost;
        public PlayerEvent type;
        public string description;

        public PlayerEventData(int cost,PlayerEvent type,string description)
        {
            this.cost = cost;
            this.type = type;
            this.description = description;
        }
    }

    public class GameEventData
    {
        public GameEvent type;
        public int CountDown;
        public string description;
        public string name;
        public GameEventData(GameEvent type,int CountDown,string description,string name)
        {
            this.CountDown = CountDown;
            this.type = type;
            this.description = description;
            this.name = name;

        }
    }
}
