using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitNewEntity : BasicBuilding.Behaviour
{
    public int count;
    public CoolDownTimer triggerTimer;
    public bool immediately;


    public override void DoSetData(BuildingBehaviourOptions.BehaviourData data)
    {
        base.DoSetData(data);
        var mydata = data as InitNewEntityConfig.InitNewEntityData;
        if (mydata != null)
        {
            onEvent = mydata.onEvent;
            count = mydata.count;
            immediately = mydata.immediately;
            if (!immediately)
            {
                triggerTimer = new CoolDownTimer(mydata.coolDown);
            }
        }
    }

    public override void OnTick()
    {
        base.OnTick();
        triggerTimer.OnTick();
    }

    public override void OnEvent(BasicBuilding.BuildingEvent ev, object args = null)
    {
        base.OnEvent(ev, args);
        if (ev == onEvent)
        {
            if (immediately)
            {
                if (owner.playerSide!=GameManager.PlayerSide.NATURE)
                {
                    GameManager.instance.InitEntity(owner.playerSide,owner.pos);
                }
            }
            if (triggerTimer.isReady)
            {
                if (owner.playerSide!=GameManager.PlayerSide.NATURE)
                {
                    GameManager.instance.InitEntity(owner.playerSide,owner.pos);
                }
                triggerTimer.Reset();
            }
        }
    }

}