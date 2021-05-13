using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GD.MinMaxSlider;

public class OverworldEncounter : MonoBehaviour
{
    [System.Serializable]
    public struct EncounterObject
    {
        public string enemy_name;
        public int encounter_priority;
        public float xp_factor;
    }

    private enum EncounterType { SINGLE, HOARD };
    [SerializeField]
    private EncounterType encounter_type;

    public float move_speed;
    public float aggro_range;
    public float attack_delay;

    [SerializeField]
    public List<EncounterObject> enemy_names;
    
    [MinMaxSlider(1, 4)]
    public Vector2Int maximum_enemies;

    private float stored_delay;

    [HideInInspector]
    public bool initiated_combat;

    private Rigidbody2D rb;
    private NPCAnimationHandler anim;

    public void SetEncounterType(bool type)
    {
        if (!type) encounter_type = EncounterType.SINGLE;
        else encounter_type = EncounterType.HOARD;
    }

    IEnumerator CombatSequence()
    {
        TransitionHandler handler = GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>();
        handler.BattleTransitionDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>().pause_menu_protection = true;
        yield return new WaitUntil(() => handler.transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        stored_delay = attack_delay;

        initiated_combat = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<NPCAnimationHandler>();

        //remove enemies that exceed 4 enemies
        while(enemy_names.Count > 4)
        {
            enemy_names.RemoveAt(enemy_names.Count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //see if the player is within the aggro_range
        if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) < aggro_range &&
            !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().intangible)
        {
            anim.speedup = true;
            //begin decrementing a timer after which the enemy will chase down the player
            if (attack_delay > 0.0f) attack_delay -= Time.deltaTime;
        }
        else
        {
            attack_delay = stored_delay;
            anim.speedup = false;
            rb.velocity = Vector3.zero;
        }

        if (attack_delay < 0.0f && !initiated_combat) rb.velocity = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized * move_speed;
        if (initiated_combat) rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (!other.GetComponent<PlayerMovement>().intangible)
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
                    for (int i = 0; i < enemy_names.Count; i++)
                    {
                        for (int j = 0; j < enemy_names[i].encounter_priority; j++)
                        {
                            pool_size++;
                        }
                    }
                    string[] enemy_pool = new string[pool_size];
                    float[] xp_factor_pool = new float[pool_size];
                    int index = 0;
                    for (int i = 0; i < enemy_names.Count; i++)
                    {
                        for (int j = 0; j < enemy_names[i].encounter_priority; j++)
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

                data.active_scene = SceneManager.GetActiveScene().name;
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                PlayerPrefs.SetFloat("_xp_factor_", average_xp_factor);
                if (!initiated_combat)
                {
                    StartCoroutine(CombatSequence());
                    initiated_combat = true;
                }
            }
        }
    }
}
