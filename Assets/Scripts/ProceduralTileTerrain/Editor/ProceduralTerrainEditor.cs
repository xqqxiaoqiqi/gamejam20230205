using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PCGTerrain
{
    [CustomEditor(typeof(ProceduralTileTerrain))]
    public class ProceduralTerrainEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ProceduralTileTerrain terrain = target as ProceduralTileTerrain;
            base.OnInspectorGUI();
            if (terrain.m_PCGTex)
            {
                Rect PCGTexArea = EditorGUILayout.BeginVertical(GUILayout.Height(terrain.m_hundredTileCount_y*100));
                EditorGUI.DrawPreviewTexture(new Rect(Mathf.Max(0,(PCGTexArea.width - terrain.m_hundredTileCount_x*100)/2),PCGTexArea.y,terrain.m_hundredTileCount_x*100,terrain.m_hundredTileCount_y*100),terrain.m_PCGTex);
                EditorGUILayout.LabelField("",GUILayout.Height(terrain.m_hundredTileCount_y*100));
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("生成贴图"))
            {
                terrain.ReadPCGTex();
            }

            if (GUILayout.Button("设置Tilemap"))
            {
                terrain.SetTilleMap();
            }

            if (GUILayout.Button("设置资源"))
            {
                terrain.SetRscMap();
            }

            if (GUILayout.Button("刷新全图"))
            {
                terrain.RefreshAllMaps();
            }
        }
    }
}

