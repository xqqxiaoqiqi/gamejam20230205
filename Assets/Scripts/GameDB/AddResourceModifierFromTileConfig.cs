using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Options/TileBehaviours/AddResourceModifierFromTile")]
public class AddResourceModifierFromTileConfig : TileBehaviourOptions
{
    public AddResourceModifierFromTileData[] datas;
    [Serializable]
    public class AddResourceModifierFromTileData: BehaviourData
    {
        public GameManager.ResourceType sourceType;
        public int sourceValue;
        public GameManager.ResourceType targetType;
        public int targetValue;
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}

