using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public enum SelectStatus
    {
        BASE,
        FOODCHEST,
        METALCHEST,
        ENTITYCHEST
    }
    public GameStartPanel gameStartPanel;
    public PlayerContributeValuePanel PlayerContributeValuePanel;
    public PlayerSideDataRootPanel playerSideDataRootPanel;
    public CoolDownTimer uiRefreshTimer = new CoolDownTimer(0);
    private bool preselect;
    private bool selecting = false;
    public SelectStatus selectStatus = SelectStatus.BASE;
    public Vector2Int selectSize = new Vector2Int(3, 3);
    private List<Vector3Int> selectPos = new List<Vector3Int>();


    public void InitUIRoot()
    {
        gameStartPanel.OnInit();
    }

    public void UpdateContributeValue()
    {
        PlayerContributeValuePanel.UpdateValue();
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
    }

    private void Update()
    {
        if (preselect)
        {
            if (Input.GetMouseButtonUp(0))
            {
                selecting = true;
                preselect = false;
            }
        }

        if (selecting && Input.GetMouseButton(0))
        {
            if (selectStatus == SelectStatus.BASE)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(CurrentDetailPanel.currentPlayerSide, BasicBuilding.BuildingType.BUILDING_BASE, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_BASE]);
                    EndSelection();
                }
            }
        }
    }

    public void BeginSelection(SelectStatus status)
    {
        preselect = true;
        selectStatus = status;
        selectSize = new Vector2Int(1, 1);
        if (selectStatus == SelectStatus.BASE)
            selectSize = new Vector2Int(3, 3);
    }


    public void EndSelection()
    {
        foreach (var pos in selectPos)
        {
            TileManager.Instance.selectionMap.SetTile(pos, null);
        }
        selectPos.Clear();
        selecting = false;
    }

    public Vector3Int showSelection()
    {
        if (selecting)
        {
            bool success = true;
            var curPos = GetMousePos();
            foreach (var pos in selectPos)
            {
                TileManager.Instance.selectionMap.SetTile(pos, null);
            }
            selectPos.Clear();

            for (int i = 0; i < selectSize.x; i++)
            {
                for (int j = 0; j < selectSize.y; j++)
                {
                    var pos = curPos + new Vector3Int(i, j, 0);
                    if (GameManager.instance.PosValid(pos))
                    {
                        if (TileManager.Instance.Reachable(pos))
                        {
                            TileManager.Instance.selectionMap.SetTile(pos, GameManager.instance.selectTile);
                        }
                        else
                        {
                            success = false;
                            TileManager.Instance.selectionMap.SetTile(pos, GameManager.instance.selectFailTile);
                        }
                        selectPos.Add(pos);
                    }
                }
            }
            if (success)
                return curPos;
        }
        return new Vector3Int(-1, -1, -1);
    }
}
