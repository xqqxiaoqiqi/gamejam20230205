using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Options/TileBehaviours/ModifyEntityMoveSpeed")]
public class ModifyEntityMoveSpeedConfig : TileBehaviourOptions
{
    public ModifyEntityMoveSpeedConfigData[] datas;
    [Serializable]
    public class ModifyEntityMoveSpeedConfigData: BehaviourData
    { 
        public int value;
    }
    
    public override BehaviourData[] GetData()
    {
        return datas;
    }
}
