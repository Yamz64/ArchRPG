using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldEncounter : MonoBehaviour
{
    private enum EncounterType { SINGLE, HOARD };
    [SerializeField]
    private EncounterType encounter_type;

    public float move_speed;
    public float aggro_range;
    public float attack_delay;

    [SerializeField]
    public List<string> enemy_names;

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

                data.active_scene = SceneManager.GetActiveScene().name;
                data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                data.Save(PlayerPrefs.GetInt("_active_save_file_"));
                if (!initiated_combat)
                {
                    StartCoroutine(CombatSequence());
                    initiated_combat = true;
                }
            }
        }
    }
}
