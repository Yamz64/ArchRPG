using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterAnimationHandler
{
    public bool interaction_protection;
    public int direction;
    public float move_speed;
    public float interaction_distance;

    private Rigidbody2D rb;

    private new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        base.Start();
    }

    public void Update()
    {
        if (!interaction_protection)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * move_speed;

            //handle animation
            if (rb.velocity.magnitude > 0.0f) base.moving = true;
            else base.moving = false;

            if (rb.velocity.y > 0.0f) direction = 0;
            else if (rb.velocity.y < 0.0f) direction = 2;
            else if (rb.velocity.x > 0.0f) direction = 1;
            else if (rb.velocity.x < 0.0f) direction = 3;

            base.Animate(direction);

            was_moving = moving;

            //handle interaction
            if (Input.GetButtonDown("Interact"))
            {
                //prepare a direction to raycast
                Vector2 direction_cast = Vector2.zero;
                switch (direction)
                {
                    //up
                    case 0:
                        direction_cast = Vector2.up;
                        break;
                    //right
                    case 1:
                        direction_cast = Vector2.right;
                        break;
                    //down
                    case 2:
                        direction_cast = Vector2.down;
                        break;
                    //left
                    case 3:
                        direction_cast = Vector2.left;
                        break;
                    default:
                        break;
                }

                //raycast in that direction for a valid object
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction_cast, interaction_distance, 1 << 8);
                if (hit)
                {
                    hit.collider.gameObject.GetComponent<InteractableBaseClass>().Interact();
                }
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GetComponent<PlayerDataMono>().data);
            data.Save(0);
        }else if (Input.GetKeyDown(KeyCode.L))
        {
            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GetComponent<PlayerDataMono>().data);
            data.Load(0);
            data.UpdatePlayerData(ref GetComponent<PlayerDataMono>().data);
        }
    }
}
