using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteKnightDialogue : NPCDialogue
{
    private bool interacted;
    private bool lvl_20;

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

        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>().SetInteracted("White Knight", true);
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>().Save();

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    new void Start()
    {
        //see if Ember Moon has been interacted with before
        MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        interacted = data.GetInteracted("White Knight");
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

        //see if the player is level 20
        lvl_20 = player.GetComponent<PlayerDataMono>().data.GetLVL() == 20;
    }

    public override void Interact()
    {
        if (!lvl_20)
        {
            interacted = true;

            if (player.GetComponent<PlayerDataMono>().data.GetPartySize() < 3)
            {
                PlayerData data = player.GetComponent<PlayerDataMono>().data;
                data.AddPartyMember(new WhiteKnight());
                data.GetPartyMember(data.GetPartySize() - 1).SetHP(data.GetPartyMember(data.GetPartySize() - 1).GetHPMAX());
                data.GetPartyMember(data.GetPartySize() - 1).SetSP(data.GetPartyMember(data.GetPartySize() - 1).GetSPMax());
                data.UnlockPartyMember(7);
                alt_converted_text.Add("White Knight joins the party!");
                alt_container.Add(new EffectContainer());
                alt_converted_images.Add(null);
            }
            else
            {
                player.GetComponent<PlayerDataMono>().data.UnlockPartyMember(7);
                alt_converted_text.Add("You do not have enough room in your party");
                alt_container.Add(new EffectContainer());
                alt_converted_images.Add(null);
            }

            base.Interact();

            StartCoroutine(LateDestroy());
        }
        else
        {
            player.OpenTextBox();
            player.SetWriteQueue(alt_converted_text);
            player.SetEffectQueue(alt_container);
            player.SetImageQueue(alt_converted_images);
            player.WriteDriver();
        }
    }
}