using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            if(CompareTag("Fuel") && eventData.pointerDrag.GetComponent<InventoryItem>().item.fuelValue == 0)
            {

            } else if(CompareTag("Fuel") && eventData.pointerDrag.GetComponent<InventoryItem>().item.fuelValue > 0)
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            } else if (CompareTag("Input")){
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
            else
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
        } else if(transform.GetComponentInChildren<InventoryItem>().item == eventData.pointerDrag.GetComponent<InventoryItem>().item){
            transform.GetComponentInChildren<InventoryItem>().count += eventData.pointerDrag.GetComponent<InventoryItem>().count;
            Destroy(eventData.pointerDrag);
        }
    }
}
