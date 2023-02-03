using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResourceImmedately : BasicBuilding.Behaviour
{
    public GameManager.ResourceType resourceType;
    public int count = 0;

    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceImmedatelyConfig.AddResourceImmedatelyData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            resourceType = mydata.resourceType;
            count = mydata.Count;
        }
    }

    public override void OnEvent(BasicBuilding.BuildingEvent ev, object args = null)
    {
        base.OnEvent(ev, args);
        
        var entity = args as Entity;
        if (entity != null)
        {
            if (ev == onEvent)
            {
                var resourcedata = GameManager.instance.allPlayerSideDatas[entity.playerSide].resourcesData;
                resourcedata[(int) resourceType] += count;
            }
        }
    }

}