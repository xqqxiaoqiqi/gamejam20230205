using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyEnitityCapability :  BasicTile.Behaviour
{
    
    public int value;
    public override void DoSetData(TileBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as ModifyEnitityCapabilityConfig.ModifyEnitityCapabilityConfigData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            value = mydata.value;
        }
    }

    public override void OnEvent(BasicTile.TileEvent ev,object args = null)
    {
        base.OnEvent(ev,args);
        if (ev == onEvent)
        {
            var entity = args as Entity;
            if (entity != null)
            {
                entity.capability += value;
            }

        }
    }
    
}
