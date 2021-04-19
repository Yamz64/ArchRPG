using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class PlayerMovement : CharacterAnimationHandler
{
    public float i_frame_count;
    public bool interaction_protection;
    [HideInInspector]
    public bool intangible;
    public int direction;
    public float move_speed;
    public float interaction_distance;

    private Rigidbody2D rb;

    IEnumerator i_frame_sequence()
    {
        intangible = true;
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, .5f);
        yield return new WaitForSeconds(i_frame_count);
        intangible = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private new void Start()
    {
        intangible = false;
        rb = GetComponent<Rigidbody2D>();
        base.Start();

        //see if the player just fleed, and if they did, put them in i_frames
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
        if (data.flee)
        {
            StartCoroutine(i_frame_sequence());
        }
    }

    public void Update()
    {
        if (!interaction_protection)
        {
            rb.velocity = new Vector2(InputManager.GetAxisRaw("Horizontal"), InputManager.GetAxisRaw("Vertical")).normalized * move_speed;

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
            if (InputManager.GetButtonDown("Interact"))
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
        else
        {
            rb.velocity = Vector3.zero;
            base.Animate(direction);
        }
    }
}
