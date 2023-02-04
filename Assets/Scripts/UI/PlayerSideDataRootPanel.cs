using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSideDataRootPanel : MonoBehaviour
{
    public PlayerSideDataResourceDetailPanel[] playerSideDataResourceDetailPanels;
    
    public void UpdatePlayerSideResourceData()
    {
        for (int i = 0; i < playerSideDataResourceDetailPanels.Length; i++)
        {
            playerSideDataResourceDetailPanels[i].UpdatePlayerSideDataResources();
        }
    }
}
