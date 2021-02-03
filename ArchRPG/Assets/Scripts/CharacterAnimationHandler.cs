using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    public bool moving;
    public bool was_moving;
    public float idle_anim_delay;
    public float walk_anim_delay;

    [SerializeField]
    public List<Sprite> idle_sprites_up,
        idle_sprites_side,
        idle_sprites_down;

    [SerializeField]
    public List<Sprite> walk_sprites_up,
        walk_sprites_side,
        walk_sprites_down;

    private int index;
    private float idle_delay;
    private float walk_delay;

    private SpriteRenderer rend;

    public void Start()
    {
        index = 0;
        idle_delay = idle_anim_delay;
        idle_anim_delay = 0;
        walk_delay = walk_anim_delay;
        walk_anim_delay = 0;
        rend = GetComponent<SpriteRenderer>();
    }

    //chuck this function into an update function and it will animate the character
    //directions: 0 = up, 1 = right, 2 = down, 3 = left
    public void Animate(int direction)
    {
        //idle
        if (!moving)
        {
            //start on the starting frame
            if (was_moving)
            {
                was_moving = false;
                index = 0;
                switch (direction) {
                    case 0:
                        rend.flipX = false;
                        rend.sprite = idle_sprites_up[index];
                        break;
                    case 1:
                        rend.flipX = false;
                        rend.sprite = idle_sprites_side[index];
                        break;
                    case 2:
                        rend.flipX = false;
                        rend.sprite = idle_sprites_down[index];
                        break;
                    case 3:
                        rend.flipX = true;
                        rend.sprite = idle_sprites_side[index];
                        break;
                    default:
                        break;
                }
                return;
            }

            if(idle_anim_delay >= idle_delay)
            {
                index++;
                //check the direction and change the idlesprite accordingly
                switch (direction)
                {
                    case 0:
                        if (index >= idle_sprites_up.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = idle_sprites_up[index];
                        break;
                    case 1:
                        if (index >= idle_sprites_side.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = idle_sprites_side[index];
                        break;
                    case 2:
                        if (index >= idle_sprites_down.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = idle_sprites_down[index];
                        break;
                    case 3:
                        if (index >= idle_sprites_side.Count) index = 0;
                        rend.flipX = true;
                        rend.sprite = idle_sprites_side[index];
                        break;
                    default:
                        break;
                }
                idle_anim_delay = 0f;
                return;
            }
            else
            {
                //after a delay change the idlesprite
                idle_anim_delay += Time.deltaTime;
            }
        }
        //walking
        else
        {
            //start on the starting frame
            if (!was_moving)
            {
                was_moving = true;
                index = 0;
                switch (direction)
                {
                    case 0:
                        rend.flipX = false;
                        rend.sprite = walk_sprites_up[index];
                        break;
                    case 1:
                        rend.flipX = false;
                        rend.sprite = walk_sprites_side[index];
                        break;
                    case 2:
                        rend.flipX = false;
                        rend.sprite = walk_sprites_down[index];
                        break;
                    case 3:
                        rend.flipX = true;
                        rend.sprite = walk_sprites_side[index];
                        break;
                    default:
                        break;
                }
                return;
            }

            //after a delay change the walksprite
            if(walk_anim_delay >= walk_delay)
            {
                index++;
                //check the direction and change the walksprite accordingly
                switch (direction)
                {
                    case 0:
                        if (index >= walk_sprites_up.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = walk_sprites_up[index];
                        break;
                    case 1:
                        if (index >= walk_sprites_side.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = walk_sprites_side[index];
                        break;
                    case 2:
                        if (index >= walk_sprites_down.Count) index = 0;
                        rend.flipX = false;
                        rend.sprite = walk_sprites_down[index];
                        break;
                    case 3:
                        if (index >= walk_sprites_side.Count) index = 0;
                        rend.flipX = true;
                        rend.sprite = walk_sprites_side[index];
                        break;
                    default:
                        break;
                }
                walk_anim_delay = 0f;
                return;
            }
            else
            {
                walk_anim_delay += Time.deltaTime;
            }
        }
    }
}
