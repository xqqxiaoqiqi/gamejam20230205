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
            //�����ǰû��ʵ������
            if (s_instance == null)
            {
                s_instance = new TileManager();
                s_instance.Init();
            }
            //�����ʵ��������ֱ�ӷ��ء�
            return s_instance;
        }
    }
    public void Init()
    {
        terrainMap = GameObject.Find("TerrainMap").GetComponent<Tilemap>();
        buildingMap = GameObject.Find("BuildingMap").GetComponent<Tilemap>();
    }

    public Tilemap terrainMap;
    public Tilemap buildingMap;



}
