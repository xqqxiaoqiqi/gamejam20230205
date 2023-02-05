using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventPanel : MonoBehaviour
{
    public GameEventDetailPanel gameEventDetailPanel;

    public void ShowGameEventDetailPanel()
    {
        if (GameManager.instance.isGameStarted)
        {
            gameEventDetailPanel.gameObject.SetActive(true);
        }
    }
    
    public void HideGameEventDetailPanel()
    {
        if (GameManager.instance.isGameStarted)
        {
            gameEventDetailPanel.gameObject.SetActive(false);
        }
    }
}
