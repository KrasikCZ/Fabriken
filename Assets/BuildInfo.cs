using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfo : MonoBehaviour
{
    public GameObject slotPrefab;
    public Slider progressBar;
    public Slider FuelBar;
    public InventorySlot fuelSlot;
    public GameObject inputs;
    public GameObject itemPrefab;
    public void Awake()
    {
        progressBar = transform.GetChild(0).GetComponent<Slider>();
        FuelBar = transform.GetChild(1).GetComponent<Slider>();
        fuelSlot = transform.GetChild(2).GetComponent<InventorySlot>();
    }
    public void GetBuildInfo(Building build)
    {
        if (build == null)
        {
            foreach(Transform obj in fuelSlot.transform)
            {
                Destroy(obj.gameObject);
            }
            //curBuild = null;
        } else
        {
            progressBar.maxValue = build.progressMax;
            progressBar.value = build.progressTime;
            FuelBar.maxValue = build.fuelMax;
            FuelBar.value = build.fuelRemain;
            if (build.fuel != null && fuelSlot.transform.childCount == 0) { 
                InventoryItem it = Instantiate(itemPrefab, fuelSlot.transform).GetComponent<InventoryItem>();
            }
            if (build.GetComponent<Furnace>())
            {
                if(build.GetComponent<Furnace>().input != null && inputs.transform.GetChild(0).childCount == 0)
                {
                    InventoryItem it = Instantiate(itemPrefab, inputs.transform.GetChild(0).transform).GetComponent<InventoryItem>();
                    it.InitialiseItem(build.GetComponent<Furnace>().inputItem, build.GetComponent<Furnace>().inputAmount);
                }
            }
        }
    }
    public void LoadSlots()
    {
        InventorySlot inp = Instantiate(slotPrefab, inputs.transform).GetComponent<InventorySlot>();
        inp.tag = "Input";
    }
}
