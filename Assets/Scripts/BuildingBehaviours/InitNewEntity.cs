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
        if (triggerTimer == null)
            return;
        triggerTimer.OnTick();
        if (triggerTimer.isReady)
        {
            if (owner.playerSide != GameManager.PlayerSide.NATURE)
            {
                GameManager.instance.InitEntity(owner.playerSide, owner.pos);
            }
            triggerTimer.Reset();
        }
    }

    public override void OnEvent(BasicBuilding.BuildingEvent ev, object args = null)
    {
        base.OnEvent(ev, args);
        if (ev == onEvent)
        {
            if (immediately)
            {
                var entity = args as Entity;
                if (entity != null)
                {
                    if (entity.playerSide != GameManager.PlayerSide.NATURE)
                    {

                        GameManager.instance.InitEntity(entity.playerSide, owner.pos);
                    }
                }
            }
        }
    }

}