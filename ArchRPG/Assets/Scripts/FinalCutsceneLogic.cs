using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutsceneLogic : NPCDialogue
{
    public List<SpeechObject> dialogues;

    private int ending;               //determines which sequence to use in the final cutscene based on how the player has played
                                      //0 = good, 1 = normal, 2 = bad, 3 = doomed

    IEnumerator AnimationSequence()
    {
        //wait for player to finish initial dialogue
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);
        yield return new WaitForEndOfFrame();

        player.GetComponent<PlayerMovement>().interaction_protection = true;

        //determine the correct cutscene outcome
        switch (ending)
        {
            //good
            case 0:

                break;
            //normal
            case 1:
                break;
            //bad
            case 2:
                break;
            //doomed
            case 3:
                break;
            //if for some bizarre reason the ending is none of these default to the good ending
            default:
                break;
        }
    }

    private new void Start()
    {
        base.Start();

        //determine what ending to use
        PlayerData data = player.GetComponent<PlayerDataMono>().data;

        //check to see if the player has spent at least 3 EP on an ability
        int combined_cost = 0;
        bool has_eldritch = false;
        List<Ability> e_abilities = EldritchAbilities.GetAll();
        for(int i=0; i<data.GetAbilityCount(); i++)
        {
            for(int j=0; j<e_abilities.Count; j++)
            {
                if(data.GetAbility(i).name == e_abilities[j].name)
                {
                    combined_cost += e_abilities[j].level_cost;
                    break;
                }
            }
            if (combined_cost > 3)
            {
                has_eldritch = true;
                break;
            }
        }

        //GOOD
        if (data.GetEP() <= 0 && !has_eldritch) ending = 0;
        //NORMAL
        else if (!has_eldritch) ending = 1;
        //BAD
        else if (has_eldritch) ending = 2;
        //DOOMED
        else if (has_eldritch && data.GetStatus(25) > 0) ending = 3;
    }

    public override void Interact()
    {
        base.Interact();

    }
}
