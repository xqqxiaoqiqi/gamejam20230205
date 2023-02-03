using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEnterBuilding : BasicBuilding.Behaviour
{
public bool checkCapability = false;
public int maxCapabilityCount = 1;

public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
{
    base.DoSetData(data);
    var mydata = data as EntityEnterBuildingConfig.EnitityEnterBuildingData;
    if (mydata != null)
    {
        onEvent = mydata.onEvent;
        checkCapability = mydata.checkCapability;
        maxCapabilityCount = mydata.maxCapabilityCount;
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
                if (checkCapability)
                {
                    //todo:战斗力计算
                }
                else
                {
                    //todo:mark entity destroyed
                }
            }
        }
    }

}
