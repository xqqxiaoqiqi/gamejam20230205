using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCGTerrain
{
    [CreateAssetMenu(menuName = "ProceduralTileTerrain/ProceduralTileRscMapAsset")]
    public class ProceduralTileResourcesMapAsset : ScriptableObject
    {
        public TileBase m_farmTile;
        public TileBase m_mineTile;
        public TileBase m_hotTile;
        public TileBase m_foodChestTile;
        public TileBase m_metalChestTile;
        public TileBase m_peopleChestTile;
        //
        public TileBase m_campTile;
        public TileBase m_relicTile;
        public TileBase m_gateTile;
        //
        public bool m_isAssetValid() => m_farmTile && m_mineTile && m_hotTile && m_foodChestTile && m_metalChestTile &&
                                 m_peopleChestTile && m_campTile
                                 && m_relicTile && m_gateTile;
    }
}

