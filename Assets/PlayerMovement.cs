using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    Vector2 pohyb;
    Vector3 lastPos = new Vector3(0, 0, 0);
    void Update()
    {
        if (!Player.breaking && Player.canMove)
        {
            pohyb.x = Input.GetAxisRaw("Horizontal");
            pohyb.y = Input.GetAxisRaw("Vertical");
        }
    }
    void FixedUpdate()
    {
        Vector2 offset = transform.position - lastPos;
        rb.MovePosition(rb.position + pohyb * speed * Time.fixedDeltaTime);
        if (lastPos != new Vector3((int)transform.position.x, (int)transform.position.y, 0))
        {
            lastPos = new Vector3((int)transform.position.x, (int)transform.position.y, 0);
            MapGenerator.MoveMap((int)transform.position.x, (int)transform.position.y);
        }
    }
}
