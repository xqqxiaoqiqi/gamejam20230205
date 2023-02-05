using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class PlayerEventButton : MonoBehaviour
{
    public PlayerManager.PlayerEvent type;
    public GameEventDetailPanel Panel;
    public Transform highlightImage;

    private void Update()
    {
        UpdateHighLight();
    }

    public void OnShowDetail()
    {
        if (GameManager.instance.isGameStarted)
        {
            Panel.gameObject.SetActive(true);
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
        }
        else
        {
            highlightImage.gameObject.SetActive(false);
        }
    }
}
