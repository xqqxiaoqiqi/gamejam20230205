using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyStartResourcesPanel : MonoBehaviour
{

    public GameManager.ResourceType resourceType;
    public CurrentDetailPanel currentDetailPanel;
    public Slider Slider;
    private int m_currValue;
    public int currValue
    {
        get
        {
            return m_currValue;
        }
        set
        {
            m_currValue = value;
            Slider.value = (float)m_currValue / PlayerManager.instance.initModifierTotalCount;
            currentDetailPanel.ModifyCurrentResourcecsUI();
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
            currentDetailPanel.currentPlayerSideResourcesValue[currentDetailPanel.currentPlayerSide][(int)resourceType]++;
            currentDetailPanel.currentPlayerSideMaxResourceValue[currentDetailPanel.currentPlayerSide]--;
            currValue++;
        }
    }

    public void OnMinusButtonClicked()
    {
        if (currValue > 0)
        {
            currentDetailPanel.currentPlayerSideResourcesValue[currentDetailPanel.currentPlayerSide][(int)resourceType]--;
            currentDetailPanel.currentPlayerSideMaxResourceValue[currentDetailPanel.currentPlayerSide]++;
            currValue--;
        }
    }
    
    
}
