using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : Building
{
    public List<Item> ores = new List<Item>();
    public float speed;
    public GameObject copper;
    public GameObject iron;
    public GameObject coal;
    public override void Clicked()
    {
        if (beingBuilt)
        {
            if (kolize)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                Invoke("ResetColor", 0.7f);
                return;
            }
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.name == "trees") trees = obj;
                if (obj.name == "zelezoTile_") iron = obj;
                if (obj.name == "uhliTile_") coal = obj;
                if (obj.name == "medTile_") copper = obj;
            }
            pos = gameObject.transform.position;
            if (CheckForOres())
            {
                beingBuilt = false;
                Player.isBuilding = false;
                foreach (BuildReqs req in buildCost)
                {
                    manager.AddItem(req.item, -req.amount);
                }
                progressMax = 1/(speed / 4 * ores.Count);
                GameObject info = Instantiate(buildInfo, buildPlace.transform);
                info.transform.localPosition = new Vector3(1200, 0);
                buildInfo = info;
                buildInfo.SetActive(true);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                Invoke("ResetColor", 0.7f);
            }
        }
        else
        {
            player.ShowInv();
            buildInfo.transform.localPosition = new Vector3(530, 0);
        }
    }
    public bool CheckForOres()
    {
        foreach(Transform tr in trees.transform)
        {
            if (tr.gameObject.name == "tree_" + (pos.x - 0.5f) + "_" + (pos.y - 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x - 0.5f) + "_" + (pos.y + 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x + 0.5f) + "_" + (pos.y - 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x + 0.5f) + "_" + (pos.y + 0.5f)) return false;
        }
        foreach(Transform tr in coal.transform)
        {
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
        }
        foreach (Transform tr in iron.transform)
        {
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
        }
        foreach (Transform tr in copper.transform)
        {
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x - 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y - 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
            if (tr.gameObject.name == "tile_" + (pos.x + 0.5f) + "_" + (pos.y + 0.5f)) ores.Add(tr.GetComponent<Clickable>().drop);
        }
        Debug.Log(ores[0]);
        if (ores.Count > 0) return true;
        else return false;
    }
    public void Mine()
    {
        Debug.Log(ores[0]);
        GameObject pocket = Instantiate(dropPocket, transform.position + dropSpot, Quaternion.identity);
        InventoryItem it = pocket.GetComponent<InventoryItem>();
        it.InitialiseItem(ores[0], 1);
        pocket.GetComponent<BoxCollider2D>().size = new Vector2(0.4f,0.4f);
    }
    private void Update()
    {
        if (beingBuilt)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.FloorToInt(mousePosition.x + 0.5f * (size.x - 1)) - 0.5f * (size.x - 1), Mathf.FloorToInt(mousePosition.y + 0.5f * (size.y - 1)) - 0.5f * (size.y - 1), -2);
        }
        if(buildInfo.GetComponent<BuildInfo>().FuelBar.value <= 0 && buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>() != null && !beingBuilt)
        {
            fuelMax = buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().item.fuelValue;
            buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().count--;
            buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().RefreshCount();
            buildInfo.GetComponent<BuildInfo>().FuelBar.maxValue = buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().item.fuelValue;
            buildInfo.GetComponent<BuildInfo>().FuelBar.value = buildInfo.GetComponent<BuildInfo>().FuelBar.maxValue;
            if (buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().count == 0) Destroy(buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().gameObject);
        }
        if(buildInfo.GetComponent<BuildInfo>().FuelBar.value > 0 && !beingBuilt)
        {
            buildInfo.GetComponent<BuildInfo>().FuelBar.value -= Time.deltaTime;
            buildInfo.GetComponent<BuildInfo>().progressBar.value += Time.deltaTime;
        }
        if(buildInfo.GetComponent<BuildInfo>().progressBar.value >= buildInfo.GetComponent<BuildInfo>().progressBar.maxValue && !beingBuilt)
        {
            buildInfo.GetComponent<BuildInfo>().progressBar.value -= buildInfo.GetComponent<BuildInfo>().progressBar.maxValue;
            Mine();
        }
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
        if(buildInfo.GetComponent<BuildInfo>().inputs.transform.childCount > 0)
        {
            Destroy(buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).gameObject);
        }
    }
}
