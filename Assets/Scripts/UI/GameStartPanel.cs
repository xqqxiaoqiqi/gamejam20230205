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
            gameObject.SetActive(false);
            GameManager.instance.isGameStarted = true;
        }
        else
        {
            currentDetailPanel.gameObject.SetActive(true);
            for (int i = 1; i < 4; i++)
            {
                if (!playerSideInited[i])
                {
                    currentDetailPanel.SwitchCurrentPlayerSide((GameManager.PlayerSide)i);
                    return;
                }
            }
        }
    }
    
}
