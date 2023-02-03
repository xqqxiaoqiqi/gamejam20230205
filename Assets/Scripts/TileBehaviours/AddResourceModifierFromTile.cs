using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResourceModifierFromTile : BasicTile.Behaviour
{
    public GameManager.ResourceType sourceType;
    public int sourceValue;
    public GameManager.ResourceType targetType;
    public int targetValue;
    public Modifier myModifier;

    public override void DoSetData(TileBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceModifierFromTileConfig.AddResourceModifierFromTileData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            sourceType = mydata.sourceType;
            sourceValue = mydata.sourceValue;
            targetType = mydata.targetType;
            targetValue = mydata.targetValue;
        }
    }

    public override void OnEvent(BasicTile.TileEvent ev,object args = null)
    {
        base.OnEvent(ev,args);
        if (ev == onEvent)
        {
            if (myModifier == null&&owner.building!=null)
            {
                myModifier = new Modifier(owner.building.playerSide, sourceType, sourceValue, targetType, targetValue);
            }
        }
    }
}