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
        COIN = 1,
        ENUM = 2
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

        foreach (var playerSideData in allPlayerSideDatas.Values)
        {
            playerSideData.DoApplyAllModifier();
        }
        
        
    }

    public void InitMap()
    {
        
    }
}