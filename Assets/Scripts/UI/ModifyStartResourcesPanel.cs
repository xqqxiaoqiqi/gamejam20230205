using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyStartResourcesPanel : MonoBehaviour
{

    public GameManager.ResourceType resourceType;
    public CurrentDetailPanel currentDetailPanel;

    public int currValue
    {
        get
        {
            return currValue;
        }
        set
        {
            currValue = value;
            //todo: modify slider
        }
    }
    public Button addButton;
    public Button minusButton;
    public void OnInit()
    {
        currValue = 0;
    }

    public void UpdateCurrentPlayerSideData(int[] data)
    {
        currValue = data[(int) resourceType];
    }

    public void OnAddButtonClicked()
    {
        if (currentDetailPanel.currentPlayerSideMaxResourceValue[currentDetailPanel.currentPlayerSide] > 0)
        {
            currValue++;
            currentDetailPanel.currentPlayerSideResourcesValue[currentDetailPanel.currentPlayerSide][(int)resourceType]--;
        }
    }

    public void OnMinusButtonClicked()
    {
        if (currValue > 0)
        {
            currValue--;
            currentDetailPanel.currentPlayerSideResourcesValue[currentDetailPanel.currentPlayerSide][(int)resourceType]++;
        }
    }
    
    
}
