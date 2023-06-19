using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour, IDataPersistance
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_trava;
    public GameObject prefab_zelezo;
    public GameObject prefab_uhli;
    public GameObject prefab_med;
    public GameObject prefab_strom;
    int map_width = 64;
    int map_height = 36;
    int seed_x;
    int seed_y;
    static bool zmenaMista = false;

    List<List<int>> noise_grid = new List<List<int>>();
    List<Vector3> tile_grid = new List<Vector3>();

    float magnification = 10f;
    public static int x_offset = 0;
    public static int y_offset = 0;
    static Vector2 smer = new Vector2(0,0);
    public void NewMap()
    {
        seed_x = Random.Range(+10000, 10000);
        seed_y = Random.Range(-10000, 10000);
        CreateTileSet();
        CreateTileGroup();
        GenerateMap();
    }
    void CreateTileSet()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_trava);
        tileset.Add(1, prefab_zelezo);
        tileset.Add(2, prefab_uhli);
        tileset.Add(3, prefab_med);
    }
    void CreateTileGroup()
    {
        tile_groups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name+"_");
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
        GameObject tile_grou = new GameObject("trees");
        tile_grou.transform.parent = gameObject.transform;
        tile_grou.transform.localPosition = new Vector3(0, 0.75f, 0);
        tile_groups.Add(4, tile_grou);
    }
    void GenerateMap()
    {
        for (int x = -map_width / 2 + x_offset; x < map_width / 2 + x_offset; x++)
        {
            for (int y = -map_height / 2 + y_offset; y < map_height / 2 + y_offset; y++)
            {
                if (!tile_grid.Contains(new Vector3(x,y,0))) { 
                    int tile_id = GetIdUsingPerlin(x, y);
                    CreateTile(tile_id, x, y);
                }
            }
        }
    }
    private int GetIdUsingPerlin(int x, int y)
    {
        float raw_perlin = Mathf.PerlinNoise((x + seed_x)/magnification,(y + seed_y)/magnification);
        float clamp_perlin = Mathf.Clamp(raw_perlin, 0.0f, 1.0f);
        int id = 0;
        if (clamp_perlin >= 0.75f)
        {
            id = Random.Range(1, 4);
        }
        if (clamp_perlin < 0.2f) id = -1;
        return id;
    }
    private void CreateTile(int tile_id, int x ,int y)
    {
        tile_grid.Add(new Vector3(x, y, 0));
        bool strom = false;
        if(tile_id == -1)
        {
            strom = true;
            tile_id = 0;
        }
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = "tile_" + (x) + "_" + (y);
        tile.transform.localPosition = new Vector3(x, y, 0);
        if (tile.GetComponent<Clickable>() != null) {
            tile.GetComponent<Clickable>().pos = new Vector2(x, y);
             }
        if(strom)
        {
            GameObject tree = Instantiate(prefab_strom, tile_groups[4].transform);
            tree.name = "tree_" + (x) + "_" + (y);
            tree.transform.localPosition = new Vector3(x, y, -1);
        }
        if(tile_id != 0)
        {
            if (Mathf.Clamp(Mathf.PerlinNoise((x + 1 + seed_x) / magnification, (y + seed_y) / magnification), 0.0f, 1.0f) >= 0.75f && !tile_grid.Contains(new Vector3(x + 1, y, 0))) CreateTile(tile_id, x + 1, y);
            if (Mathf.Clamp(Mathf.PerlinNoise((x - 1 + seed_x) / magnification, (y + seed_y) / magnification), 0.0f, 1.0f) >= 0.75f && !tile_grid.Contains(new Vector3(x - 1, y, 0))) CreateTile(tile_id, x - 1, y);
            if (Mathf.Clamp(Mathf.PerlinNoise((x + seed_x) / magnification, (y + 1 + seed_y) / magnification), 0.0f, 1.0f) >= 0.75f && !tile_grid.Contains(new Vector3(x, y + 1, 0))) CreateTile(tile_id, x, y + 1);
            if (Mathf.Clamp(Mathf.PerlinNoise((x + seed_x) / magnification, (y - 1 + seed_y) / magnification), 0.0f, 1.0f) >= 0.75f && !tile_grid.Contains(new Vector3(x, y - 1, 0))) CreateTile(tile_id, x, y - 1);
        }
    }

    public static void MoveMap(int x, int y)
    {   
        smer = new Vector2(x - x_offset, y - y_offset);
        x_offset = x;
        y_offset = y;
        zmenaMista = true;
    }
    private void Update()
    {
        if (zmenaMista)
        {
            zmenaMista = false;
            GenerateMap();
            //TreesMove();
        }
    }
    public void WipeMap()
    {
        if (tile_groups != null) { 
            for (int i = 0; i < 5; i++)
            {
                foreach (Transform obj in tile_groups[i].transform)
                {
                    Destroy(obj.gameObject);
                }
            }
        } else
        {
            CreateTileSet();
            CreateTileGroup();
        }
    }

    public void LoadData(GameData data)
    {
        WipeMap();
        this.seed_x = data.seed_x;
        this.seed_y = data.seed_y;
        foreach(Vector3 v in data.zeleza)
        {
            tile_grid.Add(v);
            GameObject tile = Instantiate(prefab_zelezo, tile_groups[1].gameObject.transform);
            tile.name = "tile_" + (v.x) + "_" + (v.y);
            tile.transform.localPosition = new Vector3(v.x, v.y, 0);
            tile.GetComponent<Clickable>().pos = new Vector2(v.x, v.y);
        }
        foreach (Vector3 v in data.mede)
        {
            tile_grid.Add(v);
            GameObject tile = Instantiate(prefab_med, tile_groups[3].gameObject.transform);
            tile.name = "tile_" + (v.x) + "_" + (v.y);
            tile.transform.localPosition = new Vector3(v.x, v.y, 0);
            tile.GetComponent<Clickable>().pos = new Vector2(v.x, v.y);
        }
        foreach (Vector3 v in data.uhli)
        {
            tile_grid.Add(v);
            GameObject tile = Instantiate(prefab_uhli, tile_groups[2].gameObject.transform);
            tile.name = "tile_" + (v.x) + "_" + (v.y);
            tile.transform.localPosition = new Vector3(v.x, v.y, 0);
            tile.GetComponent<Clickable>().pos = new Vector2(v.x, v.y);
        }
        foreach (Vector3 v in data.travy)
        {
            tile_grid.Add(v);
            GameObject tile = Instantiate(prefab_trava, tile_groups[0].gameObject.transform);
            tile.name = "tile_" + (v.x) + "_" + (v.y);
            tile.transform.localPosition = new Vector3(v.x, v.y, 0);
        }
        foreach(Vector3 v in data.stromy)
        {
            GameObject tree = Instantiate(prefab_strom, tile_groups[4].gameObject.transform);
            tree.name = "tree_" + (v.x) + "_" + (v.y);
            tree.transform.localPosition = new Vector3(v.x, v.y, -1);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.seed_x = this.seed_x;
        data.seed_y = this.seed_y;
        List<Vector3> zeleza = new List<Vector3>();
        foreach (Transform obj in transform.GetChild(1).transform) { zeleza.Add(obj.position); }
        List<Vector3> mede = new List<Vector3>();
        foreach (Transform obj in transform.GetChild(3).transform) { mede.Add(obj.position); }
        List<Vector3> travy = new List<Vector3>();
        foreach (Transform obj in transform.GetChild(0).transform) { travy.Add(obj.position); }
        List<Vector3> uhli = new List<Vector3>();
        foreach (Transform obj in transform.GetChild(2).transform) { uhli.Add(obj.position); }
        List<Vector3> stromy = new List<Vector3>();
        foreach (Transform obj in transform.GetChild(4).transform) { stromy.Add(obj.position); }
        data.zeleza = zeleza;
        data.mede = mede;
        data.travy = travy;
        data.uhli = uhli;
        data.stromy = stromy;
    }
}
