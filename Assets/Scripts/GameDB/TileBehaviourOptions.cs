using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileBehaviourOptions : ScriptableObject
{

    public TileBehaviourDB.BehaviourType behaviourType;
    [Serializable]
    public class BehaviourData
    {
        public BasicTile.TileEvent onEvent;
        public BasicTile.TileType tileType;
    }

    public virtual BehaviourData[] GetData()
    {
        return null;
    }
}
