using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResourceImmedately : BasicBuilding.Behaviour
{
    public GameManager.ResourceType resourceType;
    public int count = 0;
    public bool needCompareCapability = false;
    public int capabilityCount = 0;

    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceImmedatelyConfig.AddResourceImmedatelyData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            resourceType = mydata.resourceType;
            count = mydata.Count;
            needCompareCapability = mydata.needCompareCapability;
            capabilityCount = mydata.capabilityCount;
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
                if (needCompareCapability&&entity.capability<capabilityCount)
                {
                    return;
                }
                var resourcedata = GameManager.instance.allPlayerSideDatas[entity.playerSide].resourcesData;
                resourcedata[(int) resourceType] += count;
                
            }
        }
    }

}