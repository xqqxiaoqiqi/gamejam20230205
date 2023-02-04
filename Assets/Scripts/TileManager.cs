using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileManager
{
    private static TileManager s_instance;
    public static TileManager Instance
    {
        get
        {
            //如果当前没有实例引用
            if (s_instance == null)
            {
                s_instance = new TileManager();
                s_instance.Init();
            }
            //如果有实例引用则直接返回。
            return s_instance;
        }
    }
    public void Init()
    {
        terrainMap = GameObject.Find("TerrainMap").GetComponent<Tilemap>();
        buildingMap = GameObject.Find("BuildingMap").GetComponent<Tilemap>();
        selectionMap = GameObject.Find("SelectionMap").GetComponent<Tilemap>();

    }

    public Tilemap terrainMap;
    public Tilemap buildingMap;
    public Tilemap selectionMap;

    public bool Reachable(Vector3Int pos)
    {
        if (!GameManager.instance.PosValid(pos))
            return false;
        var terrainTile = TileManager.Instance.terrainMap.GetTile(pos) as Tile;
        var buildingTile = TileManager.Instance.buildingMap.GetTile(pos) as Tile;
        if (terrainTile != null && terrainTile.colliderType != Tile.ColliderType.Grid &&
            (buildingTile == null || buildingTile.colliderType != Tile.ColliderType.Grid))
        {
            return true;
        }
        else return false;
    }
}
