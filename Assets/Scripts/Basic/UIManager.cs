using Lean.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
    public GameEventPanel gameEventPanel;
    public PlayerContributeValuePanel playerCoutributePanel;
    public CoolDownTimer uiRefreshTimer = new CoolDownTimer(0);
    private bool preselect;
    private bool selecting = false;
    private SelectStatus selectStatus = SelectStatus.FLAG;
    private Vector2Int selectSize = new Vector2Int(1, 1);
    private List<Vector3Int> selectPos = new List<Vector3Int>();
    public GameObject panels;
    public GameObject basePanelSource;
    public GameObject resourcePanelSource;
    public GameObject enemyPanelSource;
    public LeanPinchCamera cam;
    public Sprite sideImage0;
    public Sprite sideImage1;
    public Sprite sideImage2;
    public string[] sideTexts=new string[]{"联盟", "安那其", "联邦" };
    public Sprite farmPanelSprite;
    public Sprite powerPanelSprite;
    public Sprite metalPanelSprite;
    public Sprite enemyPanelSprite;
    public Sprite factoryPanelSprite;

    public Button endImage;

    public Sprite EndSpriteCold;
    public Sprite EndSpritePoor;
    public Sprite EndSpriteOccupy;
    public Sprite EndSpriteContri;
    public Sprite EndSpriteWar;


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
        gameEventPanel.UpdateProcess(PlayerManager.instance.gameEventTimer.coolDown,PlayerManager.instance.gameEventTimer.current);
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

        if (cam.Zoom > 15)
            panels.SetActive(false);
        else
            panels.SetActive(true);

        if (selecting && Input.GetMouseButton(0))
        {
            if (selectStatus == SelectStatus.BASE)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    var building = new BasicBuilding(CurrentDetailPanel.currentPlayerSide, BasicBuilding.BuildingType.BUILDING_BASE, pos);
                    GameManager.instance.buildings.Add(pos, building);
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_BASE]);
                    ElectircLineManager.m_Instance.ExtentBuildingLines(building, building.playerSide);
                    EndSelection();
                    var obj = GameObject.Instantiate(UIManager.instance.basePanelSource);
                    var panel = obj.GetComponent<BasePanel>();
                    obj.transform.parent = UIManager.instance.panels.transform;
                    building.panel = obj.GetComponent<BuildingPanel>();
                    building.panel.OnInit(building);
                    building.panel.building = building;
                    if (CurrentDetailPanel.currentPlayerSide == GameManager.PlayerSide.SIDE_A)
                    {
                        panel.image.sprite = UIManager.instance.sideImage0;
                        panel.side.text = UIManager.instance.sideTexts[0];
                    }
                    else if (CurrentDetailPanel.currentPlayerSide == GameManager.PlayerSide.SIDE_B)
                    {
                        panel.image.sprite = UIManager.instance.sideImage1;
                        panel.side.text = UIManager.instance.sideTexts[1];
                    }
                    else if (CurrentDetailPanel.currentPlayerSide == GameManager.PlayerSide.SIDE_C)
                    {
                        panel.image.sprite = UIManager.instance.sideImage2;
                        panel.side.text = UIManager.instance.sideTexts[2];
                    }
                    OnInitBase(CurrentDetailPanel.currentPlayerSide);
                }
            }
            if (selectStatus == SelectStatus.FOODCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_FOODCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_FOODCHEST]);
                    EndSelection();
                }
            }
            if (selectStatus == SelectStatus.METALCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_METALCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_METALCHEST]);
                    EndSelection();
                }
            }
            if (selectStatus == SelectStatus.ENTITYCHEST)
            {
                var pos = GameManager.instance.selectPos;
                if (GameManager.instance.PosValid(pos))
                {
                    GameManager.instance.buildings.Add(pos, new BasicBuilding(GameManager.PlayerSide.NATURE, BasicBuilding.BuildingType.BUILDING_ENTITYCHEST, pos));
                    TileManager.Instance.buildingMap.SetTile(pos, GameManager.instance.buildingSource[(int)BasicBuilding.BuildingType.BUILDING_ENTITYCHEST]);
                    EndSelection();
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
                    EndSelection();
                }
            }
        }
    }

    public void GameStart()
    {
        gameStartPanel.gameObject.SetActive(false);
        playerSideDataRootPanel.gameObject.SetActive(true);
        PlayerContributeValuePanel.gameObject.SetActive(true);
        gameEventPanel.gameObject.SetActive(true);
        GameManager.instance.isGameStarted = true;
    }

    public void OnInitBase(GameManager.PlayerSide playerSide)
    {
        gameStartPanel.OnPlayerSideInitBase(playerSide);
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

    public string GetName(BasicBuilding.BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BasicBuilding.BuildingType.BUILDING_FACTORY:
                return "遗迹";
            case BasicBuilding.BuildingType.BUILDING_FOOD:
                return "农场";
            case BasicBuilding.BuildingType.BUILDING_METAL:
                return "矿厂";
            case BasicBuilding.BuildingType.BUILDING_POWER:
                return "电厂";
            default:
                return "NULL";
        }
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
