using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscoHooligansDialogue : NPCDialogue
{
    IEnumerator StartFight()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => !player.GetActive());

        //begin fight with the player
        CharacterStatJsonConverter data = new CharacterStatJsonConverter(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data);
        data.SaveEnemyNames("Dylan", "Dan", "Brian");
        data.active_scene = SceneManager.GetActiveScene().name;
        data.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        data.Save(PlayerPrefs.GetInt("_active_save_file_"));
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    // Start is called before the first frame update
    new void Start()
    {
        //see if the hooligans have been interacted with
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<data.current_map.objects.Count; i++)
        {
            if(data.current_map.objects[i].o == "DiscoHooligans" && data.current_map.objects[i].interacted)
            {
                //check to see if the player just fought the hooligans
                CharacterStatJsonConverter save = new CharacterStatJsonConverter(PlayerPrefs.GetInt("_active_save_file_"));
                if(save.enemy_names.Length > 0)
                {
                    if(save.enemy_names[0] == "Dylan" && save.enemy_names[1] == "Dan" && save.enemy_names[2] == "Brian")
                    {
                        //player fled from the hooligan's combat so mark them as not interacted
                        if (save.flee)
                        {
                            data.current_map.objects[i].interacted = false;
                            data.Save();
                        }
                        //player didn't flee from the hooligans so destroy them
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
            if (map_data.current_map.objects[i].o == "DiscoHooligans")
            {
                map_data.current_map.objects[i].interacted = true;
                map_data.Save();
                break;
            }
        }
        base.Interact();
        StartCoroutine(StartFight());
    }
}
