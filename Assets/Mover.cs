using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Building
{
    public float speed;
    public Vector2 smer = new Vector2(1,0);
    public List<Item> itemsOnMover = new List<Item>();
    private void Update()
    {
        if (beingBuilt)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.FloorToInt(mousePosition.x + 0.5f * (size.x - 1)) - 0.5f * (size.x - 1), Mathf.FloorToInt(mousePosition.y + 0.5f * (size.y - 1)) - 0.5f * (size.y - 1), -2);
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(0, 0, -90);
                if(smer.x != 0) { smer.y = -smer.x; smer.x = 0; } 
                else { smer.x = smer.y; smer.y = 0; }
            }
        }
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Dropped")
        {
            Rigidbody2D rb = other.attachedRigidbody;
            rb.MovePosition(rb.position + (smer * speed * Time.deltaTime));
        }
    }
}
