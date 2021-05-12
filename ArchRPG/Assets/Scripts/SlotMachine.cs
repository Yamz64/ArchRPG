using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachine : InteractableBaseClass
{
    //variable denotes slot data (index 0 is first slot, and index 2 is slot 3)
    /*SLOT VALUES
     * 0 - $
     * 1 - cherry
     * 2 - bomb
     * 3 - cone
     */
    private int[,] slot_data = new int[,] { { 0, 1, 2, 3 },
                                            { 0, 1, 2, 3 },
                                            { 0, 1, 2, 3 }
                                          };

    private PlayerData data;
    private PlayerDialogueBoxHandler player;
    private PauseMenuHandler pause;

    IEnumerator Battle()
    {
        yield return new WaitForEndOfFrame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().BattleTransitionDriver();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player").GetComponent<TransitionHandler>().transition_completed);
        SceneManager.LoadScene("BattleScene");
    }

    IEnumerator ApproachSlot()
    {
        //open text box and initialize writing variables
        player.OpenTextBox();
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        //first dialogue
        dialogue_queue.Add("Do you want to have a go at the slot machine for $20?");
        
        temp.name = "_NO_EFFECT_";
        temp_effect.effects.Add(new TextEffectClass(temp));
        effect_queue.Add(new EffectContainer(temp_effect));

        image_queue.Add(null);

        player.SetWriteQueue(dialogue_queue);
        player.SetEffectQueue(effect_queue);
        player.SetImageQueue(image_queue);
        player.WriteDriver();
        yield return new WaitForEndOfFrame();

        //wait until question is asked before opening the choice menu
        yield return new WaitUntil(() => player.GetWriteCount() == 0);
        yield return new WaitUntil(() => player.GetWriting() == false);
        pause.menu_mode = true;
        pause.menu_input = true;
        pause.pause_menu_protection = false;
        pause.OpenMenu(7);
        pause.ActivateCursor();
        pause.UpdateSaveMenu();
        pause.SetChoiceText("Yes", false);
        pause.SetChoiceText("No", true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

        //wait until the interact button is pressed close the menu and then write dialogue based on the selection
        yield return new WaitUntil(() => pause.menu_input == false);
        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
        bool choice = pause.GetChoice();

        pause.CloseAllMenus();
        yield return new WaitForEndOfFrame();
        //yes
        if (!choice)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;
            //check to see if the player has enough money
            if (data.GetMoney() > 20)
            {
                StartCoroutine(RollSlotsDialogue());
            }
            else
            {
                player.OpenTextBox();
                dialogue_queue.Clear();
                dialogue_queue.Add("You don't have enough money to spin the slot machine!");

                effect_queue.Clear();
                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                image_queue.Clear();
                image_queue.Add(null);

                player.SetWriteQueue(dialogue_queue);
                player.SetEffectQueue(effect_queue);
                player.SetImageQueue(image_queue);
                player.WriteDriver();
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = false;
        }
    }

    IEnumerator RollSlotsDialogue()
    {
        List<string> dialogue_queue = new List<string>();
        List<EffectContainer> effect_queue = new List<EffectContainer>();
        TextEffectClass temp = new TextEffectClass();
        EffectContainer temp_effect = new EffectContainer();
        List<string> image_queue = new List<string>();

        bool loop = true;
        while (loop)
        {
            player.OpenTextBox();
            dialogue_queue.Clear();
            effect_queue.Clear();
            temp_effect.effects.Clear();
            image_queue.Clear();

            RollSlots();
            data.SetMoney(data.GetMoney() - 20);
            dialogue_queue.Add("You spin the slots for $20!");
            dialogue_queue.Add("The slots come up as follows:");

            //add slot info
            AddSlotInfoToQueue(ref dialogue_queue, 0);
            AddSlotInfoToQueue(ref dialogue_queue, 1);
            AddSlotInfoToQueue(ref dialogue_queue, 2);

            int additional_lines = 0;
            bool cancel = false;
            switch (CheckForMatches())
            {
                case 0:
                    dialogue_queue.Add("You didn't win anything...");
                    additional_lines = 1;
                    break;
                case 1:
                    dialogue_queue.Add("A $ Triple!");
                    dialogue_queue.Add("You won $100!");
                    data.SetMoney(data.GetMoney() + 100);
                    additional_lines = 2;
                    break;
                case 2:
                    dialogue_queue.Add("A Cherry Triple!");
                    dialogue_queue.Add("You won $300!");
                    data.SetMoney(data.GetMoney() + 300);
                    additional_lines = 2;
                    break;
                case 3:
                    dialogue_queue.Add("You won a low cash prize!");
                    dialogue_queue.Add("You got your money back!");
                    data.SetMoney(data.GetMoney() + 20);
                    additional_lines = 2;
                    break;
                case 4:
                    dialogue_queue.Add("You won a high cash prize!");
                    dialogue_queue.Add("You won $50!");
                    data.SetMoney(data.GetMoney() + 50);
                    additional_lines = 2;
                    break;
                case 5:
                    dialogue_queue.Add("Oh no, you got Bomb Triple!");
                    dialogue_queue.Add("You lost $10!");
                    data.SetMoney(data.GetMoney() - 10);
                    additional_lines = 2;
                    break;
                case 6:
                    dialogue_queue.Add("You got a Traffic Cone Triple?");
                    dialogue_queue.Add("You won something weird.");
                    cancel = true;
                    additional_lines = 2;
                    break;
                case 7:
                    dialogue_queue.Add("You didn't win a normal prize...");
                    dialogue_queue.Add("But you did hit a diagonal!");
                    dialogue_queue.Add("You won $20!");
                    data.SetMoney(data.GetMoney() + 20);
                    additional_lines = 2;
                    break;
                default:
                    break;
            }

            //add effects and images to queue
            for(int i=0; i<additional_lines + 5; i++)
            {
                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                image_queue.Add(null);
            }

            //add question
            if (!cancel)
            {
                dialogue_queue.Add("Play again?");

                temp_effect.effects.Clear();
                temp.name = "_NO_EFFECT_";
                temp_effect.effects.Add(new TextEffectClass(temp));
                effect_queue.Add(new EffectContainer(temp_effect));

                image_queue.Add(null);

                player.SetWriteQueue(dialogue_queue);
                player.SetEffectQueue(effect_queue);
                player.SetImageQueue(image_queue);
                player.WriteDriver();

                yield return new WaitForEndOfFrame();

                //wait until question is asked before opening the choice menu
                yield return new WaitUntil(() => player.GetWriteCount() == 0);
                yield return new WaitUntil(() => player.GetWriting() == false);
                pause.menu_mode = true;
                pause.menu_input = true;
                pause.pause_menu_protection = false;
                pause.OpenMenu(7);
                pause.ActivateCursor();
                pause.UpdateSaveMenu();
                pause.SetChoiceText("Yes", false);
                pause.SetChoiceText("No", true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                //wait until the interact button is pressed close the menu and then write dialogue based on the selection
                yield return new WaitUntil(() => pause.menu_input == false);
                yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
                bool choice = pause.GetChoice();

                //no break the loop
                if (choice)
                {
                    loop = false;
                    pause.CloseAllMenus();
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    pause.CloseAllMenus();
                    yield return new WaitForEndOfFrame();
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = true;

                    //check to see if the player has enough money before looping again
                    if (data.GetMoney() < 20)
                    {
                        loop = false;
                        dialogue_queue.Clear();
                        effect_queue.Clear();
                        image_queue.Clear();

                        player.OpenTextBox();
                        dialogue_queue.Add("You don't have enough money to spin the slot machine!");

                        temp_effect.effects.Clear();
                        temp.name = "_NO_EFFECT_";
                        temp_effect.effects.Add(new TextEffectClass(temp));
                        effect_queue.Add(new EffectContainer(temp_effect));

                        image_queue.Add(null);

                        player.SetWriteQueue(dialogue_queue);
                        player.SetEffectQueue(effect_queue);
                        player.SetImageQueue(image_queue);
                        player.WriteDriver();

                        yield return new WaitForEndOfFrame();
                        yield return new WaitUntil(() => player.GetActive() == false);
                    }
                }
            }
            //if cancel roll a fight
            else
            {
                player.SetWriteQueue(dialogue_queue);
                player.SetEffectQueue(effect_queue);
                player.SetImageQueue(image_queue);
                player.WriteDriver();

                yield return new WaitForEndOfFrame();
                yield return new WaitUntil(() => player.GetActive() == false);

                loop = false;

                string[] enemies = new string[] { "Killer Cone", "Killer Cone", "Killer Cone", "Killer Cone" };
                //1/4 chance to spawn a conniving cone
                int conniving_chance = Random.Range(0, 4);
                if (conniving_chance == 0) enemies[0] = "Conniving Cone";

                int number_of_enemies = Random.Range(0, 4);

                CharacterStatJsonConverter save = new CharacterStatJsonConverter(data);
                switch (number_of_enemies)
                {
                    case 0:
                        save.SaveEnemyNames(enemies[0]);
                        break;
                    case 1:
                        save.SaveEnemyNames(enemies[0], enemies[1]);
                        break;
                    case 2:
                        save.SaveEnemyNames(enemies[0], enemies[1], enemies[2]);
                        break;
                    case 3:
                        save.SaveEnemyNames(enemies[0], enemies[1], enemies[2], enemies[3]);
                        break;
                }

                //start battle transition and load into the battle
                save.active_scene = SceneManager.GetActiveScene().name;
                save.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                save.Save(PlayerPrefs.GetInt("_active_save_file_"));
                StartCoroutine(Battle());
            }
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().interaction_protection = false;
    }

    public void AddSlotInfoToQueue(ref List<string> queue, int slot)
    {
        Mathf.Clamp(slot, 0, 2);
        switch (slot_data[slot, 2])
        {
            case 0:
                queue.Add("$!");
                break;
            case 1:
                queue.Add("Cherry!");
                break;
            case 2:
                queue.Add("Bomb!");
                break;
            case 3:
                queue.Add("Cone!");
                break;
            default:
                break;
        }

    }

    //function for rolling slots
    private void RollSlots()
    {
        //loop through slots
        for(int i=0; i<slot_data.GetLength(0); i++)
        {
            //determine the number of spins per slot
            int spin_amount = Random.Range(10, 51);
            //Debug.Log(string.Format("Spun {0} row by {1} times.", i, spin_amount));

            //shift the slot by spin amount
            int[] temp = new int[4];
            for(int j=0; j<4; j++)
            {
                int swap_with = (j + spin_amount) % 4;
                temp[swap_with] = slot_data[i, j];
            }
            for(int j=0; j<4; j++) { slot_data[i, j] = temp[j]; }
        }
        
        //output the results
        Debug.Log(string.Format("Slots:\t[{0}, {1}, {2}]\n\t[{3}, {4}, {5}]\n\t[{6}, {7}, {8}]\n\t[{9}, {10}, {11}]", 
            slot_data[0, 0], slot_data[1, 0], slot_data[2, 0], 
            slot_data[0, 1], slot_data[1, 1], slot_data[2, 1], 
            slot_data[0, 2], slot_data[1, 2], slot_data[2, 2],
            slot_data[0, 3], slot_data[1, 3], slot_data[2, 3]));
        
    }

    //checks slot machine for matches and returns an integer code for what to do next
    /*
     * 0 = no match
     * 1 = $ match
     * 2 = cherry match
     * 3 = $ cherry $
     * 4 = cherry $ cherry
     * 5 = bomb
     * 6 = traffic cone
     * 7 = diagonal
     */
    private int CheckForMatches()
    {
        //convert int code to string code
        string result = slot_data[0, 2].ToString() + slot_data[1, 2].ToString() + slot_data[2, 2].ToString();

        //check for immediate match
        switch (result)
        {
            case "000":
                return 1;
            case "111":
                return 2;
            case "010":
                return 3;
            case "101":
                return 4;
            case "222":
                return 5;
            case "333":
                return 6;
            default:
                break;
        }

        //if there is no immediate match then check for diagonals
        if ((slot_data[0, 1] == slot_data[1, 2] && slot_data[1, 2] == slot_data[2, 3]) || (slot_data[2, 1] == slot_data[1, 2] && slot_data[1, 2] == slot_data[0, 3]))
            return 7;
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDataMono>().data;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogueBoxHandler>();
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<PauseMenuHandler>();
    }

    public override void Interact()
    {
        StartCoroutine(ApproachSlot());
    }
}
