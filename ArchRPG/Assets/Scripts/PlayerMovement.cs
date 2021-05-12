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

    [SerializeField]
    public List<Sprite> alt_idle_sprites_up,
        alt_idle_sprites_side,
        alt_idle_sprites_down;

    [SerializeField]
    public List<Sprite> alt_walk_sprites_up,
        alt_walk_sprites_side,
        alt_walk_sprites_down;

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
        if (data.enemy_names != null)
        {
            if(data.enemy_names.Length > 0)
            StartCoroutine(i_frame_sequence());
        }

        //if the player is doomed then replace all of the idle and walksprites with the doomed version
        if(GetComponent<PlayerDataMono>().data.GetStatus(25) > 0)
        {
            //clear all existing data
            idle_sprites_up.Clear();
            idle_sprites_side.Clear();
            idle_sprites_down.Clear();
            walk_sprites_up.Clear();
            walk_sprites_side.Clear();
            walk_sprites_down.Clear();

            //populate new walksprites to old walksprites
            for(int i=0; i<alt_idle_sprites_up.Count; i++) { idle_sprites_up.Add(alt_idle_sprites_up[i]); }
            for(int i=0; i<alt_idle_sprites_side.Count; i++) { idle_sprites_side.Add(alt_idle_sprites_side[i]); }
            for(int i=0; i<alt_idle_sprites_down.Count; i++) { idle_sprites_down.Add(alt_idle_sprites_down[i]); }
            for(int i=0; i<alt_walk_sprites_up.Count; i++) { walk_sprites_up.Add(alt_walk_sprites_up[i]); }
            for(int i=0; i<alt_walk_sprites_side.Count; i++) { walk_sprites_side.Add(alt_walk_sprites_side[i]); }
            for(int i=0; i<alt_walk_sprites_down.Count; i++) { walk_sprites_down.Add(alt_walk_sprites_down[i]); }

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
