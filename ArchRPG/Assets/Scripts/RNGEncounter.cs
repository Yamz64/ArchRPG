using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RNGEncounter : MonoBehaviour
{
    private enum EncounterType { SINGLE, HOARD };
    [SerializeField]
    private EncounterType encounter_type;

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
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity.magnitude != 0 && 
                !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().intangible)
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
                            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
                            if (encounter_type == EncounterType.SINGLE)
                            {
                                data.SaveEnemyNames(enemy_names[0]);
                            }
                            else
                            {
                                //first figure out how many enemies to spawn
                                int enemy_count = Random.Range(1, 5);
                                string[] chosen_enemies = new string[enemy_count];

                                //pick random enemies from the list of spawnable enemies and save them to the chosen enemies
                                for (int i = 0; i < enemy_count; i++)
                                {
                                    int rand_enemy = Random.Range(0, enemy_names.Count);
                                    chosen_enemies[i] = enemy_names[rand_enemy];
                                }

                                switch (enemy_count)
                                {
                                    case 1:
                                        data.SaveEnemyNames(chosen_enemies[0]);
                                        break;
                                    case 2:
                                        data.SaveEnemyNames(chosen_enemies[0], chosen_enemies[1]);
                                        break;
                                    case 3:
                                        data.SaveEnemyNames(chosen_enemies[0], chosen_enemies[1], chosen_enemies[2]);
                                        break;
                                    case 4:
                                        data.SaveEnemyNames(chosen_enemies[0], chosen_enemies[1], chosen_enemies[2], chosen_enemies[3]);
                                        break;
                                }
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
