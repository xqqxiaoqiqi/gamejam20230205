using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.GlobalIllumination;

public class Modifier
{

    public GameManager.PlayerSide playerSide;
    public GameManager.ResourceType sourceType;
    public GameManager.ResourceType targetType;
    public int sourceValue;
    public int targetValue;
    public bool isEnabled = true;


    public Modifier(GameManager.PlayerSide playerSide, GameManager.ResourceType sourceType,int sourceValue,GameManager.ResourceType targetType,int targetValue)
    {
        this.playerSide = playerSide;
        this.sourceType = sourceType;
        this.sourceValue = sourceValue;
        this.targetType = targetType;
        this.targetValue = targetValue;
        GameManager.instance.allPlayerSideDatas[playerSide].modifierSources.Add(this);
    }
    public bool OnApplyingModifier()
    {
        if (GameManager.instance.allPlayerSideDatas[playerSide].resourcesData[(int) sourceType] < sourceValue)
        {
            isEnabled = false;
            return false;
        }
        isEnabled = true;
        return true;
    }

    public void DoApplyModifier()
    {
        GameManager.instance.allPlayerSideDatas[playerSide].resourcesData[(int) sourceType] += sourceValue;
        GameManager.instance.allPlayerSideDatas[playerSide].resourcesData[(int) targetType] += targetValue;
    }
}
