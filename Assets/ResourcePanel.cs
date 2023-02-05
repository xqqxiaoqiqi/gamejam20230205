using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourcePanel : BuildingPanel
{
    public Text type;
    public Text value;
    public Text Power;
    public Image background;

    public override void OnInit(object args)
    {
        base.OnInit(args);
        var building = args as BasicBuilding;
        if (building != null)
        {
            var myModifier = GetFirstModifier(building);
            if (myModifier == null)
            {
                Debug.LogError("NOMODIFIER");
            }
            else
            {
                var index = (int) building.playerSide;
                type.text =  UIManager.instance.sideTexts[index-1];
                switch (building.buildType)
                {
                    case BasicBuilding.BuildingType.BUILDING_METAL:
                        value.color =  new Color(9,255,188);
                        Power.text = myModifier.sourceValue.ToString();
                        value.text = myModifier.targetValue + "/s";
                        background.sprite = UIManager.instance.metalPanelSprite;
                        break;
                    case BasicBuilding.BuildingType.BUILDING_FOOD:
                        value.color = new Color(255,90,9);
                        Power.text = myModifier.sourceValue.ToString();
                        value.text = myModifier.targetValue + "/s";
                        background.sprite = UIManager.instance.farmPanelSprite;
                        break;
                    case BasicBuilding.BuildingType.BUILDING_FACTORY:
                        value.color = new Color(255,80,78);
                        Power.text = myModifier.sourceValue.ToString();
                        value.text = myModifier.targetValue.ToString();
                        background.sprite = UIManager.instance.metalPanelSprite;
                        break;
                    default:
                        break;
                }


            }
        }
    }

    public Modifier GetFirstModifier(BasicBuilding building)
    {
        for (int i = 0; i < building.behaviours.Count; i++)
        {
            var behaviour = building.behaviours[i] as AddResourceModifierFromBuilding;
            if (behaviour != null && behaviour.myModifier != null)
            {
                return behaviour.myModifier;
            }
        }
        return null;
    }
}
