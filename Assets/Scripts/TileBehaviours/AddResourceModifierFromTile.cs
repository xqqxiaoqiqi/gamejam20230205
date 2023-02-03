using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResourceModifierFromTile : BasicTile.Behaviour
{
    public GameManager.ResourceType type;
    public int value;

    public override void DoSetData(TileBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceModifierFromTileConfig.AddResourceModifierFromTileData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            type = mydata.type;
            value = mydata.value;
        }
    }

    public override void OnEvent(BasicTile.TileEvent ev)
    {
        base.OnEvent(ev);
        if (ev == onEvent)
        {
            Debug.Log("do something here");
        }
    }
}