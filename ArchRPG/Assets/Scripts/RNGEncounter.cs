using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GD.MinMaxSlider;

public class RNGEncounter : MonoBehaviour
{
    [System.Serializable]
    public struct EncounterObject
    {
        public string enemy_name;
        public int encounter_priority;
        [Range(1.0f, Mathf.Infinity)]
        public float xp_factor;
    }

    private enum EncounterType { SINGLE, HOARD };
    [SerializeField]
    private EncounterType encounter_type;

    [SerializeField]
    public List<EncounterObject> enemy_names;

    [MinMaxSlider(1, 4)]
    public Vector2Int maximum_enemies;

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
                            float average_xp_factor = 1f;
                            CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
                            if (encounter_type == EncounterType.SINGLE)
                            {
                                data.SaveEnemyNames(enemy_names[0].enemy_name);
                            }
                            else
                            {
                                //first figure out how many enemies to spawn
                                int enemy_count = Random.Range(maximum_enemies[0], maximum_enemies[1] + 1);

                                //create a pool of enemies
                                int pool_size = 0;
                                for(int i=0; i<enemy_names.Count; i++) {
                                    for (int j = 0; j < enemy_names[i].encounter_priority; j++)
                                    {
                                        pool_size++;
                                    }
                                }
                                string[] enemy_pool = new string[pool_size];
                                float[] xp_factor_pool = new float[pool_size];
                                int index = 0;
                                for(int i=0; i<enemy_names.Count; i++) {
                                    for(int j=0; j<enemy_names[i].encounter_priority; j++)
                                    {
                                        enemy_pool[index] = enemy_names[i].enemy_name;
                                        xp_factor_pool[index] = enemy_names[i].xp_factor;
                                        index++;
                                    }
                                }

                                //shuffle the pool
                                string stored_name = "";
                                float stored_factor = 0f;
                                for (int i = 0; i < enemy_pool.GetLength(0); i++)
                                {
                                    int random_index = Random.Range(0, enemy_pool.GetLength(0));
                                    stored_name = enemy_pool[random_index];
                                    stored_factor = xp_factor_pool[random_index];
                                    enemy_pool[random_index] = enemy_pool[i];
                                    xp_factor_pool[random_index] = xp_factor_pool[i];
                                    enemy_pool[i] = stored_name;
                                    xp_factor_pool[i] = stored_factor;
                                }

                                int first = 0, second = 1, third = 2, fourth = 3;

                                if (second > pool_size) second = Random.Range(0, pool_size);
                                if (third > pool_size) third = Random.Range(0, pool_size);
                                if (fourth > pool_size) fourth = Random.Range(0, pool_size);


                                //add enemies to the save file
                                switch (enemy_count)
                                {
                                    case 1:
                                        data.SaveEnemyNames(enemy_pool[first]);
                                        average_xp_factor = xp_factor_pool[first];
                                        break;
                                    case 2:
                                        data.SaveEnemyNames(enemy_pool[first], enemy_pool[second]);
                                        average_xp_factor = (xp_factor_pool[first] + xp_factor_pool[second]) / 2f;
                                        break;
                                    case 3:
                                        data.SaveEnemyNames(enemy_pool[first], enemy_pool[second], enemy_pool[third]);
                                        average_xp_factor = (xp_factor_pool[first] + xp_factor_pool[second] + xp_factor_pool[third]) / 3f;
                                        break;
                                    case 4:
                                        data.SaveEnemyNames(enemy_pool[first], enemy_pool[second], enemy_pool[third], enemy_pool[fourth]);
                                        average_xp_factor = (xp_factor_pool[first] + xp_factor_pool[second] + xp_factor_pool[third] + xp_factor_pool[fourth]) / 4f;
                                        break;
                                }
                            }

                            //start battle transition and load into the battle
                            data.active_scene = SceneManager.GetActiveScene().name;
                            data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                            data.Save(PlayerPrefs.GetInt("_active_save_file_"), false, average_xp_factor);
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
