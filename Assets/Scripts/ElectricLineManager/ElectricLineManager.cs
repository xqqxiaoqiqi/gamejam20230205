using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElectircLineManager : IDisposable
{
    public static ElectircLineManager m_Instance = new Lazy<ElectircLineManager>(()=> new ElectircLineManager()).Value;

    private List<BasicBuilding> m_buildings;
    private List<int> m_nodeLayer;

    private List<LineRenderer> m_lines;
    private List<GameManager.PlayerSide> m_lineSides;

    private Material lineMat;
    private Material mat_A, mat_B, mat_C;
    public int maxLayer_A, maxLayer_B, maxLayer_C;

    private Tilemap m_terrainMap;
    private bool m_enable = false;

    private const float maxLineWidth = 0.05f;
    private const float minLineWidth = 0.01f;


    public ElectircLineManager()
    {
        m_buildings = new List<BasicBuilding>();
        m_nodeLayer = new List<int>();
        m_lines = new List<LineRenderer>();
        m_lineSides = new List<GameManager.PlayerSide>();
        //
        if (TileManager.Instance != null)
        {
            m_terrainMap = TileManager.Instance.terrainMap;
        }
        else
        {
            var terrainObj = GameObject.Find("TerrainMap");
            if (terrainObj)
            {
                terrainObj.TryGetComponent<Tilemap>(out m_terrainMap);
            }
        }

        if (m_terrainMap != null)
        {
            m_enable = true;
        }
        //
        lineMat = Resources.Load<Material>("Materials/LineMat");

        mat_A = Material.Instantiate(lineMat);
        mat_B = Material.Instantiate(lineMat);
        mat_C= Material.Instantiate(lineMat);
        mat_A.SetColor("_Color", new Color(200.0f / 255, 22.0f / 255, 22.0f / 255));
        mat_B.SetColor("_Color", new Color(201.0f / 255, 200.0f / 255, 6.0f / 255));
        mat_C.SetColor("_Color",new Color(3.0f/255,51.0f/255,150.0f/255));
        maxLayer_A = 0;
        maxLayer_B = 0;
        maxLayer_C = 0;
        
    }

    public void ExtentBuildingLines(BasicBuilding building, GameManager.PlayerSide side)
    {
        if (m_enable == false)
        {
            return;
        }

        if (!m_buildings.Contains(building))
        {
            int nearestIndex = GetNearestPlayerSideBuilding(building, side);
            if (nearestIndex < 0)
            {
                m_buildings.Add(building);
                m_nodeLayer.Add(0);
            }
            else
            {
                var nearestBuilding = m_buildings[nearestIndex];
                m_buildings.Add(building);
                m_nodeLayer.Add(m_nodeLayer[nearestIndex] + 1);
                //Create New Line
                GameObject newLine =
                    new GameObject(
                        $"{nearestBuilding.playerSide}_{nearestBuilding.buildType} ----- new {building.buildType}");
                var LineRenderer = newLine.AddComponent<LineRenderer>();
                var posOld = GetbuildingPosWS(nearestBuilding.pos);
                if(nearestBuilding.buildType==BasicBuilding.BuildingType.BUILDING_BASE|| nearestBuilding.buildType == BasicBuilding.BuildingType.BASE1|| nearestBuilding.buildType == BasicBuilding.BuildingType.BASE2)
                    posOld= GetbuildingPosWS(nearestBuilding.pos+new Vector3Int(3,3,0));
                var posNew = GetbuildingPosWS(building.pos);
                Vector3[] posList = new Vector3[]
                {
                    posOld,
                    posNew
                };
                LineRenderer.SetPositions(posList);
                if (side == GameManager.PlayerSide.SIDE_A)
                {
                    LineRenderer.material = mat_A;
                    maxLayer_A = Mathf.Max(maxLayer_A, m_nodeLayer[nearestIndex] + 1);
                    
                }
                else if (side == GameManager.PlayerSide.SIDE_B)
                {
                    LineRenderer.material = mat_B;
                    maxLayer_B = Mathf.Max(maxLayer_B, m_nodeLayer[nearestIndex] + 1);
                }
                else
                {
                    LineRenderer.material = mat_C;
                    maxLayer_C = Mathf.Max(maxLayer_C, m_nodeLayer[nearestIndex] + 1);
                }
                float startWidth = (1 - Mathf.Min(m_nodeLayer[nearestIndex]/4.0f,1))*(maxLineWidth-minLineWidth) + minLineWidth;
                float endWidth = (1 - Mathf.Min((m_nodeLayer[nearestIndex]+1)/4.0f,1))*(maxLineWidth-minLineWidth) + minLineWidth;
                LineRenderer.widthCurve.keys = new Keyframe[]
                {
                    new Keyframe(0,startWidth),
                    new Keyframe(1,endWidth)
                };
                LineRenderer.startWidth = startWidth;
                LineRenderer.endWidth = endWidth;
                m_lines.Add(LineRenderer);
                m_lineSides.Add(nearestBuilding.playerSide);
                building.playerSide = nearestBuilding.playerSide;
            }
        }
    }

    public void ClearALLLines()
    {
        m_buildings.Clear();
        m_nodeLayer.Clear();
        for (int i = 0; i < m_lines.Count; i++)
        {
            #if UNITY_EDITOR
            GameObject.DestroyImmediate(m_lines[i].gameObject);
            #else
            GameObject.Destroy(m_lines[i].gameObject);
            #endif
        }
        m_lines.Clear();
        m_lineSides.Clear();
    }

    public void Dispose()
    {
        ClearALLLines();
        //让GC去做吧 我懒了= =
        mat_B = null;
        mat_A = null;
        mat_C = null;
        lineMat = null;
    }

    int GetNearestPlayerSideBuilding(BasicBuilding building, GameManager.PlayerSide side)
    {
        if (m_buildings.Contains(building))
        {
            return -1;
        }

        int nearestIndex = -1;
        for (int i = m_buildings.Count - 1 ; i >-1; i--)
        {
            if (m_buildings[i].playerSide == side)
            {
                if (nearestIndex < 0)
                {
                    nearestIndex = i;
                }
                else
                {
                    var curNearestCellPos = m_buildings[nearestIndex].pos;
                    var curBuildingCellPos = m_buildings[i].pos;
                    var newBuildingCellPos = building.pos;
                    if ((curNearestCellPos - newBuildingCellPos).magnitude >
                        (curBuildingCellPos - newBuildingCellPos).magnitude)
                    {
                        nearestIndex = i;
                    }
                }
            }
        }
        return nearestIndex;
    }
    
    Vector3 GetbuildingPosWS(Vector3Int p)
    {
        var result = 0.5f * (m_terrainMap.CellToWorld(p + new Vector3Int(2, 2, 0)) + m_terrainMap.CellToWorld(p + new Vector3Int(1, 1, 0)));
        result.z = -1;
        return result;
    }




}