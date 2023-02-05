using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ElectricLineTest : MonoBehaviour
{
    public bool AddARandomBuilding = false;
    public int sideCount = -1;
    public bool Clearall = false;
    public List<Vector3Int> posCache;

    public GameObject man;
    public bool ins = false;
    public Tilemap map;

    private void OnValidate()
    {
        if (ins)
        {
            ins = false;
            if (map != null && man)
            {
                GameObject.Instantiate(man, map.CellToWorld(new Vector3Int(1,1,0)), Quaternion.identity);
            }
        }

        if (AddARandomBuilding)
        {
            AddARandomBuilding = false;
            sideCount++;
            sideCount = sideCount>=3? sideCount-3:sideCount;
            GameManager.PlayerSide side = GameManager.PlayerSide.SIDE_C;
            if (sideCount == 0)
            {
                side = GameManager.PlayerSide.SIDE_A;
            }
            else if (sideCount == 1)
            {
                side = GameManager.PlayerSide.SIDE_B;
            }
            Vector3Int p = new Vector3Int(Random.Range(0, 100), Random.Range(0, 100));
            while (posCache.Contains(p))
            {
                p = new Vector3Int(Random.Range(0, 100), Random.Range(0, 100));
            }
            posCache.Add(p);
            BasicBuilding bd = new BasicBuilding(side,BasicBuilding.BuildingType.BUILDING_BASE,p);
            ElectircLineManager.m_Instance.ExtentBuildingLines(bd,side);
        }

        if (Clearall)
        {
            Clearall = false;
            posCache.Clear();
            sideCount = -1;
            ElectircLineManager.m_Instance.ClearALLLines();
        }
    }
}
