using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum PlayerSide
    { 
        NATURE,
        SIDE_A,
        SIDE_B,
        SIDE_C,
        ENUM//
    }
    public enum ResourceType
    {
        FOOD = 0,
        METAL = 1,
        POWER =2,
        ENTITY_CAPABILITY =3,
        ENUM = 4
    }

    public class PlayerSideData
    {
        public int[] resourcesData = new int[(int)ResourceType.ENUM];
        public List<Modifier> modifierSources = new List<Modifier>();
        public void DoApplyAllModifier()
        {
            resourcesData[(int) ResourceType.ENTITY_CAPABILITY] = 0;
            for (int i = 0; i < modifierSources.Count; i++)
            {
                int value = 0;
                if (modifierSources[i].targetType==ResourceType.POWER)
                {
                    modifierSources[i].DoApplyModifier();
                }
            }
            for (int i = 0; i < modifierSources.Count; i++)
            {
                int value = 0;
                if (modifierSources[i].targetType!=ResourceType.POWER)
                {
                    if (modifierSources[i].OnApplyingModifier())
                    {
                        modifierSources[i].DoApplyModifier();
                    }
                }
            }
            resourcesData[(int)ResourceType.POWER] = 0;
        }
    }
    public List<Tile> buildingSource;
    public Tile selectTile;
    public Tile selectFailTile;

    public GameObject entitySource;
    public GameObject entitySource1;
    public GameObject entitySource2;
    public Transform tileRoot;
    public int mapHeight;
    public int mapWidth;

    public List<Entity> Entities = new List<Entity>();
    public Dictionary<Vector3Int, BasicBuilding> buildings = new Dictionary<Vector3Int, BasicBuilding>();
    public Dictionary<PlayerSide,PlayerSideData> allPlayerSideDatas = new Dictionary<PlayerSide, PlayerSideData>();
    public List<BasicTile> testTile = new List<BasicTile>();
    public List<BasicBuilding> testBuilding = new List<BasicBuilding>();
    public TileBehaviourDB TileBehaviourDB;
    public BuildingBehaviourDB BuildingBehaviourDB;
    public CoolDownTimer gameTickTimer = new CoolDownTimer(30);
    public Vector3Int selectPos;
    public bool isGameStarted = false;
    public void FixedUpdate()
    {
        selectPos = UIManager.instance.showSelection();
        if (!isGameStarted)
        {
            return;
        }
        gameTickTimer.OnTick();
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].OnTick();
        }

        foreach (var building in buildings.Values)
        {
            building.OnTick();
        }

        if (gameTickTimer.isReady)
        {
            foreach (var playerSideData in allPlayerSideDatas.Values)
            {
                playerSideData.DoApplyAllModifier();
            }
            //test
            foreach(var playerdata in GameManager.instance.allPlayerSideDatas)
            {
                var side = playerdata.Key;
                var resources = playerdata.Value.resourcesData;
                for (int i = 0; i < resources.Length; i++)
                {
                    var resourceType = (GameManager.ResourceType)i;
                    Debug.Log(side.ToString() + resourceType + ":"+resources[i]);
                }
            }
            gameTickTimer.Reset();
        }
        var destroyList = new List<Vector3Int>();
        foreach(var building in buildings)
        {
            if (building.Value.isDestroyed)
            {
                destroyList.Add(building.Key);
            }
        }
        foreach(var key in destroyList)
        {
            TileManager.Instance.buildingMap.SetTile(key, null);
            buildings.Remove(key);
        }

        for(int i=Entities.Count-1;i>=0;i--)
        {
            if (Entities[i].isFinished)
            {
                Destroy(Entities[i].gameObject);
                Entities.RemoveAt(i);
            }
        }
        UIManager.instance.OnTick();
        PlayerManager.instance.CalculateContributeValue();
        UIManager.instance.UpdateContributeValue();
    }

    public void InitMap()
    {
        
    }

    public void InitEntity(PlayerSide playerSide, Vector3Int position)
    {
        GameObject source=null;
        if (playerSide == PlayerSide.SIDE_A)
            source = entitySource;
        else if (playerSide == PlayerSide.SIDE_B)
            source = entitySource1;
        else if (playerSide == PlayerSide.SIDE_C)
            source = entitySource2;
        var obj=GameObject.Instantiate(source);
        var newEntity = obj.GetComponent<Entity>();
        newEntity.playerSide = playerSide;
        obj.transform.position = TileManager.Instance.terrainMap.CellToWorld(position);
    }

    private void Start()
    {
        InitGame();
        TileManager.Instance.Init();
        InitBuildings();
        UIManager.instance.InitUIRoot();
    }

    public void InitGame()
    {
        allPlayerSideDatas.Add(PlayerSide.SIDE_A,new PlayerSideData());
        allPlayerSideDatas.Add(PlayerSide.SIDE_B,new PlayerSideData());
        allPlayerSideDatas.Add(PlayerSide.SIDE_C, new PlayerSideData());
    }

    private void InitBuildings()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                var pos = new Vector3Int(i, j, 0);
                for (int k = 0; k < buildingSource.Count; k++)
                    if (TileManager.Instance.buildingMap.GetTile(pos) == buildingSource[k])
                    {
                        buildings.Add(pos, new BasicBuilding(PlayerSide.NATURE, (BasicBuilding.BuildingType)k, pos));
                    }
            }
        }

    }

    public bool PosValid(Vector3Int pos)
    {
        if (pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight && pos[2] == 0)
        {
            return true;
        }
        return false;
    }
}

public class CoolDownTimer
{
    public int coolDown;
    public int current;

    public CoolDownTimer(int coolDown)
    {
        this.coolDown = coolDown;
        current = 0;
    }

    public void OnTick()
    {
        current++;
    }

    public bool isReady
    {
        get { return current >= coolDown; }
    }

    public void Reset()
    {
        current = 0;
    }

}