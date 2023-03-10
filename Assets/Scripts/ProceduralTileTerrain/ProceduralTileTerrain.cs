using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using static Unity.Mathematics.math;

namespace PCGTerrain
{
    public class ProceduralTileTerrain : MonoBehaviour
    {
        public Tilemap m_tileMap;
        public Tilemap m_rscTileMap;
        public Material m_proceduralMat;

        public ProceduralTileTerrainMapAsset m_mapAsset;

        public ProceduralTileResourcesMapAsset m_rscMapAsset;
        //Procedural Params
        //consts
        const float m_noisePersist = 0.5f;

        private const float m_spNoiseFac = 0.086f;

        private const float m_spNoiseScale = 6.03f;
        //vars
        [Header("基本地形")]
        public Vector2 m_offset = Vector2.zero;
        [Range(5,15)]public float m_noiseFreq = 11f;
        [Range(1,3)]public int m_noiseSteps = 2;
        [Range(1, 5)] public int m_hundredTileCount_x = 1;
        [Range(1,5)]public int m_hundredTileCount_y = 1;
        /// <summary>
        /// 地形大形状噪声
        /// </summary>
        [Range(0.1f,12)]public float m_largeScale = 3;

        /// <summary>
        /// 地形影响程度
        /// </summary>
        [Range(0,10)]public float m_largeFac = 1.11f;

        /// <summary>
        /// 小形状影响程度
        /// </summary>
        [Range(0,0.5f)]public float m_detailFac = 0.2f;
        
        //正常地形颜色
        public Color m_color_Mount = Color.black;
        [Range(0.8f,1)]public float m_Mount = 0.88f;
        
        public Color m_color_Hill = Color.gray;
        [Range(0.4f,0.8f)]public float m_Hill = 0.623f;
        
        public Color m_color_NormalHeight = Color.white;
        [Range(0,0.6f)]public float m_NormalHeight = 0.25f;
        
        public Color m_color_Water = Color.cyan;
        
        //沙漠与冰川
        public Color m_color_Sand = Color.yellow;
        [Range(0.6f,1)]public float m_Sand = 0.89f;
        
        public Color m_color_Ice = Color.blue;
        [Range(0,0.4f)]public float m_Ice = 0.09f;
        
        public Color m_color_Marsh = Color.magenta;
        public Color m_color_Forest = Color.green;
        public Vector4 m_marshForestRange = new Vector4(0.34f,0.29f,0.24f,0.32f);

        [Header("资源")] 
        [Range(0, 50)] public float m_normalRscCount = 50;

        [Range(0, 50)] public float m_chestRscCount = 50;
        [Range(0, 50)] public float m_enemyCount = 30;
        [Range(0, 50)] public float m_relicCount = 10;
     
        [HideInInspector]
        public Texture2D m_PCGTex = null;

        void SetMat(Material material)
        {
            if (material)
            {
                material.SetVector("_NoiseOffset",m_offset);
                material.SetFloat("_NoiseFreq",m_noiseFreq);
                material.SetInt("_NoiseStep",m_noiseSteps);
                material.SetFloat("_NoisePersistence",m_noisePersist);
                material.SetFloat("_TerrainTiles_x",m_hundredTileCount_x);
                material.SetFloat("_TerrainTiles_y",m_hundredTileCount_y);
                material.SetFloat("_LargeScale",m_largeScale);
                material.SetFloat("_LargeFac",m_largeFac);
                material.SetFloat("_DetailFac",m_detailFac);
                //
                material.SetColor("_Color_Mount",m_color_Mount);
                material.SetFloat("_Mount",m_Mount);
                material.SetColor("_Color_Hill",m_color_Hill);
                material.SetFloat("_Hill",m_Hill);
                material.SetColor("_Color_NormalHeight",m_color_NormalHeight);
                material.SetFloat("_NormalHeight",m_NormalHeight);
                material.SetColor("_Color_Water",m_color_Water);
                //
                material.SetColor("_Color_Sand",m_color_Sand);
                material.SetFloat("_Sand",m_Sand);
                material.SetColor("_Color_Ice",m_color_Ice);
                material.SetFloat("_Ice",m_Ice);
                material.SetColor("_Color_Marsh",m_color_Marsh);
                material.SetColor("_Color_Forest",m_color_Forest);
                material.SetVector("_MarshForestRange",m_marshForestRange);
                //
                material.SetFloat("_SpecialNoiseFac",m_spNoiseFac);
                material.SetFloat("_SpecialNoiseScale",m_spNoiseScale);
            }
        }

        public void ReadPCGTex()
        {
            if (m_proceduralMat)
            {
                int size_x= 100 * m_hundredTileCount_x;
                int size_y= 100 * m_hundredTileCount_y;
                if (m_PCGTex)
                {
                    m_PCGTex = null;
                    //让他自己GC吧= =
                }

                m_PCGTex = new Texture2D(size_x, size_y, TextureFormat.RGBA64,false,true);
                m_PCGTex.filterMode = FilterMode.Point;
                Material blitMat = Material.Instantiate(m_proceduralMat);
                blitMat.CopyPropertiesFromMaterial(m_proceduralMat);
                SetMat(blitMat);
                //
                RenderTexture rt = new RenderTexture(size_x, size_y, 0,
                    GraphicsFormat.R16G16B16A16_SFloat);
                rt.filterMode = FilterMode.Point;
                Graphics.Blit(null,rt,blitMat);
                RenderTexture.active = rt;
                m_PCGTex.ReadPixels(new Rect(0,0,size_x,size_y),0,0);
                m_PCGTex.Apply();
                rt.Release();
            }
        }

        public void SetTilleMap()
        {
            if (m_tileMap && m_PCGTex)
            {
                if (m_mapAsset==null || !m_mapAsset.isAssetValid())
                {
                    return;
                }

                m_tileMap.ClearAllTiles();

                Color[] cols = m_PCGTex.GetPixels();
                for (int i = 0; i < m_hundredTileCount_x*100;i++)
                {
                    for (int j = 0; j < m_hundredTileCount_y*100; j++)
                    {
                        Vector3Int p = new Vector3Int(i, j);
                        var tile = GetTileType(cols[i * m_hundredTileCount_x * 100 + j]);
                        m_tileMap.SetTile(p,tile);
                    }
                }
            }
        }

        public void SetRscMap()
        {
            if (m_rscTileMap && m_tileMap)
            {
                if (!m_rscMapAsset || !m_rscMapAsset.m_isAssetValid())
                {
                    return;
                }

                HashSet<Vector3Int> rscCache = new HashSet<Vector3Int>();
                m_rscTileMap.ClearAllTiles();

                Vector3Int gateP = GetRandomTilePoint();
                int safeTimes = 0;
                while (!isGateValid(gateP))
                {
                    gateP = GetRandomTilePoint();
                    safeTimes++;
                    if (safeTimes > 10000)
                    {
                        safeTimes = 0;
                        Debug.Log("NoGate");
                        break;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var gatePart = gateP + new Vector3Int(i, j, 0);
                        rscCache.Add(gatePart);
                        //m_rscTileMap.SetColor(gatePart,Color.black);
                    }
                }
                m_rscTileMap.SetTile(gateP,m_rscMapAsset.m_gateTile);
                //m_tileMap.SetTile(gateP,m_rscMapAsset.m_gateTile);

                for (int i = 0; i < m_normalRscCount; i++)
                {
                    Vector3Int farmP = GetRandomTilePoint();
                    Vector3Int mineP = GetRandomTilePoint();
                    Vector3Int hotP = GetRandomTilePoint();
                    //Farm
                    while (m_tileMap.GetTile(farmP) != m_mapAsset.m_normalTile || rscCache.Contains(farmP))
                    {
                        farmP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(farmP);
                    m_rscTileMap.SetTile(farmP,m_rscMapAsset.m_farmTile);
                    //m_tileMap.SetTile(farmP,m_rscMapAsset.m_farmTile);

                    //mine
                    while (m_tileMap.GetTile(mineP) != m_mapAsset.m_hillTile || rscCache.Contains(mineP))
                    {
                        mineP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(mineP);
                    m_rscTileMap.SetTile(mineP,m_rscMapAsset.m_mineTile);
                    //m_tileMap.SetTile(mineP,m_rscMapAsset.m_mineTile);

                    //hot
                    while ((m_tileMap.GetTile(hotP) != m_mapAsset.m_normalTile &&
                           m_tileMap.GetTile(hotP) != m_mapAsset.m_hillTile) || rscCache.Contains(hotP))
                    {
                        hotP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }
                    
                    rscCache.Add(hotP);
                    m_rscTileMap.SetTile(hotP,m_rscMapAsset.m_hotTile);
                    //m_tileMap.SetTile(hotP,m_rscMapAsset.m_hotTile);
                }
                //CHests
                for (int i = 0; i < m_chestRscCount; i++)
                {
                    Vector3Int foodP = GetRandomTilePoint();
                    Vector3Int metalP = GetRandomTilePoint();
                    Vector3Int peopleP = GetRandomTilePoint();
                    //food
                    while ((m_tileMap.GetTile(foodP) == m_mapAsset.m_mountTile ||
                           m_tileMap.GetTile(foodP) == m_mapAsset.m_waterTile)
                        || rscCache.Contains(foodP))
                    {
                        foodP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(foodP);
                    m_rscTileMap.SetTile(foodP,m_rscMapAsset.m_foodChestTile);

                    //metal
                    while ((m_tileMap.GetTile(metalP) == m_mapAsset.m_mountTile ||
                           m_tileMap.GetTile(metalP) == m_mapAsset.m_waterTile)
                           || rscCache.Contains(metalP))
                    {
                        metalP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(metalP);
                    m_rscTileMap.SetTile(metalP,m_rscMapAsset.m_metalChestTile);

                    //people
                    while ((m_tileMap.GetTile(peopleP) == m_mapAsset.m_mountTile ||
                           m_tileMap.GetTile(peopleP) == m_mapAsset.m_waterTile)
                           || rscCache.Contains(peopleP))
                    {
                        peopleP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }
                    
                    rscCache.Add(peopleP);
                    m_rscTileMap.SetTile(peopleP,m_rscMapAsset.m_peopleChestTile);
                }
                
                //Enemies
                for (int i = 0; i < m_enemyCount; i++)
                {
                    Vector3Int enemyP = GetRandomTilePoint();
                    while ((m_tileMap.GetTile(enemyP) == m_mapAsset.m_mountTile ||
                           m_tileMap.GetTile(enemyP) == m_mapAsset.m_waterTile)
                           || rscCache.Contains(enemyP))
                    {
                        enemyP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(enemyP);
                    int enemyTypeCount = m_rscMapAsset.m_campTiles.Count;
                    int randId = Random.Range(0, enemyTypeCount);
                    m_rscTileMap.SetTile(enemyP,m_rscMapAsset.m_campTiles[randId]);
                    //m_tileMap.SetTile(enemyP,m_rscMapAsset.m_campTile);
                }
                
                //relics
                for (int i = 0; i < m_relicCount; i++)
                {
                    Vector3Int relicP = GetRandomTilePoint();
                    while ((m_tileMap.GetTile(relicP) == m_mapAsset.m_mountTile ||
                            m_tileMap.GetTile(relicP) == m_mapAsset.m_waterTile)
                           || rscCache.Contains(relicP))
                    {
                        relicP = GetRandomTilePoint();
                        safeTimes++;
                        if (safeTimes > 10000)
                        {
                            safeTimes = 0;
                            break;
                        }
                    }

                    rscCache.Add(relicP);
                    m_rscTileMap.SetTile(relicP,m_rscMapAsset.m_relicTile);
                    //m_tileMap.SetTile(relicP,m_rscMapAsset.m_relicTile);
                }
            }
        }

        public void RefreshAllMaps(int seed = 0)
        {
            if (seed < 0)
            {
                m_offset = Vector2.zero;
            }
            else if (seed > 0)
            {
                m_offset = new Vector2(sin(frac(seed / 2023)), cos(frac(max(seed,2023*2023) / 2023.0205f)));
            }

            SetTilleMap();
            SetRscMap();
        }

        bool isGateValid(Vector3Int p)
        {
            Vector3Int doorP = p - new Vector3Int(0, 1, 0);
            for (int i = -4; i < 5; i++)
            {
                for (int j = -4; j < 5; j++)
                {
                    if (doorP.x+i < 0 || doorP.x+i >= m_hundredTileCount_x*100 || doorP.y+j < 0 || doorP.y+j > m_hundredTileCount_y*100)
                    {
                        return false;
                    }

                    var tile = m_tileMap.GetTile(new Vector3Int(doorP.x+i, doorP.y+j, 0));
                    if (tile == m_mapAsset.m_mountTile || tile == m_mapAsset.m_waterTile)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        Vector3Int GetRandomTilePoint()
        {
            return new Vector3Int(Random.Range(0, m_hundredTileCount_x*100), Random.Range(0, m_hundredTileCount_y*100), 0);
        }

        /// <summary>
        /// mount - 0
        /// hill - 1
        /// normalheight - 2
        /// water - 3
        /// sand - 4
        /// ice - 5
        /// marsh - 6
        /// forest - 7
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        TileBase GetTileType(Color col)
        {
            if (!m_mapAsset || !m_mapAsset.isAssetValid())
            {
                return null;
            }

            if (((Vector4)col - (Vector4)m_color_Mount).magnitude < 0.02f)
            {
                return m_mapAsset.m_mountTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Hill).magnitude < 0.02f)
            {
                return m_mapAsset.m_hillTile;
            }
            else if (((Vector4)col - (Vector4)m_color_NormalHeight).magnitude < 0.02f)
            {
                return m_mapAsset.m_normalTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Water).magnitude < 0.02f)
            {
                return m_mapAsset.m_waterTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Sand).magnitude < 0.02f)
            {
                return m_mapAsset.m_sandTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Ice).magnitude < 0.02f)
            {
                return m_mapAsset.m_iceTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Marsh).magnitude < 0.02f)
            {
                return m_mapAsset.m_marshTile;
            }
            else if (((Vector4)col - (Vector4)m_color_Forest).magnitude < 0.02f)
            {
                return m_mapAsset.m_forestTile;
            }

            return m_mapAsset.m_normalTile;
        }

    }
}

