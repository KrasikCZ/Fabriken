using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                itemInSlot.count += amount;
                if(itemInSlot.count <= 0)
                {
                    amount = -itemInSlot.count;
                    Destroy(itemInSlot.gameObject);
                    return;
                }
                if (itemInSlot.count > itemInSlot.item.stack)
                {
                    amount = itemInSlot.count - itemInSlot.item.stack;
                    itemInSlot.count = itemInSlot.item.stack;
                    continue;
                }
                itemInSlot.RefreshCount();
                return;
            }
        }
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot, amount);
                return;
            }
        }
    }
    public void SpawnNewItem(Item item, InventorySlot slot, int amount)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item, amount);
    }
    public int FindItem(Item needed)
    {
        int found = 0;
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null && itemInSlot.item == needed)
            {
                found += itemInSlot.count;
            }
        }
        return found;
    }
}
