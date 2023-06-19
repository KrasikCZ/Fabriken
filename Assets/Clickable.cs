using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public Sprite sprite;
    public Vector2 pos;
    public Item drop;
    public int amountDropped;
    public float breakSpeed;
    private InventoryManager manager;
    public int id;
    private void Start()
    {
        manager = Resources.FindObjectsOfTypeAll<InventoryManager>()[0];
    }
    /*private void OnMouseEnter()
    {
            Oznaceni.Instance.CursorMoved(this.gameObject);
    }
    private void OnMouseExit()
    {
            Oznaceni.Instance.CursorMoved(null);
    }*/
    public virtual void Clicked()
    {
        
    }
    public void DoneBreaking()
    {
        //pridat do inventare
        manager = Resources.FindObjectsOfTypeAll<InventoryManager>()[0];
        if(drop != null)
        manager.AddItem(drop, amountDropped);
        if (!gameObject.CompareTag("Mineable"))
        {
            Destroy(gameObject);
        }
    }
}
