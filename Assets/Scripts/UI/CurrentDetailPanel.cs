using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDetailPanel : MonoBehaviour
{
    public Dictionary<GameManager.PlayerSide, int> currentPlayerSideMaxResourceValue = new Dictionary<GameManager.PlayerSide, int>();
    public Dictionary<GameManager.PlayerSide, int[]> currentPlayerSideResourcesValue = new Dictionary<GameManager.PlayerSide, int[]>();
    public ModifyStartResourcesPanel[] modifyStartResourcesPanel;
    public GameManager.PlayerSide currentPlayerSide;
    public Text totalCount;
    public Text remainCount;
    public void OnInit()
    {
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_A,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_B,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_C,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_A,new int[(int)GameManager.ResourceType.ENUM]);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_B,new int[(int)GameManager.ResourceType.ENUM]);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_C,new int[(int)GameManager.ResourceType.ENUM]);
        for (int i = 0; i < modifyStartResourcesPanel.Length; i++)
        {
            modifyStartResourcesPanel[i].OnInit();
        }
    }

    public void OnConfirmClicked()
    {
        //switch to build base state
    }

    public void SwitchCurrentPlayerSide(GameManager.PlayerSide newSide)
    {
        currentPlayerSide = newSide;
        for (int i = 0; i < modifyStartResourcesPanel.Length; i++)
        {
            modifyStartResourcesPanel[i].UpdateCurrentPlayerSideData(currentPlayerSideResourcesValue[currentPlayerSide]);
        }
    }

    public void ModifyCurrentResourcecsUI()
    {
        remainCount.text = currentPlayerSideMaxResourceValue[currentPlayerSide].ToString();
        totalCount.text = PlayerManager.instance.initModifierTotalCount.ToString();
    }
}
