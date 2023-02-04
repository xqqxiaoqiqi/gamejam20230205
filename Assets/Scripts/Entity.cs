using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Entity : MonoBehaviour
{
    public int capability;
    public float movespeed;
    public GameManager.PlayerSide playerSide;
    public BasicTile currentTile;
    public Behaviour[] behaviours;
    public bool isFinished = false;
    public void OnInit(GameManager.PlayerSide playerSide)
    {
        this.playerSide = playerSide;
    }

    public void OnTick()
    {
       UpdateCurrentTIle();
    }

    public void SearchTarget()
    {
        
    }

    public void UpdateCurrentTIle()
    {
        
    }

    public void MarkFinish()
    {
        isFinished = true;
    }
}
