using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuildingPanel : MonoBehaviour
{

    public Text side;
    public Image image;
    public BasicBuilding building;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (building != null)
        {
            if(building.buildType==BasicBuilding.BuildingType.BUILDING_BASE)
                transform.position = Camera.main.WorldToScreenPoint(TileManager.Instance.terrainMap.CellToWorld(building.pos+new Vector3Int(3,3,0)))+new Vector3(0,80,0);
            else
                transform.position = Camera.main.WorldToScreenPoint(TileManager.Instance.terrainMap.CellToWorld(building.pos + new Vector3Int(1, 1, 0))) + new Vector3(0, 50, 0);
        }
    }
}

    public class BasePanel : BuildingPanel
{
    public Text capability;
    public Text level;
}
