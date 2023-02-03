using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddResourceModifierFromBuilding : BasicBuilding.Behaviour
{
    public GameManager.ResourceType type;
    public int value;

    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceModifierFromBuildingConfig.AddResourceModifierFromBuildingData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            type = mydata.type;
            value = mydata.value;
        }
    }
    
    public override void OnEvent(BasicBuilding.BuildingEvent ev)
    {
        base.OnEvent(ev);
        if (ev == onEvent)
        {
            Debug.Log("do something here");
        }
    }
}
