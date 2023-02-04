using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : MonoBehaviour
{
    public CurrentDetailPanel currentDetailPanel;

    public void OnInit()
    {
        currentDetailPanel.OnInit();
    }
}
