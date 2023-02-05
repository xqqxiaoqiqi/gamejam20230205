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
        CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_A);
    }
    public void OnPlayerSideBClicked()
    {
        CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_B);
    }    public void OnPlayerSideCClicked()
    {
        CurrentDetailPanel.SwitchCurrentPlayerSide(GameManager.PlayerSide.SIDE_C);
    }
    
}
