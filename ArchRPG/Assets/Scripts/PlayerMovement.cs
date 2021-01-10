using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterAnimationHandler
{
    public int direction;
    public float move_speed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        base.Start();
    }

    public void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * move_speed;

        if (rb.velocity.magnitude > 0.0f) base.moving = true;
        else base.moving = false;

        if (rb.velocity.y > 0.0f) direction = 0;
        else if (rb.velocity.x > 0.0f) direction = 1;
        else if (rb.velocity.y < 0.0f) direction = 2;
        else if (rb.velocity.x < 0.0f) direction = 3;

        base.Animate(direction);

        was_moving = moving;
    }
}
