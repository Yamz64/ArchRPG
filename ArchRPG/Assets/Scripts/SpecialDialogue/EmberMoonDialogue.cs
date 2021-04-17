using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberMoonDialogue : NPCDialogue
{
    private bool interacted;
    private bool has_guitar;

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
        //see if Ember Moon has been interacted with before
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        interacted = data.GetInteracted("EmberMoon");
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

        //see if the player has the rad guitar
        PlayerData player_data = player.GetComponent<PlayerDataMono>().data;
        for(int i=0; i<player_data.GetInventorySize(); i++)
        {
            if(player_data.GetItem(i).name == "Rad Guitar")
            {
                has_guitar = true;
                break;
            }
        }

    }

    public override void Interact()
    {
        if (!has_guitar)
        {
            base.Interact();
        }
        else
        {
            interacted = true;

            if (player.GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
            {
                PlayerData data = player.GetComponent<PlayerDataMono>().data;
                data.AddPartyMember(new EmberMoon());
                data.GetPartyMember(data.GetPartySize() - 1).SetHP(data.GetPartyMember(data.GetPartySize() - 1).GetHPMAX());
                data.GetPartyMember(data.GetPartySize() - 1).SetSP(data.GetPartyMember(data.GetPartySize() - 1).GetSPMax());
                data.UnlockPartyMember(9);
                alt_converted_text.Add("Ember Moon joins the party!");
                alt_container.Add(new EffectContainer());
                alt_converted_images.Add(null);
            }
            else
            {
                player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(9);
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
