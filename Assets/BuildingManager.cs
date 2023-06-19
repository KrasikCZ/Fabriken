using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour, IDataPersistance
{
    public List<GameObject> buildings;
    public void LoadData(GameData data)
    {
        foreach (Transform tr in transform) Destroy(tr.gameObject);
        int i = 0;
        foreach(Vector3 b in data.builds)
        {
            foreach (GameObject bu in buildings)
            {
                if (bu.name + "(Clone)" == data.buildNames[i])
                {
                    Building obj = Instantiate(bu, b, Quaternion.identity, transform).GetComponent<Building>();
                    if (!bu.GetComponent<Mover>())
                    {
                        obj.Loaded();
                    }
                    if (bu.GetComponent<Miner>())
                    {
                        bu.GetComponent<Building>().pos = bu.transform.position;
                        foreach (GameObject ob in Resources.FindObjectsOfTypeAll<GameObject>())
                        {
                            if (ob.name == "trees") bu.GetComponent<Miner>().trees = ob;
                            if (ob.name == "zelezoTile_") bu.GetComponent<Miner>().iron = ob;
                            if (ob.name == "uhliTile_") bu.GetComponent<Miner>().coal = ob;
                            if (ob.name == "medTile_") bu.GetComponent<Miner>().copper = ob;
                        }
                        bu.GetComponent<Miner>().ores.Add(bu.GetComponent<Miner>().iron.transform.GetComponentInChildren<Clickable>().drop);
                        bu.GetComponent<Miner>().CheckForOres();
                    }
                }
            }
            i++;
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach(Transform tr in transform)
        {
            data.builds.Add(tr.position);
            data.buildNames.Add(tr.name);
        }
    }

}
