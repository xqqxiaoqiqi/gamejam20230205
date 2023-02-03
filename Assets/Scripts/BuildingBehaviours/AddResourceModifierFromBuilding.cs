using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class AddResourceModifierFromBuilding : BasicBuilding.Behaviour
{
    public GameManager.ResourceType sourceType;
    public int sourceValue;
    public GameManager.ResourceType targetType;
    public int targetValue;
    public Modifier myModifier;

    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as AddResourceModifierFromBuildingConfig.AddResourceModifierFromBuildingData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            sourceType = mydata.sourceType;
            sourceValue = mydata.sourceValue;
            targetType = mydata.targetType;
            targetValue = mydata.targetValue;
        }
    }
    
    public override void OnEvent(BasicBuilding.BuildingEvent ev,Object args = null)
    {
        base.OnEvent(ev,args);
        if (ev == onEvent)
        {
            if (myModifier == null)
            {
                myModifier = new Modifier(owner.playerSide, sourceType, sourceValue, targetType, targetValue);
            }
        }
    }
}
