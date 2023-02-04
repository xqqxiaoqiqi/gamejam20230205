using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PCGTerrain
{
    [CreateAssetMenu(menuName = "ProceduralTileTerrain/ProceduralTileTerrainMapAsset")]
    public class ProceduralTileTerrainMapAsset : ScriptableObject
    {
        public TileBase m_mountTile;
        public TileBase m_hillTile;
        public TileBase m_normalTile;
        public TileBase m_waterTile;
        public TileBase m_sandTile;
        public TileBase m_iceTile;
        public TileBase m_forestTile;
        public TileBase m_marshTile;

        public bool isAssetValid() => m_mountTile && m_hillTile && m_normalTile && m_waterTile && m_sandTile && m_iceTile &&
                                      m_forestTile && m_marshTile;
    }
}


