using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTile
{
    public int passableWeight = 0;
    
    public List<Behaviour> behaviours;

    public HashSet<Entity> entities = new HashSet<Entity>();

    public BasicBuilding building;

    public TileType tileType;


    public BasicTile(TileType type)
    {
        tileType = type;
        //behaviours = GameManager.instance.TileBehaviourDB.GetBehaviours(tileType);
        for (int i = 0; i < behaviours.Count; i++)
        {
            behaviours[i].OnTileInit(this);
        }
    }
    
    [Serializable]
    public enum TileType
    {
        TILE_MARSH,
        TILE_ICE
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
        public virtual void OnEvent(TileEvent ev, System.Object args = null)
        {
            
        }
    }

    public void OnEntityEnter(Entity entity)
    {
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].OnEvent(TileEvent.ON_ENTITY_ENTER, entity);
            }
            building.OnEntityEnter(entity);
        }
    }

    public void OnEntityLeave(Entity entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
            for (int i = 0; i < behaviours.Count; i++)
            {
                behaviours[i].OnEvent(TileEvent.ON_ENTITY_LEAVE,entity);
            }
        }
    }
    
}
