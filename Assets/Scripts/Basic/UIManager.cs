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
        ENTITYCHEST,
        FLAG,
    }
    public GameStartPanel gameStartPanel;
    public PlayerContributeValuePanel PlayerContributeValuePanel;
    public PlayerSideDataRootPanel playerSideDataRootPanel;
    public CoolDownTimer uiRefreshTimer = new CoolDownTimer(0);
    private bool preselect;
    private bool selecting = true;
    private SelectStatus selectStatus = SelectStatus.FLAG;
    private Vector2Int selectSize = new Vector2Int(1, 1);
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
                }
            }
            if (selectStatus == SelectStatus.FOODCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_FOODCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_FOODCHEST]);
                }
            }
            if (selectStatus == SelectStatus.METALCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_METALCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_METALCHEST]);
                }
            }
            if (selectStatus == SelectStatus.ENTITYCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_ENTITYCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_ENTITYCHEST]);
                }
            }
            if (selectStatus == SelectStatus.FLAG)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.TryGetValue(pos, out var building);
                    if (!building.targeted)
                    {
                        Entity nearestEntity = null;
                        float neardis = 0;
                        foreach(var entity in GameManager.instance.Entities)
                        {
                            var entitypos = TileManager.Instance.terrainMap.WorldToCell(entity.transform.position);
                            entitypos[2] = 0;
                            if (nearestEntity == null || Vector3Int.Distance(entitypos, pos) < neardis)
                            {
                                nearestEntity = entity;
                                neardis = Vector3Int.Distance(entitypos, pos);
                            }
                        }
                        if (nearestEntity != null)
                        {
                            nearestEntity.targetBuilding.targeted = false;
                            building.targeted = true;
                            nearestEntity.targetBuilding = building;
                            nearestEntity.MoveTo(pos);
                        }
                    }

                }
            }
            EndSelection();
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
            if (selectStatus == SelectStatus.FLAG)
            {
                var curPos = GetMousePos();
                foreach (var pos in selectPos)
                {
                    TileManager.Instance.selectionMap.SetTile(pos, null);
                }
                selectPos.Clear();

                if (GameManager.instance.PosValid(curPos))
                {
                    GameManager.instance.buildings.TryGetValue(curPos, out var building);
                    if (building!=null)
                    {
                        if(building.playerSide==GameManager.PlayerSide.NATURE)
                            TileManager.Instance.selectionMap.SetTile(curPos, GameManager.instance.selectTile);
                        selectPos.Add(curPos);
                        return curPos;
                    }
                    else
                    {
                        TileManager.Instance.selectionMap.SetTile(curPos, GameManager.instance.selectFailTile);
                        selectPos.Add(curPos);
                    }
                }
            }
            else
            {
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
        }
        return new Vector3Int(-1, -1, -1);
    }
}
