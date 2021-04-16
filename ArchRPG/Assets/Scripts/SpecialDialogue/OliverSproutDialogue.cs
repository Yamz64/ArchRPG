using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliverSproutDialogue : NPCDialogue
{
    private bool interacted;
    private bool bouncer_beaten;

    [SerializeField]
    public List<ExpandedString> alt_text;
    [SerializeField]
    public List<EffectContainer> alt_container;
    [SerializeField]
    public List<DialogueImages> alt_dialogueImages;
    private List<string> alt_converted_text;
    private List<string> alt_converted_images;

    IEnumerator LateDestroy()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetActive() == false);

        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>().SetInteracted("Oliver Sprout", true);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>().Save();

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    new void Start()
    {
        //see if Oliver has been interacted with before
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        interacted = data.GetInteracted("Oliver Sprout");
        if (interacted) Destroy(gameObject);

        base.Start();

        alt_converted_text = new List<string>();
        for (int i = 0; i < alt_text.Count; i++)
        {
            alt_converted_text.Add(alt_text[i].text);
        }

        alt_converted_images = new List<string>();
        for (int i = 0; i < alt_dialogueImages.Count; i++)
        {
            for (int j = 0; j < alt_dialogueImages[i].lines; j++)
            {
                alt_converted_images.Add(alt_dialogueImages[i].sprite_path);
            }
        }

        //see if the bouncer has been defeated
        bouncer_beaten = false;
        MapSaveData map_save_data = new MapSaveData();
        map_save_data.Load();
        for(int i=0; i<map_save_data.map_data.Count; i++)
        {
            if(map_save_data.map_data[i].name == "NightclubHallway")
            {
                for (int j = 0; j < map_save_data.map_data[i].objects.Count; j++)
                {
                    if (map_save_data.map_data[i].objects[j].o == "Bouncer" && map_save_data.map_data[i].objects[j].interacted)
                    {
                        bouncer_beaten = true;
                        break;
                    }
                }
                break;
            }
        }
    }

    public override void Interact()
    {
        if (!bouncer_beaten)
        {
            base.Interact();
        }
        else
        {
            interacted = true;

            if (player.GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
            {
                PlayerData data = player.GetComponent<PlayerDataMono>().data;
                data.AddPartyMember(new OliverSprout());
                data.GetPartyMember(data.GetPartySize() - 1).SetHP(data.GetPartyMember(data.GetPartySize() - 1).GetHPMAX());
                data.GetPartyMember(data.GetPartySize() - 1).SetSP(data.GetPartyMember(data.GetPartySize() - 1).GetSPMax());
                data.UnlockPartyMember(8);
                alt_converted_text.Add("Oliver joins the party!");
                alt_container.Add(new EffectContainer());
                alt_converted_images.Add(null);
            }
            else
            {
                player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(8);
                alt_converted_text.Add("You do not have enough room in your party");
                alt_container.Add(new EffectContainer());
                alt_converted_images.Add(null);
            }

            player.OpenTextBox();
            player.SetWriteQueue(alt_converted_text);
            player.SetEffectQueue(alt_container);
            player.SetImageQueue(alt_converted_images);
            player.WriteDriver();

            StartCoroutine(LateDestroy());
        }
    }
}
