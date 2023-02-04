using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Tilemaps;

namespace PCGTerrain
{
    public class ProceduralTileTerrain : MonoBehaviour
    {
        public Tilemap m_tileMap;
        public Material m_proceduralMat;

        public ProceduralTileTerrainMapAsset m_mapAsset;
        //Procedural Params
        //consts
        const float m_noisePersist = 0.5f;

        private const float m_spNoiseFac = 0.086f;

        private const float m_spNoiseScale = 6.03f;
        //vars
        public Vector2 m_offset = Vector2.zero;
        [Range(5,15)]public float m_noiseFreq = 11f;
        [Range(1,3)]public int m_noiseSteps = 2;
        [Range(1, 5)] public int m_hundredTileCount_x = 2;
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
        public Vector4 m_marshForestRange = new Vector4(0,0.13f,0.27f,0.39f);
     
        [HideInInspector]
        public Texture2D m_PCGTex = null;


        private void OnValidate()
        {
            ReadPCGTex();
        }

        void SetMat(Material material)
        {
            if (material)
            {
                m_proceduralMat.SetVector("_NoiseOffset",m_offset);
                m_proceduralMat.SetFloat("_NoiseFreq",m_noiseFreq);
                m_proceduralMat.SetInt("_NoiseStep",m_noiseSteps);
                m_proceduralMat.SetFloat("_NoisePersistence",m_noisePersist);
                m_proceduralMat.SetFloat("_TerrainTiles_x",m_hundredTileCount_x);
                m_proceduralMat.SetFloat("_TerrainTiles_y",m_hundredTileCount_y);
                m_proceduralMat.SetFloat("_LargeScale",m_largeScale);
                m_proceduralMat.SetFloat("_LargeFac",m_largeFac);
                m_proceduralMat.SetFloat("_DetailFac",m_detailFac);
                //
                m_proceduralMat.SetColor("_Color_Mount",m_color_Mount);
                m_proceduralMat.SetFloat("_Mount",m_Mount);
                m_proceduralMat.SetColor("_Color_Hill",m_color_Hill);
                m_proceduralMat.SetFloat("_Hill",m_Hill);
                m_proceduralMat.SetColor("_Color_NormalHeight",m_color_NormalHeight);
                m_proceduralMat.SetFloat("_NormalHeight",m_NormalHeight);
                m_proceduralMat.SetColor("_Color_Water",m_color_Water);
                //
                m_proceduralMat.SetColor("_Color_Sand",m_color_Sand);
                m_proceduralMat.SetFloat("_Sand",m_Sand);
                m_proceduralMat.SetColor("_Color_Ice",m_color_Ice);
                m_proceduralMat.SetFloat("_Ice",m_Ice);
                m_proceduralMat.SetColor("_Color_Marsh",m_color_Marsh);
                m_proceduralMat.SetColor("_Color_Forest",m_color_Forest);
                m_proceduralMat.SetVector("_MarshForestRange",m_marshForestRange);
                //
                m_proceduralMat.SetFloat("_SpecialNoiseFac",m_spNoiseFac);
                m_proceduralMat.SetFloat("_SpecialNoiseScale",m_spNoiseScale);
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

