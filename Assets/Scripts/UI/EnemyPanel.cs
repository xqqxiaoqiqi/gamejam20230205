using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : BuildingPanel
{
    public Text caText;

    public override void OnInit(object args)
    {
        base.OnInit(args);
        var building = args as BasicBuilding;
        if (building != null && building.hasCapability)
        {
            caText.text = building.capabilityCount.ToString();
        }
    }
}
