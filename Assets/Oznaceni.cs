using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oznaceni : MonoBehaviour
{
    public static Oznaceni Instance { get; private set; }
    public SpriteRenderer sprite;
    public Clickable selected = null;
    private void Start()
    {
        Instance = this;
    }
    public void CursorMoved(GameObject newSelected)
    {
        
        if (newSelected != null)
        {
            selected = newSelected.GetComponent<Clickable>();
            if (!Player.isBuilding)
            {
                sprite.sprite = selected.sprite;
                sprite.enabled = true;
                transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y, -5);
                if(newSelected.GetComponent<Building>() != null)
                {
                    transform.rotation = new Quaternion(0,0,0,0);
                    transform.Rotate(newSelected.GetComponent<Building>().smer);
                } else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            } else
            {
                selected = newSelected.GetComponent<Building>();
            }
        } else
        {
            selected = null;
            sprite.enabled = false;
        }
    }
}
