using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePointBehavior : InteractableBaseClass
{
    public string save_name;
    private PlayerDialogueBoxHandler dialogue;
    private PauseMenuHandler pause;
    private bool city_sequence;

    IEnumerator SaveSequence()
    {
        //open text box and initialize writing variables
        yield return new WaitForEndOfFrame();
        dialogue.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //see if this is the first time interacting with a save point
        if (PlayerPrefs.GetInt("Saved") == 0)
        {
            //start populating with dialogue
            dialogue_queue.Add("Fish! What are you doing so far from home!");
            dialogue_queue.Add("...");
            dialogue_queue.Add("I see... you must have encountered a rift in space-time as the result of the coming of the end of the world!");
            dialogue_queue.Add("Perhaps we can use this to access and create more favorable timelines to ensure we reach our goal!");
            dialogue_queue.Add("...");
            dialogue_queue.Add("That's right fish, this isn't exactly a Save System,");
            dialogue_queue.Add("some timelines we create might become warped in ways we could not comprehend!");
            dialogue_queue.Add("Blub blub.");

            //effects
            temp.name = "Quake";
            temp.lower = 0;
            temp.upper = 4;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Color";
            temp.color = Color.blue;
            temp.lower = 21;
            temp.upper = 26;
            temp_effect.effects.Add(new TextEffectClass(temp));
            temp.name = "Color";
            temp.color = Color.green;
            temp.lower = 30;
            temp.upper = 35;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Wave";
            temp.lower = 33;
            temp.upper = 43;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Color";
            temp.color = Color.red;
            temp.lower = 21;
            temp.upper = 37;
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            //images
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");

            dialogue.SetWriteQueue(dialogue_queue);
            dialogue.SetEffectQueue(effect_queue);
            dialogue.SetImageQueue(image_queue);
            dialogue.WriteDriver();

            //wait until the dialogue is finished before opening the save menu
            yield return new WaitUntil(() => !dialogue.GetActive());
            yield return new WaitForEndOfFrame();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PauseMenuHandler>().menu_mode = true;
            player.GetComponent<PauseMenuHandler>().menu_input = true;
            player.GetComponent<PauseMenuHandler>().OpenMenu(6);
            player.GetComponent<PauseMenuHandler>().ActivateCursor();
            player.GetComponent<PauseMenuHandler>().UpdateSaveMenu();
            player.GetComponent<PlayerMovement>().interaction_protection = true;

            //heal everyone and restore SP in the party and the player do not heal dead party members unless it's the player
            PlayerData data = player.GetComponent<PlayerDataMono>().data;
            if (data.GetDead()) data.SetDead(false);
            data.SetHP(data.GetHPMAX());
            data.SetSP(data.GetSPMax());
            for (int i = 0; i < data.GetPartySize(); i++)
            {
                if (data.GetPartyMember(i).GetHP() > 0)
                {
                    data.GetPartyMember(i).SetHP(data.GetPartyMember(i).GetHPMAX());
                    data.GetPartyMember(i).SetSP(data.GetPartyMember(i).GetSPMax());
                }
            }

            //remove status effects
            for(int i=0; i<data.GetStatusCount(); i++)
            {
                if (i != 25) data.SetStatus(i, -1);
            }

            //mark savepoints as having been visited
            PlayerPrefs.SetInt("Saved", 1);
        }
        //see if the player should do the city sequence
        else if (city_sequence)
        {
            //start warp sequence dialogue
            dialogue_queue.Add("Fish, I never asked? But how are you consistently able to know where I'm going to be before I get there?");
            dialogue_queue.Add("...");
            dialogue_queue.Add("So due to the ever increasing supernatural occurrences, the distances between the exits of the rift in space time are shrinking?");
            dialogue_queue.Add("...");
            dialogue_queue.Add("Most interesting... so I can use this to Warp between Save Points. This may prove most useful.");
            dialogue_queue.Add("Blub blub");

            //add effects
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "Wave";
            temp.lower = 33;
            temp.upper = 36;
            temp_effect.effects.Add(new TextEffectClass(temp));
            temp.name = "Color";
            temp.color = Color.blue;
            temp.lower = 44;
            temp.upper = 53;
            effect_queue.Add(new EffectContainer(temp_effect));

            temp_effect.effects.Clear();
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            //Add images
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");
            image_queue.Add("CharacterSprites/PC");
            image_queue.Add("CharacterSprites/Fish2");

            //start writing
            dialogue.SetWriteQueue(dialogue_queue);
            dialogue.SetEffectQueue(effect_queue);
            dialogue.SetImageQueue(image_queue);
            dialogue.WriteDriver();

            //mark this object as interacted
            MapDataManager map_data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < map_data.current_map.objects.Count; i++)
            {
                if (map_data.current_map.objects[i].o == "TestSave")
                {
                    map_data.current_map.objects[i].interacted = true;
                    map_data.Save();
                    break;
                }
            }

            //wait until the dialogue is finished before opening the save menu
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => !dialogue.GetActive());
            yield return new WaitForEndOfFrame();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PauseMenuHandler>().warp_unlock = true;
            player.GetComponent<PauseMenuHandler>().menu_mode = true;
            player.GetComponent<PauseMenuHandler>().menu_input = true;
            player.GetComponent<PauseMenuHandler>().OpenMenu(6);
            player.GetComponent<PauseMenuHandler>().ActivateCursor();
            player.GetComponent<PauseMenuHandler>().UpdateSaveMenu();
            player.GetComponent<PlayerMovement>().interaction_protection = true;

            //heal everyone in the party and the player do not heal dead party members unless it's the player
            PlayerData data = player.GetComponent<PlayerDataMono>().data;
            if (data.GetDead()) data.SetDead(false);
            data.SetHP(data.GetHPMAX());
            data.SetSP(data.GetSPMax());
            for (int i = 0; i < data.GetPartySize(); i++)
            {
                if (data.GetPartyMember(i).GetHP() > 0)
                {
                    data.GetPartyMember(i).SetHP(data.GetPartyMember(i).GetHPMAX());
                    data.GetPartyMember(i).SetSP(data.GetPartyMember(i).GetSPMax());
                }
            }

            //remove status effects
            for (int i = 0; i < data.GetStatusCount(); i++)
            {
                if (i != 25) data.SetStatus(i, -1);
            }
        }
        //no special cases
        else
        {
            //have the fish say one line
            dialogue_queue.Add("Blub blub?");
            
            temp.name = "_NO_EFFECT_";
            temp_effect.effects.Add(new TextEffectClass(temp));
            effect_queue.Add(new EffectContainer(temp_effect));

            image_queue.Add("CharacterSprites/Fish2");

            dialogue.SetWriteQueue(dialogue_queue);
            dialogue.SetEffectQueue(effect_queue);
            dialogue.SetImageQueue(image_queue);
            dialogue.WriteDriver();

            //wait until the dialogue is finished before opening the save menu
            yield return new WaitUntil(() => !dialogue.GetActive());
            yield return new WaitForEndOfFrame();

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PauseMenuHandler>().menu_mode = true;
            player.GetComponent<PauseMenuHandler>().menu_input = true;
            player.GetComponent<PauseMenuHandler>().OpenMenu(6);
            player.GetComponent<PauseMenuHandler>().ActivateCursor();
            player.GetComponent<PauseMenuHandler>().UpdateSaveMenu();
            player.GetComponent<PlayerMovement>().interaction_protection = true;

            //heal everyone in the party and the player do not heal dead party members unless it's the player
            PlayerData data = player.GetComponent<PlayerDataMono>().data;
            if (data.GetDead()) data.SetDead(false);
            data.SetHP(data.GetHPMAX());
            data.SetSP(data.GetSPMax());
            for (int i = 0; i < data.GetPartySize(); i++)
            {
                if (data.GetPartyMember(i).GetHP() > 0)
                {
                    data.GetPartyMember(i).SetHP(data.GetPartyMember(i).GetHPMAX());
                    data.GetPartyMember(i).SetSP(data.GetPartyMember(i).GetSPMax());
                }
            }

            //remove status effects
            for (int i = 0; i < data.GetStatusCount(); i++)
            {
                if (i != 25) data.SetStatus(i, -1);
            }
        }
    }

    private void Start()
    {
        //PlayerPrefs.SetInt("Saved", 0);
        dialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();
    }

    public override void Interact()
    {
        //if the scene is the city1 scene, check to see if this object has been interacted with
        city_sequence = false;
        if (SceneManager.GetActiveScene().name == "City1")
        {
            MapDataManager data = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapDataManager>();
            for (int i = 0; i < data.current_map.objects.Count; i++)
            {
                if (data.current_map.objects[i].o == "TestSave")
                {
                    city_sequence = !data.current_map.objects[i].interacted;
                    break;
                }
            }
        }
        ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/Saves/" + (PlayerPrefs.GetInt("_active_save_file_") + 1).ToString() + "/ScreenCaptures/" + save_name + ".png");
        StartCoroutine(SaveSequence());
    }
}
