using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSideSelectPanel : MonoBehaviour
{
    public Button playerSideA;
    public Button playerSIdeB;
    public Button playerSIdeC;
    public CurrentDetailPanel CurrentDetailPanel;

    public void OnPlayerSideAClicked()
    {
        if(!CurrentDetailPanel.GameStartPanel.playerSideInited[(int)GameManager.PlayerSide.SIDE_A])
        {
            CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_A);
        }
    }
    public void OnPlayerSideBClicked()
    {
        if (!CurrentDetailPanel.GameStartPanel.playerSideInited[(int) GameManager.PlayerSide.SIDE_B])
        {
            CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_B);
        }
    }    public void OnPlayerSideCClicked()
    {
        if (!CurrentDetailPanel.GameStartPanel.playerSideInited[(int) GameManager.PlayerSide.SIDE_C])
        {
            CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_C);
        }
    }
    
}
