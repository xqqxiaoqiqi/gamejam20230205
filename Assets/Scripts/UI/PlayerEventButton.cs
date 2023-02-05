using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;

public class PlayerEventButton : MonoBehaviour
{
    public PlayerManager.PlayerEvent type;
    public PlayerEventDetailPanel Panel;
    public Transform highlightImage;
    private bool isHighlight;

    private void Update()
    {
        UpdateHighLight();
    }

    public void OnShowDetail()
    {
        if (GameManager.instance.isGameStarted)
        {
            if (PlayerManager.instance.playerEventDatas.ContainsKey(type))
            {
                Panel.gameObject.SetActive(true);
                Panel.myText.text = PlayerManager.instance.playerEventDatas[type].description;
            }
        }
    }
    
    public void OnHideDetail()
    {
        if (GameManager.instance.isGameStarted)
        {
            Panel.gameObject.SetActive(false);
        }
    }

    public void UpdateHighLight()
    {
        if (GameManager.instance.isGameStarted&& PlayerManager.instance.CheckPlayerEventValid(type))
        {
            highlightImage.gameObject.SetActive(true);
            isHighlight = true;
        }
        else
        {
            highlightImage.gameObject.SetActive(false);
            isHighlight = false;
        }
    }

    public void OnPlayerEventClicked()
    {
        if (isHighlight)
        {
            PlayerManager.instance.OnSelectingTile(type);
        }
    }
}
