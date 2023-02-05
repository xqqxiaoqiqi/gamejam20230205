using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BuildingPanel : MonoBehaviour
{

    public Text side;
    public Image image;
    public BasicBuilding building;
    
    public virtual void FixedUpdate()
    {
        if (building != null)
        {
            if(building.buildType==BasicBuilding.BuildingType.BUILDING_BASE)
                transform.position = Camera.main.WorldToScreenPoint(TileManager.Instance.terrainMap.CellToWorld(building.pos+new Vector3Int(3,3,0)))+new Vector3(0,80,0);
            else
                transform.position = Camera.main.WorldToScreenPoint(TileManager.Instance.terrainMap.CellToWorld(building.pos + new Vector3Int(1, 1, 0))) + new Vector3(0, 50, 0);
        }
    }
    
    public virtual void OnInit(object args)
    {
        
    }
}

    public class BasePanel : BuildingPanel
{
    public Text capability;
    public Text level;
    public BasicBuilding myBuilding;


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (myBuilding != null&&myBuilding.buildType == BasicBuilding.BuildingType.BUILDING_BASE)
        {
            level.text = "LV." + myBuilding.buildingLevel;
            capability.text = (PlayerManager.instance.basicCapability+GameManager.instance.allPlayerSideDatas[myBuilding.playerSide].resourcesData[(int) GameManager.ResourceType.ENTITY_CAPABILITY]).ToString();
        }
    }

    public override void OnInit(object args)
    {
        base.OnInit(args);
        var building = args as BasicBuilding;
        myBuilding = building;
        if (myBuilding != null&&myBuilding.buildType == BasicBuilding.BuildingType.BUILDING_BASE)
        {
            level.text = "LV." + myBuilding.buildingLevel;
            capability.text = (PlayerManager.instance.basicCapability+GameManager.instance.allPlayerSideDatas[myBuilding.playerSide].resourcesData[(int) GameManager.ResourceType.ENTITY_CAPABILITY]).ToString();
        }

    }
}
