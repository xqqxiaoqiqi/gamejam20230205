using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventPanel : MonoBehaviour
{
    public GameEventDetailPanel gameEventDetailPanel;
    public Slider Slider;

    public void UpdateProcess(int total,int current)
    {
        Slider.value = (float) current / total;
    }
    
    public void ShowGameEventDetailPanel()
    {
        if (GameManager.instance.isGameStarted)
        {
            gameEventDetailPanel.gameObject.SetActive(true);
            gameEventDetailPanel.myText.text = PlayerManager.instance.gameEventDatas[PlayerManager.instance.currEventIndex].description;
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
