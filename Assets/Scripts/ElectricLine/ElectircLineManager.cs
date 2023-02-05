using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElectircLineManager
{
    public static ElectircLineManager m_Instance = new Lazy<ElectircLineManager>(()=> new ElectircLineManager()).Value;

    private List<BasicBuilding> m_buildings;
    private List<int> m_nodeLayer;

    private List<LineRenderer> m_lines;
    private List<GameManager.PlayerSide> m_lineSides;


    public ElectircLineManager()
    {
        m_buildings = new List<BasicBuilding>();
        m_nodeLayer = new List<int>();
        m_lines = new List<LineRenderer>();
        m_lineSides = new List<GameManager.PlayerSide>();
    }

    public void ExtentBuilding(BasicBuilding building, GameManager.PlayerSide side)
    {
        if (!m_buildings.Contains(building))
        {
            
        }
    }
    



}
