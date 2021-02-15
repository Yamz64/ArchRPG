﻿using System.Collections;
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

    IEnumerator Battle()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true; 
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    private void Start()
    {
        true_roll_max = roll_delay;
        if (roll_min > true_roll_max) roll_min = roll_max;
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
                            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
                            switch (enemy_names.Count)
                            {
                                //1 enemy
                                case 1:
                                    data.SaveEnemyNames(enemy_names[0]);
                                    break;
                                //2 enemies (pick how many spawn and then pick a random of the 2
                                case 2:
                                    int enemy_num = Random.Range(0, 2);
                                    if(enemy_num == 0)
                                    {
                                        int first_type = Random.Range(0, 2);
                                        data.SaveEnemyNames(enemy_names[first_type]);
                                    }
                                    else
                                    {
                                        int first_type = Random.Range(0, 2);
                                        int second_type = Random.Range(0, 2);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type]);
                                    }
                                    break;
                                case 3:
                                    enemy_num = Random.Range(0, 3);
                                    if (enemy_num == 0)
                                    {
                                        int first_type = Random.Range(0, 3);
                                        data.SaveEnemyNames(enemy_names[first_type]);
                                    }
                                    else if(enemy_num == 1)
                                    {
                                        int first_type = Random.Range(0, 2);
                                        int second_type = Random.Range(0, 2);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type]);
                                    }
                                    else
                                    {
                                        int first_type = Random.Range(0, 3);
                                        int second_type = Random.Range(0, 3);
                                        int third_type = Random.Range(0, 3);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type], enemy_names[third_type]);
                                    }
                                    break;
                                case 4:
                                    enemy_num = Random.Range(0, 4);
                                    if (enemy_num == 0)
                                    {
                                        int first_type = Random.Range(0, 4);
                                        data.SaveEnemyNames(enemy_names[first_type]);
                                    }
                                    else if (enemy_num == 1)
                                    {
                                        int first_type = Random.Range(0, 2);
                                        int second_type = Random.Range(0, 2);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type]);
                                    }
                                    else if(enemy_num == 2)
                                    {
                                        int first_type = Random.Range(0, 3);
                                        int second_type = Random.Range(0, 3);
                                        int third_type = Random.Range(0, 3);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type], enemy_names[third_type]);
                                    }
                                    else
                                    {
                                        int first_type = Random.Range(0, 4);
                                        int second_type = Random.Range(0, 4);
                                        int third_type = Random.Range(0, 4);
                                        int fourth_type = Random.Range(0, 4);
                                        data.SaveEnemyNames(enemy_names[first_type], enemy_names[second_type], enemy_names[third_type], enemy_names[fourth_type]);
                                    }
                                    break;
                            }

                            //start battle transition and load into the battle
                            data.active_scene = SceneManager.GetActiveScene().name;
                            data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                            data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                            StartCoroutine(Battle());
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
