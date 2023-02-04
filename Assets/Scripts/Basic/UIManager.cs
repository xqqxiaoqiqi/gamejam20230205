using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public GameStartPanel gameStartPanel;
    public PlayerSideDataRootPanel playerSideDataRootPanel;
    public CoolDownTimer uiRefreshTimer = new CoolDownTimer(30);

    public void OnTick()
    {
        uiRefreshTimer.OnTick();
        if (uiRefreshTimer.isReady)
        {
            playerSideDataRootPanel.UpdatePlayerSideResourceData();
            uiRefreshTimer.Reset();
        }
    }
}
