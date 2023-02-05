using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventButton : MonoBehaviour
{
    public PlayerManager.PlayerEvent type;
    public GameEventDetailPanel Panel;

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
}
