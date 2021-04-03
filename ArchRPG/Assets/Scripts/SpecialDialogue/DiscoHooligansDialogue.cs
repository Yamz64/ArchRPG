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
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        StartCoroutine(StartFight());
    }
}
