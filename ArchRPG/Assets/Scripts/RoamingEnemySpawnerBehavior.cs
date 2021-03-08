using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingEnemySpawnerBehavior : MonoBehaviour
{
    public float move_speed;
    public float aggro_range;
    public float attack_delay;

    [Tooltip("Should this spawner's enemy be a hoard type encounter?")]
    public bool hoard;

    [SerializeField]
    public Sprite[] anim_sprites;
    [SerializeField]
    public List<string> enemy_names;
    public Object enemy_prefab;

    // Start is called before the first frame update
    void Start()
    {
        //determine if conditions to spawn the enemy in are favorable (must be outside of the camera's viewport)
        bool favorable = false;
        Vector3 screen_position = Camera.main.WorldToScreenPoint(transform.position);
        //outside right bounds
        if (screen_position.x > Screen.width) favorable = true;
        //outside left bounds
        if (screen_position.x < 0.0f) favorable = true;
        //outside top bounds
        if (screen_position.y > Screen.height) favorable = true;
        //outside bottom bounds
        if (screen_position.y < 0.0f) favorable = true;

        if (favorable)
        {
            //spawn the prefab and set it's values to what it should be 
            GameObject enemy = (GameObject)Instantiate(enemy_prefab, transform.position, transform.rotation);
            //initialize the sprites
            enemy.GetComponent<NPCAnimationHandler>().idle_sprites_up.Clear();
            for(int i=0; i<anim_sprites.Length; i++) { enemy.GetComponent<NPCAnimationHandler>().idle_sprites_up.Add(anim_sprites[i]); }
            //initialize enemy spawn list
            enemy.GetComponent<OverworldEncounter>().enemy_names.Clear();
            for(int i=0; i<enemy_names.Count; i++) { enemy.GetComponent<OverworldEncounter>().enemy_names.Add(enemy_names[i]); }
            //determine the type of encounter
            enemy.GetComponent<OverworldEncounter>().SetEncounterType(hoard);
            //set other variables
            enemy.GetComponent<OverworldEncounter>().move_speed = move_speed;
            enemy.GetComponent<OverworldEncounter>().aggro_range = aggro_range;
            enemy.GetComponent<OverworldEncounter>().attack_delay = attack_delay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, "e_mobile_sprout.png", true);
    }
}
