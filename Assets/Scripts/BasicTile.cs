using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile
{
    public bool isPassable = false;

    public int passableWeight = 0;
    
    public Behaviour[] behaviours;

    public HashSet<Entity> entities = new HashSet<Entity>();

    public BasicBuilding building;
    
    [Serializable]
    public enum TileType
    {
        TILE_FARM,
    }
    
    [Serializable]
    public enum TileEvent
    {
        ON_ENTITY_ENTER,
        ON_ENTITY_LEAVE
    }

    [Serializable]
    public class Behaviour
    {
        public BasicTile owner;
        public TileEvent onEvent;
        public virtual void OnTileInit(BasicTile tile)
        {
            owner = tile;
        }

        public virtual void DoSetData(TileBehaviourOptions.BehaviourData data)
        {
            
        }
        public virtual void OnEvent(TileEvent ev)
        {
            
        }
    }

    public void OnEntityEnter(Entity entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].OnEvent(TileEvent.ON_ENTITY_ENTER);
            }
            building.OnEntityEnter(entity);
        }
    }

    public void OnEntityLeave(Entity entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
            for (int i = 0; i < behaviours.Length; i++)
            {
                behaviours[i].OnEvent(TileEvent.ON_ENTITY_LEAVE);
            }
        }
    }

    private void Awake()
    {
        //behaviours = GetComponentsInChildren<Behaviour>();
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i].OnTileInit(this);
        }
    }
}
