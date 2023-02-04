using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Aoiti.Pathfinding;
public class Entity : MonoBehaviour
{
    public int capability;
    public float movespeed;
    public GameManager.PlayerSide playerSide;
    public Behaviour[] behaviours;


    Vector3Int targetPos;
    BasicBuilding targetBuilding;

    Vector3Int[] directions = new Vector3Int[4] { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

    Pathfinder<Vector3Int> pathfinder;

    public List<Vector3Int> path;
    public bool isFinished;

    public void OnInit(GameManager.PlayerSide playerSide)
    {
        this.playerSide = playerSide;
    }

    private void Start()
    {
        pathfinder = new Pathfinder<Vector3Int>(DistanceFunc, connectionsAndCosts);
        GameManager.instance.Entities.Add(this);
    }

    public void OnTick()
    {
        if (targetBuilding == null|| targetBuilding.isDestroyed==true)
        {
            SearchTargetBuilding();
        }
    }

    public void SearchTarget()
    {
        
    }


    public void MoveTo(Vector3Int target)
    {
        var currentCellPos = TileManager.Instance.terrainMap.WorldToCell(transform.position);
        target.z = 0;
        targetPos = target;
        pathfinder.GenerateAstarPath(currentCellPos, target, out path);
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    public void MoveTo(Vector3 targetPos)
    {
        MoveTo(TileManager.Instance.terrainMap.WorldToCell(targetPos));
    }

    public float DistanceFunc(Vector3Int a, Vector3Int b)
    {
        return (a - b).sqrMagnitude;
    }


    public Dictionary<Vector3Int, float> connectionsAndCosts(Vector3Int a)
    {
        Dictionary<Vector3Int, float> result = new Dictionary<Vector3Int, float>();
        foreach (Vector3Int dir in directions)
        {
            var terrainTile = TileManager.Instance.terrainMap.GetTile(a + dir) as Tile;
            var buildingTile = TileManager.Instance.buildingMap.GetTile(a + dir) as Tile;
            if (terrainTile != null && terrainTile.colliderType != Tile.ColliderType.Grid &&
                (buildingTile == null || buildingTile.colliderType != Tile.ColliderType.Grid) || (a + dir) == targetPos)
            {
                if (terrainTile.colliderType == Tile.ColliderType.None)
                    result.Add(a + dir, 10);
                else
                    result.Add(a + dir, 20);
            }
        }
        return result;
    }


    IEnumerator Move()
    {
        while (path.Count > 0)
        {
            var currentPos = TileManager.Instance.terrainMap.WorldToCell(transform.position);
            var targetPos = TileManager.Instance.terrainMap.CellToWorld(path[0]);
            if ((TileManager.Instance.terrainMap.GetTile(currentPos) as Tile).colliderType == Tile.ColliderType.Sprite)
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.fixedDeltaTime * movespeed * 0.5f);
            else
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.fixedDeltaTime * movespeed);
            if (Vector3.SqrMagnitude(transform.position - targetPos) < 0.01f)
            {
                path.RemoveAt(0);
                if (path.Count == 0 && targetBuilding != null)
                    targetBuilding.OnEntityEnter(this);
            }
            yield return null;
        }
    }

    public void MarkFinish()
    {
        isFinished = true;
    }

    private void SearchTargetBuilding()
    {
        var currentPos = TileManager.Instance.terrainMap.WorldToCell(transform.position);
        for (int circle = 1; circle < 50; circle++)
        {
            for (int i = -circle; i < circle; i++)
            {
                var pos = new Vector3Int(currentPos.x + i, currentPos.y - circle, 0);
                if (CheckBuildingCennect(currentPos, pos))
                {
                    targetBuilding = GameManager.instance.buildings[pos];
                    targetBuilding.targeted = true;
                    MoveTo(pos);
                    return;
                }
            }
            for (int i = -circle; i < circle; i++)
            {
                var pos = new Vector3Int(currentPos.x + circle, currentPos.y + i, 0);
                if (CheckBuildingCennect(currentPos,pos))
                {
                    targetBuilding = GameManager.instance.buildings[pos];
                    targetBuilding.targeted = true;
                    MoveTo(pos);
                    return;
                }
            }
            for (int i = circle; i > -circle; i--)
            {
                var pos = new Vector3Int(currentPos.x + i, currentPos.y + circle, 0);
                if (CheckBuildingCennect(currentPos, pos))
                {
                    targetBuilding = GameManager.instance.buildings[pos];
                    targetBuilding.targeted = true;
                    MoveTo(pos);
                    return;
                }
            }
            for (int i = circle; i > -circle; i--)
            {
                var pos = new Vector3Int(currentPos.x - circle, currentPos.y + i, 0);
                if (CheckBuildingCennect(currentPos, pos))
                {
                    targetBuilding = GameManager.instance.buildings[pos];
                    targetBuilding.targeted = true;
                    MoveTo(pos);
                    return;
                }
            }
        }
    }

    private bool CheckBuildingCennect(Vector3Int currentPos, Vector3Int pos)
    {
        if (!GameManager.instance.PosValid(pos))
            return false;
        targetPos = pos;
        if (GameManager.instance.buildings.ContainsKey(pos) && !GameManager.instance.buildings[pos].targeted && pathfinder.GenerateAstarPath(currentPos, pos, out path))
        {

            return true;
        }
        return false;
    }
}
