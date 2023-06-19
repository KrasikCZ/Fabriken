using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Building
{
    public List<Recipe> recepty;
    public InventoryItem input;
    public Item inputItem;
    public int inputAmount;
    public bool hasEnough = false;
    private void Update()
    {
        progressMax = 2;
        if (beingBuilt)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.FloorToInt(mousePosition.x + 0.5f * (size.x - 1)) - 0.5f * (size.x - 1), Mathf.FloorToInt(mousePosition.y + 0.5f * (size.y - 1)) - 0.5f * (size.y - 1), -2);
        }
        if (buildInfo.GetComponent<BuildInfo>().FuelBar.value <= 0 && buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>() != null && !beingBuilt)
        {
            fuelMax = buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().item.fuelValue;
            buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().count--;
            buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().RefreshCount();
            buildInfo.GetComponent<BuildInfo>().FuelBar.maxValue = buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().item.fuelValue;
            buildInfo.GetComponent<BuildInfo>().FuelBar.value = buildInfo.GetComponent<BuildInfo>().FuelBar.maxValue;
            if (buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().count == 0) Destroy(buildInfo.GetComponent<BuildInfo>().fuelSlot.GetComponentInChildren<InventoryItem>().gameObject);
        }
        if (buildInfo.GetComponent<BuildInfo>().FuelBar.value > 0 && !beingBuilt && hasEnough)
        {
            buildInfo.GetComponent<BuildInfo>().FuelBar.value -= Time.deltaTime;
            buildInfo.GetComponent<BuildInfo>().progressBar.value += Time.deltaTime;
            hasEnough = false;
        }
        if (buildInfo.GetComponent<BuildInfo>().progressBar.value >= buildInfo.GetComponent<BuildInfo>().progressBar.maxValue && !beingBuilt)
        {
            buildInfo.GetComponent<BuildInfo>().progressBar.value -= buildInfo.GetComponent<BuildInfo>().progressBar.maxValue;
            Bake();
        }
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
        if (buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>() != null&& buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().count == 0) Destroy(buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().gameObject);
        if(buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>() != null && buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().count >= 2)
        {
            hasEnough = true;
        }
    }
    public override void Clicked()
    {
        if (beingBuilt)
        {
            beingBuilt = false;
            Player.isBuilding = false;
            foreach (BuildReqs req in buildCost)
            {
                manager.AddItem(req.item, -req.amount);
            }
            pos = gameObject.transform.position;
            GameObject info = Instantiate(buildInfo, buildPlace.transform);
            info.transform.localPosition = new Vector3(1200, 0);
            buildInfo = info;
            buildInfo.SetActive(true);
            buildInfo.GetComponent<BuildInfo>().LoadSlots();
        }
        else
        {
            player.ShowInv();
            buildInfo.transform.localPosition = new Vector3(530, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<InventoryItem>())
        {
            foreach (Recipe rp in recepty)
            {
                foreach (BuildReqs br in rp.inputs)
                {
                    if (br.item == other.gameObject.GetComponent<InventoryItem>().item)
                    {
                        if (buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>() == null)
                        {
                            GameObject it = Instantiate(buildInfo.GetComponent<BuildInfo>().itemPrefab, buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0));
                            it.GetComponent<InventoryItem>().InitialiseItem(other.gameObject.GetComponent<InventoryItem>().item, 1);
                            Destroy(other.gameObject);
                        }
                        else if (buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().item == other.gameObject.GetComponent<InventoryItem>().item)
                        {
                            buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().count++;
                            buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().RefreshCount();
                            Destroy(other.gameObject);
                        }
                    }
                }
            }
        }
    }
    public void Bake()
    {
        foreach (Recipe rp in recepty)
        {
            foreach (BuildReqs br in rp.inputs)
            {
                if (br.item == buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().item)
                {
                    buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().count -= 2;
                    buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().RefreshCount();
                    if (buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().count == 0) Destroy(buildInfo.GetComponent<BuildInfo>().inputs.transform.GetChild(0).GetComponentInChildren<InventoryItem>().gameObject);
                    GameObject pocket = Instantiate(dropPocket, transform.position + dropSpot, Quaternion.identity);
                    InventoryItem it = pocket.GetComponent<InventoryItem>();
                    it.InitialiseItem(rp.output, 1);
                    pocket.GetComponent<BoxCollider2D>().size = new Vector2(0.4f, 0.4f);
                    return;
                }
            }
        }
    }
}
