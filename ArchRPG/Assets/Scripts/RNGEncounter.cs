using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RNGEncounter : MonoBehaviour
{
    [SerializeField]
    public List<string> enemy_names;

    public float roll_delay;
    public float roll_min;
    [Range(0.0f, 100.0f)]
    public float enemy_encounter_chance;

    private bool danger;
    private float roll_max;
    private float true_roll_max;

    private void Start()
    {
        true_roll_max = roll_delay;
        if (roll_min > roll_max) roll_min = roll_max;
        roll_max = Random.Range(roll_min, true_roll_max);
        roll_delay = 0;
        danger = false;

        //ensure only 4 enemies per encounter
        if(enemy_names != null)
        {
            while(enemy_names.Count > 4)
            {
                enemy_names.RemoveAt(enemy_names.Count - 1);
            }
        }
    }

    private void FixedUpdate()
    {
        //player has a chance of encountering an enemy
        if (danger)
        {
            //if the player is moving decrement the amount of time it takes to encounter an enemy
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.magnitude != 0)
            {
                //if it is not time to see if the player has encountered an enemy decrement the amount of time to reach the enemy
                if(roll_delay < roll_max)
                {
                    roll_delay += Time.deltaTime;
                }
                //time to encounter an enemy
                else
                {
                    //reset the counters and roll a chance to find an enemy
                    roll_max = Random.Range(roll_min, true_roll_max);
                    roll_delay = 0;

                    //enemy encountered
                    if (enemy_names.Count > 0)
                    {
                        if (Random.Range(0.0f, 100.0f) <= enemy_encounter_chance)
                        {
                            //save current player's condition and load the battle scene
                            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>());
                            switch (enemy_names.Count)
                            {
                                case 1:
                                    data.SaveEnemyNames(enemy_names[0]);
                                    break;
                                case 2:
                                    data.SaveEnemyNames(enemy_names[0], enemy_names[1]);
                                    break;
                                case 3:
                                    data.SaveEnemyNames(enemy_names[0], enemy_names[1], enemy_names[2]);
                                    break;
                                case 4:
                                    data.SaveEnemyNames(enemy_names[0], enemy_names[1], enemy_names[2], enemy_names[3]);
                                    break;
                            }
                        }
                    }
                }
            }
        }
        //player doesn't have a chance of encountering an enemy
        else
        {
            roll_delay = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            danger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            danger = false;
        }
    }
}
