using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCBackstageTimDialogue : NPCDialogue
{
    [SerializeField]
    public Item jm_id;

    private bool alternate_dialogue;
    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (GameObject.Find("DiscoHooligans").transform.childCount <= 0) alternate_dialogue = true;
        else alternate_dialogue = false;

        if (alternate_dialogue)
        {
            //see if Tim has been interacted with if so then destroy tim
            bool interacted = false;
            MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for(int i=0; i<map_data.current_map.objects.Count; i++)
            {
                if(map_data.current_map.objects[i].o == "Tim" && map_data.current_map.objects[i].interacted)
                {
                    interacted = true;
                    break;
                }
            }

            if (interacted) { Destroy(gameObject); }
            //If Tim has not been interacted with then change Tim's dialogue up
            else
            {
                text.Clear();
                container.Clear();
                dialogueImages.Clear();

                //--DIALOGUE--
                text.Add(new ExpandedString("Holy cow pal!"));
                text.Add(new ExpandedString("You really gave those old guys a knuckle sandwich!"));
                text.Add(new ExpandedString("..."));
                text.Add(new ExpandedString("Oh yeah, I almost forgot, here's your Junior Goodmeat ID, you've earned it"));
                text.Add(new ExpandedString("You got a Junior Meat ID!"));
                text.Add(new ExpandedString("Can I maybe hang out with you for a bit?"));
                text.Add(new ExpandedString("I can really pull my weight with my supply of discount Goodmeat cookout supplies!"));
                text.Add(new ExpandedString("Besides if I'm in the company of a strong fella like you,"));
                text.Add(new ExpandedString("I don't have to worry about getting kidnapped again!"));
                text.Add(new ExpandedString("Uh... sure."));

                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
                {
                    text.Add(new ExpandedString("Tim joins the party!"));
                }
                else
                {
                    text.Add(new ExpandedString("You do not have enough room in your party"));
                }

                //--EFFECT--
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                TextEffectClass effect = new TextEffectClass();
                effect.name = "Color";
                effect.color = Color.yellow;
                effect.lower = 7;
                effect.upper = 19;
                EffectContainer temp_container = new EffectContainer();
                temp_container.effects.Add(new TextEffectClass(effect));
                container.Add(new EffectContainer(temp_container));
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());
                container.Add(new EffectContainer());

                //--IMAGE--
                dialogueImages.Add(new DialogueImages("CharacterSprites/Tim_sad", 4));
                dialogueImages.Add(new DialogueImages(null));
                dialogueImages.Add(new DialogueImages("CharacterSprites/Tim_sad", 4));
                dialogueImages.Add(new DialogueImages("CharacterSprites/PC"));
                dialogueImages.Add(new DialogueImages(null));
            }
        }

        base.Start();
    }

    private IEnumerator TimSequence()
    {
        yield return new WaitForEndOfFrame();
        MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        for(int i=0; i<map_data.current_map.objects.Count; i++)
        {
            if(map_data.current_map.objects[i].o == "Tim")
            {
                map_data.current_map.objects[i].interacted = true;
                map_data.Save();
                break;
            }
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
        {
            PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
            data.AddPartyMember(new Tim());
            data.GetPartyMember(data.GetPartySize() - 1).SetHP(data.GetPartyMember(data.GetPartySize() - 1).GetHPMAX());
            data.GetPartyMember(data.GetPartySize() - 1).SetSP(data.GetPartyMember(data.GetPartySize() - 1).GetSPMax());
            data.UnlockPartyMember(6);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.UnlockPartyMember(4);
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data.AddItem(new Item(jm_id));

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => !player.GetActive());
        Destroy(gameObject);
    }

    private new void Start()
    {
        StartCoroutine(LateStart());
    }

    public override void Interact()
    {
        base.Interact();
        if (alternate_dialogue) StartCoroutine(TimSequence());
    }
}
