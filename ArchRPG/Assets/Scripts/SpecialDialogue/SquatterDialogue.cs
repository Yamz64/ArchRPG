using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SquatterDialogue : NPCDialogue
{
    IEnumerator SquatterSequence()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => !player.GetActive());

        //begin fight with the player
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.SaveEnemyNames("The Squatter");
        data.active_scene = SceneManager.GetActiveScene().name;
        data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    // Start is called before the first frame update
    public new void Start()
    {
        //see if the squatter has been interacted with
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < data.current_map.objects.Count; i++)
        {
            if (data.current_map.objects[i].o == "The Squatter" && data.current_map.objects[i].interacted)
            {
                //check to see if the player just fought the squatter
                CharacterStatJsonConverter save = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
                if (save.enemy_names.Length > 0)
                {
                    if (save.enemy_names[0] == "The Squatter")
                    {
                        //player fled from the squatter's combat so mark them as not interacted
                        if (save.flee)
                        {
                            data.current_map.objects[i].interacted = false;
                            data.Save();
                        }
                        //player didn't flee from the squatter so destroy the attached gameobject
                        else
                        {
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
                break;
            }
        }
        base.Start();
    }

    public override void Interact()
    {
        MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for (int i = 0; i < map_data.current_map.objects.Count; i++)
        {
            if (map_data.current_map.objects[i].o == "The Squatter")
            {
                map_data.current_map.objects[i].interacted = true;
                map_data.Save();
                break;
            }
        }
        base.Interact();

        //if the player has boxed pizza remove it
        for(int i=0; i < player.GetComponent<PlayerDataMono>().data.GetInventorySize(); i++)
        {
            if(player.GetComponent<PlayerDataMono>().data.GetItem(i).name == "Boxed Pizza")
            {
                player.GetComponent<PlayerDataMono>().data.RemoveItem(i);
                break;
            }
        }

        StartCoroutine(SquatterSequence());
    }
}
