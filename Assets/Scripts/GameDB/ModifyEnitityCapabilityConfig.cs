using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Options/TileBehaviours/ModifyCapabilityWhenEntityStay")]
public class ModifyEnitityCapabilityConfig  : TileBehaviourOptions
{
    public ModifyEnitityCapabilityConfigData[] datas;
    [Serializable]
    public class ModifyEnitityCapabilityConfigData: BehaviourData
    { 
        public int value;
    }
    
    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
