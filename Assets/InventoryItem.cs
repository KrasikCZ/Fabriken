using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;
    public int count = 1;
    public Text countText;
    public Transform parentAfterDrag;
    public void InitialiseItem(Item newItem, int amount)
    {
        item = newItem;
        image.sprite = newItem.image;
        count = amount;
        RefreshCount();
        if(tag == "Dropped")
        {
            GetComponent<SpriteRenderer>().sprite = newItem.image;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        countText.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        RefreshCount();
    }

    public void RefreshCount()
    {
        if (tag != "Dropped") { 
            countText.text = count.ToString();
            bool textActive = count > 1;
            countText.gameObject.SetActive(textActive); }
    }
}
