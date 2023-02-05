using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public GameStartPanel gameStartPanel;
    public PlayerSideDataRootPanel playerSideDataRootPanel;
    public CoolDownTimer uiRefreshTimer = new CoolDownTimer(0);
    public bool selecting = true;
    public Vector2Int selectSize = new Vector2Int(2, 2);
    private List<Vector3Int> selectPos=new List<Vector3Int>();


    public void InitUIRoot()
    {
        gameStartPanel.OnInit();
    }

    public Vector3Int GetMousePos()
    {
        var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = TileManager.Instance.terrainMap.WorldToCell(target);
        pos[2] = 0;
        return pos;
    }

    public BasicBuilding GetMouseBuiling()
    {
        GameManager.instance.buildings.TryGetValue(GetMousePos(), out var building);
        return building;
    }

    public void OnTick()
    {
        playerSideDataRootPanel.UpdatePlayerSideResourceData();
        if (Input.GetMouseButton(1))
            Debug.Log("saosoa");
    }

    public Vector3Int showSelection()
    {
        if (selecting)
        {
            var curPos = GetMousePos();
            foreach(var pos in selectPos)
            {
                TileManager.Instance.selectionMap.SetTile(pos, null);
            }
            selectPos.Clear();

            for(int i = 0; i < selectSize.x; i++)
            {
                for(int j = 0; j < selectSize.y; j++)
                {
                    var pos = curPos + new Vector3Int(i, j, 0);
                    if (GameManager.instance.PosValid(pos))
                    {
                        if(TileManager.Instance.Reachable(pos))
                            TileManager.Instance.selectionMap.SetTile(pos, GameManager.instance.selectTile);
                        else
                            TileManager.Instance.selectionMap.SetTile(pos, GameManager.instance.selectFailTile);
                        selectPos.Add(pos);
                    }
                }
            }

        }
        return new Vector3Int(-1, -1, -1);
    }
}
