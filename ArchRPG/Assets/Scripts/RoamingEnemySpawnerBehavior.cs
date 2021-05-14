using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class RoamingEnemySpawnerBehavior : MonoBehaviour
{
    [System.Serializable]
    public struct EncounterObject
    {
        public string enemy_name;
        public int encounter_priority;
        [Range(1.0f, Mathf.Infinity)]
        public float xp_factor;
    }

    public float move_speed;
    public float aggro_range;
    [Range(0, 1)]
    public float spawn_range = 1;
    public float attack_delay;

    [Tooltip("Should this spawner's enemy be a hoard type encounter?")]
    public bool hoard;

    public bool intangible;

    [SerializeField]
    public Sprite[] anim_sprites;
    [SerializeField]
    public List<EncounterObject> enemy_names;
    public Object enemy_prefab;

    [MinMaxSlider(1, 4)]
    public Vector2Int maximum_enemies;

    // Start is called before the first frame update
    void Start()
    {
        //determine if conditions to spawn the enemy in are favorable (must be outside of the camera's viewport)
        bool favorable = false;
        Vector3 screen_position = Camera.main.WorldToScreenPoint(transform.position);
        spawn_range += 1f;
        spawn_range /= 2f;
        float alt_spawn_range = Mathf.Abs(spawn_range - 1.0f);
        //outside right bounds
        if (screen_position.x > Screen.width * spawn_range) favorable = true;
        //outside left bounds
        if (screen_position.x < 0.0f + Screen.width * alt_spawn_range) favorable = true;
        //outside top bounds
        if (screen_position.y > Screen.height * spawn_range) favorable = true;
        //outside bottom bounds
        if (screen_position.y < 0.0f + Screen.height * alt_spawn_range) favorable = true;

        if (favorable)
        {
            //spawn the prefab and set it's values to what it should be 
            GameObject enemy = (GameObject)Instantiate(enemy_prefab, transform.position, transform.rotation);
            //initialize the sprites
            enemy.GetComponent<NPCAnimationHandler>().idle_sprites_up.Clear();
            for(int i=0; i<anim_sprites.Length; i++) { enemy.GetComponent<NPCAnimationHandler>().idle_sprites_up.Add(anim_sprites[i]); }
            //initialize enemy spawn list
            enemy.GetComponent<OverworldEncounter>().enemy_names.Clear();
            for(int i=0; i<enemy_names.Count; i++) {
                OverworldEncounter.EncounterObject temp = new OverworldEncounter.EncounterObject();
                temp.encounter_priority = enemy_names[i].encounter_priority;
                temp.enemy_name = enemy_names[i].enemy_name;
                temp.xp_factor = enemy_names[i].xp_factor;
                enemy.GetComponent<OverworldEncounter>().enemy_names.Add(temp);
            }
            //determine the type of encounter
            enemy.GetComponent<OverworldEncounter>().SetEncounterType(hoard);
            //set other variables
            enemy.GetComponent<OverworldEncounter>().move_speed = move_speed;
            enemy.GetComponent<OverworldEncounter>().aggro_range = aggro_range;
            enemy.GetComponent<OverworldEncounter>().attack_delay = attack_delay;
            enemy.GetComponent<OverworldEncounter>().maximum_enemies = maximum_enemies;
            if (intangible) enemy.GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawIcon(transform.position, "e_mobile_sprout.png", true);

        //get the width and height of the aggro range
        Vector2 reference_point = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        reference_point *= spawn_range;

        //draw the bounds of the aggro range
        Gizmos.color = Color.green;
        //top border
        Gizmos.DrawLine(new Vector3(-reference_point.x, reference_point.y) + transform.position, new Vector3(reference_point.x, reference_point.y) + transform.position);
        //right border
        Gizmos.DrawLine(new Vector3(reference_point.x, reference_point.y) + transform.position, new Vector3(reference_point.x, -reference_point.y) + transform.position);
        //bottom border
        Gizmos.DrawLine(new Vector3(reference_point.x, -reference_point.y) + transform.position, new Vector3(-reference_point.x, -reference_point.y) + transform.position);
        //left border
        Gizmos.DrawLine(new Vector3(-reference_point.x, -reference_point.y) + transform.position, new Vector3(-reference_point.x, reference_point.y) + transform.position);

    }
}
