using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int diff;
    public float hp;
    public float cas;
    public int den;
    public int seed_x;
    public int seed_y;
    public Vector2 pos;
    public List<Item> inv;
    public List<Vector3> builds = new List<Vector3>();
    public List<string> buildNames = new List<string>();
    public List<Vector3> travy;
    public List<Vector3> zeleza;
    public List<Vector3> uhli;
    public List<Vector3> mede;
    public List<Vector3> stromy;
    public GameData()
    {
        diff = 0;
    }
}
