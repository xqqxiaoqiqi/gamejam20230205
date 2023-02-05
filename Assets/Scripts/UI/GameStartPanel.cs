using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : MonoBehaviour
{
    public CurrentDetailPanel currentDetailPanel;
    public bool[] playerSideInited = new bool[(int) GameManager.PlayerSide.ENUM];

    public bool IsAllPlayerSideReady
    {
        get { return playerSideInited[1] && playerSideInited[2] && playerSideInited[3]; }
    }
    public void OnInit()
    {
        currentDetailPanel.OnInit();
    }

    public void OnPlayerSideInitBase(GameManager.PlayerSide playerSide)
    {
        playerSideInited[(int) playerSide] = true;
        if (IsAllPlayerSideReady)
        {
            currentDetailPanel.InitAllModifiers();
        }
        GameManager.instance.isGameStarted = true;
        //todo: hide me and GAME ON!
    }

    public void Test()
    {
        currentDetailPanel.InitAllModifiers();
        GameManager.instance.isGameStarted = true;
    }
}
