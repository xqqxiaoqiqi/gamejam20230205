using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Torappu/Options/AddResourceModifierFromTileConfig")]
public class AddResourceModifierFromTileConfig : TileBehaviourOptions
{
    public AddResourceModifierFromTileData[] datas;
    [Serializable]
    public class AddResourceModifierFromTileData: BehaviourData
    {
        public GameManager.ResourceType type;
        public int value;
    }

    public override BehaviourData[] GetData()
    {
        return datas;
    }
}

