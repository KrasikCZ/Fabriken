using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AppearBuild : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    public Building building;
    public InventoryManager manager;
    public GameObject buildy;
    private void Start()
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == "Buildy") buildy = obj;
        }
        manager = Resources.FindObjectsOfTypeAll<InventoryManager>()[0];
        panel = Resources.FindObjectsOfTypeAll<ShowBuildReqs>()[0].gameObject;
    }
    public void Clicked()
    {
        if (CheckIfEnough())
        {
            GameObject budova = Instantiate(building.gameObject, buildy.transform);
            budova.GetComponent<Building>().beingBuilt = true;
            Player.isBuilding = true;
        } else
        {
            GetComponent<Image>().color = new Color(255, 0, 0);
            Invoke("ResetColor", 0.7f);
        }
    }
    public void ResetColor()
    {
        GetComponent<Image>().color = new Color(255,255,255);
    }
    public bool CheckIfEnough()
    {
        foreach(BuildReqs potr in building.buildCost)
        {
            if(manager.FindItem(potr.item) < potr.amount)
            {
                return false;
            }
        }
        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.transform.position = new Vector3(transform.position.x, transform.position.y + 30f + building.buildCost.Count*24f);
        panel.GetComponent<ShowBuildReqs>().potreby = building.buildCost;
        panel.SetActive(true);
        panel.GetComponent<ShowBuildReqs>().ShowReqs();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
