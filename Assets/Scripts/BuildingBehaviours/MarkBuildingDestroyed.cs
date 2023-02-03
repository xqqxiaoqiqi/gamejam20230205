using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBuildingDestroyed : BasicBuilding.Behaviour
{
    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as MarkBuildingDestroyedConfig.MarkBuildingDestroyedData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
        }
    }

    public override void OnEvent(BasicBuilding.BuildingEvent ev, object args = null)
    {
        base.OnEvent(ev, args);

        if (ev == onEvent)
        {
            owner.MarkDestroyed();
        }
    }
}
