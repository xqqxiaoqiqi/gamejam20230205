using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideDataResourceDetailPanel : MonoBehaviour
{
    public GameManager.PlayerSide playerSide;

    public ResourceDataPanel[] resourceDataPanels;

    public void UpdatePlayerSideDataResources()
    {
        for (int i = 0; i < resourceDataPanels.Length; i++)
        {
            resourceDataPanels[i].UpdateResourceData(playerSide);
        }
    }
}
