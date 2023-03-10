using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDetailPanel : MonoBehaviour
{
    public Dictionary<GameManager.PlayerSide, int> currentPlayerSideMaxResourceValue = new Dictionary<GameManager.PlayerSide, int>();
    public Dictionary<GameManager.PlayerSide, int[]> currentPlayerSideResourcesValue = new Dictionary<GameManager.PlayerSide, int[]>();
    public ModifyStartResourcesPanel[] modifyStartResourcesPanel;
    public static GameManager.PlayerSide currentPlayerSide = GameManager.PlayerSide.SIDE_A;
    public GameStartPanel GameStartPanel;
    public Text totalCount;
    public Text remainCount;
    public Image currImage;
    public Sprite sideA;
    public Sprite sideB;
    public Sprite sideC;
    public Text currText;
    public void OnInit()
    {
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_A,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_B,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideMaxResourceValue.Add(GameManager.PlayerSide.SIDE_C,PlayerManager.instance.initModifierTotalCount);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_A,new int[(int)GameManager.ResourceType.ENUM]);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_B,new int[(int)GameManager.ResourceType.ENUM]);
        currentPlayerSideResourcesValue.Add(GameManager.PlayerSide.SIDE_C,new int[(int)GameManager.ResourceType.ENUM]);
        for (int i = 0; i < modifyStartResourcesPanel.Length; i++)
        {
            modifyStartResourcesPanel[i].OnInit();
        }
    }

    public void OnConfirmClicked()
    {
        UIManager.instance.BeginSelection(UIManager.SelectStatus.BASE);
        gameObject.SetActive(false);
    }

    public void SwitchCurrentPlayerSide(GameManager.PlayerSide newSide)
    {
        Debug.Log("PLAYRSIDE:" +newSide);
        currentPlayerSide = newSide;
        switch (newSide)
        {
            case GameManager.PlayerSide.SIDE_A:
                currImage.sprite = sideA;
                currText.text = "???????????????" + UIManager.instance.sideTexts[0];
                break;
            case GameManager.PlayerSide.SIDE_B:
                currImage.sprite = sideB;
                currText.text = "???????????????" + UIManager.instance.sideTexts[1];
                break;
            case GameManager.PlayerSide.SIDE_C:
                currImage.sprite = sideC;
                currText.text = "???????????????" + UIManager.instance.sideTexts[2];
                break;
            default:
                break;
            
        }
        for (int i = 0; i < modifyStartResourcesPanel.Length; i++)
        {
            modifyStartResourcesPanel[i].UpdateCurrentPlayerSideData(currentPlayerSideResourcesValue[currentPlayerSide]);
        }
    }

    public void ModifyCurrentResourcecsUI()
    {
        remainCount.text = currentPlayerSideMaxResourceValue[currentPlayerSide].ToString();
        totalCount.text = PlayerManager.instance.initModifierTotalCount.ToString();
    }

    public void InitAllModifiers()
    {
        foreach (var playersideModifier in currentPlayerSideResourcesValue)
        {
            var playerside = playersideModifier.Key;
            var modifiers = playersideModifier.Value;
            for (int i = 0; i < modifiers.Length; i++)
            {
                if (modifiers[i] != 0)
                {
                    if (i == (int) GameManager.ResourceType.FOOD)
                    {
                        new Modifier(playerside, GameManager.ResourceType.POWER, 0, (GameManager.ResourceType) i,
                            modifiers[i]*8);
                    }
                    else if(i == (int) GameManager.ResourceType.METAL)
                    {
                        new Modifier(playerside, GameManager.ResourceType.POWER, 0, (GameManager.ResourceType) i,
                            modifiers[i]*4);
                    }else if(i == (int) GameManager.ResourceType.POWER)
                    {
                        new Modifier(playerside, GameManager.ResourceType.POWER, 0, (GameManager.ResourceType) i,
                            modifiers[i]*100);
                    }
                    else
                    {
                        new Modifier(playerside, GameManager.ResourceType.POWER, 0, (GameManager.ResourceType) i,
                            modifiers[i]);
                    }

                }
            }
        }
    }
}
