using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatDogConsumerDialogue : NPCDialogue
{
    [SerializeField]
    public List<ExpandedString> alt_text;
    [SerializeField]
    public List<EffectContainer> alt_container;
    [SerializeField]
    public List<DialogueImages> alt_dialogueImages;
    
    [SerializeField]
    public List<ExpandedString> no_text;
    [SerializeField]
    public List<EffectContainer> no_container;
    [SerializeField]
    public List<DialogueImages> no_dialogueImages;

    private List<ExpandedString> stored_text;
    private List<EffectContainer> stored_container;
    private List<DialogueImages> stored_dialogueImages;

    private bool has_dogs;
    private bool interacted;

    void SetInitialDialogue()
    {
        text.Clear();
        container.Clear();
        dialogueImages.Clear();
        for (int i = 0; i < stored_text.Count; i++) { text.Add(new ExpandedString(stored_text[i].text)); }
        for (int i = 0; i < stored_container.Count; i++) { container.Add(new EffectContainer(stored_container[i])); }
        for (int i = 0; i < stored_dialogueImages.Count; i++) { dialogueImages.Add(new DialogueImages(stored_dialogueImages[i].sprite_path, stored_dialogueImages[i].lines)); }
        base.Start();
    }

    void SetPostDialogue()
    {
        text.Clear();
        container.Clear();
        dialogueImages.Clear();
        for (int i = 0; i < alt_text.Count; i++) { text.Add(new ExpandedString(alt_text[i].text)); }
        for (int i = 0; i < alt_container.Count; i++) { container.Add(new EffectContainer(alt_container[i])); }
        for (int i = 0; i < alt_dialogueImages.Count; i++) { dialogueImages.Add(new DialogueImages(alt_dialogueImages[i].sprite_path, alt_dialogueImages[i].lines)); }
        base.Start();
    }

    void SetNoDogDialogue()
    {
        text.Clear();
        container.Clear();
        dialogueImages.Clear();
        for (int i = 0; i < no_text.Count; i++) { text.Add(new ExpandedString(no_text[i].text)); }
        for (int i = 0; i < no_container.Count; i++) { container.Add(new EffectContainer(no_container[i])); }
        for (int i = 0; i < no_dialogueImages.Count; i++) { dialogueImages.Add(new DialogueImages(no_dialogueImages[i].sprite_path, no_dialogueImages[i].lines)); }
        base.Start();
    }

    // Start is called before the first frame update
    new void Start()
    {
        PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
        //find if the recruiter has been interacted
        bool met_recruiter = false;
        MapSaveData map_save_data = new MapSaveData();
        map_save_data.Load();
        for(int i=0; i<map_save_data.map_data.Count; i++)
        {
            if(map_save_data.map_data[i].name == "City2")
            {
                for(int j=0; j<map_save_data.map_data[i].objects.Count; j++)
                {
                    if(map_save_data.map_data[i].objects[j].o == "Recruiter" && map_save_data.map_data[i].objects[j].interacted)
                    {
                        met_recruiter = true;
                        break;
                    }
                }
                break;
            }
        }

        for(int i=0; i<data.GetInventorySize(); i++)
        {
            if(data.GetItem(i).name == "Meat Dog" && met_recruiter)
            {
                has_dogs = true;
                break;
            }
        }

        MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
        interacted = map_data.GetInteracted("MeatdogEnthusiast");

        //store original set values
        stored_text = new List<ExpandedString>();
        stored_container = new List<EffectContainer>();
        stored_dialogueImages = new List<DialogueImages>();

        for(int i=0; i<text.Count; i++) { stored_text.Add(new ExpandedString(text[i].text)); }
        for(int i=0; i<container.Count; i++) { stored_container.Add(new EffectContainer(container[i])); }
        for(int i=0; i<dialogueImages.Count; i++) { stored_dialogueImages.Add(new DialogueImages(dialogueImages[i].sprite_path, dialogueImages[i].lines)); }
    }

    public override void Interact()
    {
        //determine which dialogue set to use
        if (interacted)
        {
            SetPostDialogue();
            base.Interact();
        }
        else if (!has_dogs)
        {
            SetNoDogDialogue();
            base.Interact();
        }
        else
        {
            SetInitialDialogue();
            interacted = true;
            MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            map_data.SetInteracted("MeatdogEnthusiast", true);
            PlayerData data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
            data.RemoveItem("Meat Dog");
            map_data.Save();
            base.Interact();
        }
    }
}
