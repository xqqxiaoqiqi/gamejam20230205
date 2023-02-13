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
    public int maxContributeValue = 200000;
    public int currContributeValue = 0;
    public int basicCapability = 1;

    public Dictionary<GameManager.PlayerSide, int[]> initReSourcesData = new Dictionary<GameManager.PlayerSide, int[]>();
    public Dictionary<PlayerEvent, PlayerEventData> playerEventDatas = new Dictionary<PlayerEvent, PlayerEventData>();
    public List<GameEventData> gameEventDatas = new List<GameEventData>();
    public List<BasicBuilding> allBases = new List<BasicBuilding>();
    public CoolDownTimer gameEventTimer = new CoolDownTimer(0);
    public int currEventIndex;
    public int upgradeRequest = 6000;
    public int upgradeRequest1 = 10000;


    protected override void OnInit()
    {
        base.OnInit();
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_A,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_B,new int[(int)GameManager.ResourceType.ENUM]);
        initReSourcesData.Add(GameManager.PlayerSide.SIDE_C,new int[(int)GameManager.ResourceType.ENUM]);
        playerEventDatas.Add(PlayerEvent.INIT_FLAG, new PlayerEventData(10, PlayerEvent.INIT_FLAG, "指引最近的居民前往该点，仅可选择中立非空白地块"));
        playerEventDatas.Add(PlayerEvent.INIT_FOOD, new PlayerEventData(20,PlayerEvent.INIT_FOOD,"创建一个食物箱子"));
        playerEventDatas.Add(PlayerEvent.INIT_METAL, new PlayerEventData(20, PlayerEvent.INIT_METAL, "创建一个金属箱子"));
        playerEventDatas.Add(PlayerEvent.INIT_NEW_ENTITY, new PlayerEventData(50, PlayerEvent.INIT_NEW_ENTITY, "创建一个中立居民"));
        gameEventDatas.Add(new GameEventData(GameEvent.DESTROYENTITYBYCAPABILITY,3600,"严酷环境下弱者难以生还,摧毁所有战斗力低于5的单位","军备竞赛"));
        gameEventDatas.Add(new GameEventData(GameEvent.CHECKRESOURCES,3600,"巨大的寒潮将席卷世界,需要食物超过3000","极度深寒"));
        gameEventDatas.Add(new GameEventData(GameEvent.CHECKHOMELEVEL,3600,"文明的进步不容停滞,需要基地等级到达lv.3","发展迟滞"));
        gameEventTimer.Reset(3600);
    }

    public enum PlayerEvent
    {
        INIT_FOOD,
        INIT_METAL,
        INIT_NEW_ENTITY,
        INIT_FLAG,
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
        CONTRIBUTE,
        OCCUPY,
        WAR
    }

    public void DoPlayerEvent(PlayerEvent playerEvent, GameManager.PlayerSide playerSide, Vector3Int position, Object args)
    {
        switch (playerEvent)
        {
            case PlayerEvent.INIT_NEW_ENTITY:
                GameManager.instance.InitEntity(playerSide, position);
                break;
            case PlayerEvent.INIT_FOOD:
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
                    if (entity.capability < 5)
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
                    if (allBases[i].buildingLevel < 3)
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

                if (final < 3000)
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
        UIManager.instance.endImage.gameObject.SetActive(true);
        if (winReason == WinReason.CONTRIBUTE)
        {
            UIManager.instance.endImage.image.sprite = UIManager.instance.EndSpriteContri;
        }
        else if (winReason == WinReason.WAR)
        {
            UIManager.instance.endImage.image.sprite = UIManager.instance.EndSpriteWar;
        }
        else if (winReason == WinReason.OCCUPY)
        {
            UIManager.instance.endImage.image.sprite = UIManager.instance.EndSpriteOccupy;
        }

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
                    gameEventTimer.Reset(3600);
                }
            }
        }

    }

    public void LoseGame(GameEvent gameEvent)
    {

        UIManager.instance.endImage.gameObject.SetActive(true);
        if (gameEvent == GameEvent.CHECKHOMELEVEL || gameEvent == GameEvent.DESTROYENTITYBYCAPABILITY)
            UIManager.instance.endImage.image.sprite = UIManager.instance.EndSpritePoor;
        else if (gameEvent == GameEvent.CHECKRESOURCES)
            UIManager.instance.endImage.image.sprite = UIManager.instance.EndSpriteCold;

    }
    public void CalculateContributeValue()
    {
        var value = 0;
        foreach (var playerSideData in GameManager.instance.allPlayerSideDatas)
        {
            var resValue = playerSideData.Value.resourcesData;
            value +=resValue[1];
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
        switch (playerEvent)
        {
            case PlayerEvent.INIT_FOOD:
                UIManager.instance.BeginSelection(UIManager.SelectStatus.FOODCHEST);
                break;
            case PlayerEvent.INIT_METAL:
                UIManager.instance.BeginSelection(UIManager.SelectStatus.METALCHEST);
                break;
            case PlayerEvent.INIT_NEW_ENTITY:
                UIManager.instance.BeginSelection(UIManager.SelectStatus.ENTITYCHEST);
                break;
            case PlayerEvent.INIT_FLAG:
                UIManager.instance.BeginSelection(UIManager.SelectStatus.FLAG);
                break;
        }



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
