using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Options/TileBehaviours/ModifyCapabilityWhenEntityStay")]
public class ModifyEntityCapabilityConfig  : TileBehaviourOptions
{
    public ModifyEntityCapabilityConfigData[] datas;
    [Serializable]
    public class ModifyEntityCapabilityConfigData: BehaviourData
    { 
        public int value;
    }
    
    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
