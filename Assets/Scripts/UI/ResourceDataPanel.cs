using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDataPanel : MonoBehaviour
{
    public GameManager.ResourceType resourceType;
    public Text modifierCount;

    public void UpdateResourceData(GameManager.PlayerSide playerSide)
    {
        var modifiers = GameManager.instance.allPlayerSideDatas[playerSide].modifierSources;
        var modifierValue = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            if (resourceType == GameManager.ResourceType.POWER)
            {
                if (modifiers[i].sourceType == resourceType && modifiers[i].sourceValue != 0)
                {
                    modifierValue += modifiers[i].sourceValue;
                }
                else if (modifiers[i].targetType == resourceType)
                {
                    modifierValue += modifiers[i].targetValue;
                }
            }
            else
            {
                if (modifiers[i].isEnabled)
                {
                    if (modifiers[i].sourceType == resourceType && modifiers[i].sourceValue != 0)
                    {
                        modifierValue += modifiers[i].sourceValue;
                    }
                    else if (modifiers[i].targetType == resourceType)
                    {
                        modifierValue += modifiers[i].targetValue;
                    }
                }
            }


        }
        if (resourceType == GameManager.ResourceType.POWER)
            modifierCount.text = modifierValue.ToString();
        else
            modifierCount.text = GameManager.instance.allPlayerSideDatas[playerSide].resourcesData[(int)resourceType].ToString() + "+" + modifierValue.ToString()+"/s";
    }
}
