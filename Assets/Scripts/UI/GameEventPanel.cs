using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GameEventPanel : MonoBehaviour
{
    public GameEventDetailPanel gameEventDetailPanel;
    public Slider Slider;
    public Text eventName;

    public void UpdateProcess(int total,int current)
    {
        Slider.value = (float) current / total;
        if (PlayerManager.instance.currEventIndex<PlayerManager.instance.gameEventDatas.Count)
        {
        eventName.text = PlayerManager.instance.gameEventDatas[PlayerManager.instance.currEventIndex].name;
        }
        else
        {
            eventName.text = "没有了";
        }
    }
    
    public void ShowGameEventDetailPanel()
    {
        if (GameManager.instance.isGameStarted)
        {
            if (PlayerManager.instance.currEventIndex<PlayerManager.instance.gameEventDatas.Count)
            {
                gameEventDetailPanel.gameObject.SetActive(true);
                gameEventDetailPanel.myText.text = PlayerManager.instance.gameEventDatas[PlayerManager.instance.currEventIndex].description;
            }
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
