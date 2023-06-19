using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Building : Clickable
{
    public List<BuildReqs> buildCost;
    public Sprite budova;
    public bool beingBuilt = false;
    public Vector2 size;
    public InventoryManager manager;
    public GameObject trees;
    public bool kolize = false;
    public GameObject buildInfo;
    public Player player;
    public InventoryItem fuel = null;
    public float progressTime;
    public float progressMax;
    public float fuelRemain;
    public float fuelMax;
    public bool gibUpdate = false;
    public GameObject dropPocket;
    public float maxHp;
    public float hp;
    public Vector3 dropSpot = new Vector3(1.26f, -0.26f);
    public Vector3 smer = new Vector3(0,0,0);
    public bool working = false;
    public GameObject buildPlace;
    private void Start()
    {
        manager = Resources.FindObjectsOfTypeAll<InventoryManager>()[0];
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == "BuildInvents") buildPlace = obj;
        }
        player = Resources.FindObjectsOfTypeAll<Player>()[0];
        maxHp = hp;
    }
    public override void Clicked()
    {
        if (beingBuilt)
        {
            Debug.Log(kolize);
            if (kolize)
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                Invoke("ResetColor", 0.7f);
            }
            else
            {
                beingBuilt = false;
                pos = gameObject.transform.position;
                Player.isBuilding = false;
                foreach (BuildReqs req in buildCost)
                {
                    manager.AddItem(req.item, -req.amount);
                }
            }
        }
        else
        {

        }
    }
    private void Update()
    {
        if (beingBuilt)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.FloorToInt(mousePosition.x + 0.5f * (size.x - 1)) - 0.5f * (size.x - 1), Mathf.FloorToInt(mousePosition.y + 0.5f * (size.y - 1)) - 0.5f * (size.y - 1), -2);
        }
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void StopBuilding()
    {
        Destroy(this);
    }
    public void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
    }
    public bool CheckForTrees()
    {
        foreach (Transform tr in trees.transform)
        {
            if (tr.gameObject.name == "tree_" + (pos.x - 0.5f) + "_" + (pos.y - 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x - 0.5f) + "_" + (pos.y + 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x + 0.5f) + "_" + (pos.y - 0.5f)) return false;
            if (tr.gameObject.name == "tree_" + (pos.x + 0.5f) + "_" + (pos.y + 0.5f)) return false;
        }
        return true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag != "Dropped") kolize = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag != "Dropped") kolize = false;
    }
    public void Rotate()
    {
        smer.z += -90;
        if (dropSpot.x > Mathf.Abs(0.5f)) { float a = dropSpot.y; dropSpot.y = -dropSpot.x; dropSpot.x = a; }
        else { float a = dropSpot.x; dropSpot.x = dropSpot.y; dropSpot.y = -a; }
    }
    public void Loaded()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == "BuildInvents") buildPlace = obj;
        }
        GameObject info = Instantiate(buildInfo, buildPlace.transform);
        info.transform.localPosition = new Vector3(1200, 0);
        buildInfo = info;
        buildInfo.SetActive(true);
        buildInfo.GetComponent<BuildInfo>().LoadSlots();
    }
}
