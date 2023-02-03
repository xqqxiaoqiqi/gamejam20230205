using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum PlayerSide
    { 
        SIDE_A,
        SIDE_B,
        ENUM//
    }
    public enum ResourceType
    {
        FOOD = 0,
        METAL = 1,
        POWER =2,
        ENUM = 3
    }

    public class PlayerSideData
    {
        public int[] resourcesData = new int[(int)ResourceType.ENUM];
        public List<List<int>> modifiersValue = new List<List<int>>();
        public List<Modifier> modifierSources = new List<Modifier>();

        public void OnApplyingModifier(ResourceType resourceType, int count)
        {
            modifiersValue[(int)resourceType].Add(count);
        }

        public void DoApplyAllModifier()
        {
            for (int i = 0; i < modifierSources.Count; i++)
            {
                int value = 0;
                if (modifierSources[i].OnApplyingModifier())
                {
                    modifierSources[i].DoApplyModifier();
                } 
            }
        }
    }

    public Transform tileRoot;
    public int mapHeight;
    public int mapWidth;

    public List<Entity> Entities = new List<Entity>();
    public Dictionary<PlayerSide, List<BasicBuilding>> allBuildings = new Dictionary<PlayerSide, List<BasicBuilding>>();
    public Dictionary<PlayerSide,PlayerSideData> allPlayerSideDatas = new Dictionary<PlayerSide, PlayerSideData>();
    public List<BasicTile> testTile = new List<BasicTile>();
    public List<BasicBuilding> testBuilding = new List<BasicBuilding>();
    public TileBehaviourDB TileBehaviourDB;
    public BuildingBehaviourDB BuildingBehaviourDB;
    int test = 0;
    private int total = 30;
    public void FixedUpdate()
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].OnTick();
        }

        foreach (var buildings in allBuildings.Values)
        {
            for (int i = 0; i < buildings.Count; i++)
            {
                buildings[i].OnTick();
            }
        }

        if (test < total)
        {
            test++;
        }
        else
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

            test = 0;
        }
        
    }

    public void InitMap()
    {
        
    }

    private void Start()
    {
        allPlayerSideDatas.Add(PlayerSide.SIDE_A,new PlayerSideData());
        allPlayerSideDatas.Add(PlayerSide.SIDE_B,new PlayerSideData());
        testTile.Add(new BasicTile(BasicTile.TileType.TILE_ICE));
        testTile.Add(new BasicTile(BasicTile.TileType.TILE_MARSH));
        testBuilding.Add(new BasicBuilding(PlayerSide.ENUM,BasicBuilding.BuildingType.BUILDING_POWER));
        var test_1=new Modifier(PlayerSide.SIDE_A, ResourceType.POWER, 0, ResourceType.POWER, 20);
        var test_2=new Modifier(PlayerSide.SIDE_A, ResourceType.POWER, -20, ResourceType.FOOD, 7);
        var test_3=new Modifier(PlayerSide.SIDE_A, ResourceType.POWER, -20, ResourceType.METAL, 7);
        

    }
}