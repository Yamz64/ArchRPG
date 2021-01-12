using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum battleState {  START, PLAYER, ENEMY, WIN, LOSE }

public class BattleScript : MonoBehaviour
{
    /*
    Steps:
    Access party member data
    For each member:
	    Add name into the order
	    Define character as member (have actions/stats ready)
		    Could also grab player data directly and load character object in
	    Get sprite ready

    Get enemy data for stage (stage data as well)
    For each enemy:
	    Add name into the order
	    Define character as enemy (record stats and possible actions)
	    Get AI ready
	    Get Sprite ready

    For each member/enemy in the list:
	    Player:
		    Choose action (attack, defend, use item, etc.)
	    Enemy:
		    Have action chosen based on probability/circumstances (AI)

	    Record damage from attacks, skip characters who have health below 0

    Complementary Steps:
	    Reorder characters if a speed stat has been changed/ability used
	    Skip over characters whose health is < 0
     */

    //Use to determine state of the battle (turns, win/loss, etc.)
    public battleState state;

    //Main text to let player know state of battle
    public Text dialogue;

    //GameObjects to use as basis for battle characters
    public GameObject playerPrefab;
    public GameObject member1Prefab;   public GameObject member2Prefab;    public GameObject member3Prefab;
    public GameObject enemyPrefab;

    //Locations to spawn characters at
    public Transform playerStation;
    public Transform member1Station;    public Transform member2Station;    public Transform member3Station;
    public Transform enemyStation;

    //Units to use in battle
    unit playerUnit;
    unit member1Unit;    unit member2Unit;    unit member3Unit;
    unit enemyUnit;

    //Start the battle
    void Start()
    {
        state = battleState.START;
        StartCoroutine( setupBattle() );
    }

    //Create battle characters, set up HUD's, display text, and start player turn
    IEnumerator setupBattle()
    {
        //Create player unit
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGo.GetComponent<unit>();

        //Create enemy unit
        GameObject enemyGo = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGo.GetComponent<unit>();

        //Set up HUD's
        playerUnit.setHUD();
        enemyUnit.setHUD();

        //Create party member 2 if possible
        if (member1Prefab && member1Station)
        {
            GameObject member1Go = Instantiate(member1Prefab, member1Station);
            member1Unit = member1Go.GetComponent<unit>();
            member1Unit.setHUD();
        }

        //Create party member 3 if possible
        if (member2Prefab && member2Station)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Unit = member2Go.GetComponent<unit>();
            member2Unit.setHUD();
        }

        //Create party member 4 if possible
        if (member3Prefab && member3Station)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Unit = member3Go.GetComponent<unit>();
            member3Unit.setHUD();
        }

        //Display text to player, showing an enemy has appeared
        dialogue.text = "The " + enemyUnit.unitName + " appears.";

        //Start player turn
        yield return new WaitForSeconds(2f);
        state = battleState.PLAYER;
        playerTurn();
    }

    IEnumerator unitDeath(unit bot)
    {
        yield return new WaitForSeconds(2f);


    }

    //Player turn, display relevant text
    void playerTurn()    {  dialogue.text = "Player's Turn";   }

    //Deal damage to enemy, check if it is dead, and act accordingly (win battle or enemy turn)
    IEnumerator playerAttack()
    {
        dialogue.text = "Player is attacking";

        yield return new WaitForSeconds(1f);

        bool dead = enemyUnit.takeDamage(2);
        enemyUnit.setHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(1f);

        //If enemy is dead, battle is won
        if (dead)
        {
            state = battleState.WIN;
            battleEnd();
        }
        //If enemy lives, they attack
        else
        {
            state = battleState.ENEMY;
            StartCoroutine(enemyAttack());
        }
        yield return new WaitForSeconds(2f);
    }

    //Heal damage the player has taken
    IEnumerator healPlayer(int hel)
    {
        dialogue.text = "Player is healing damage";

        yield return new WaitForSeconds(1f);

        playerUnit.healDamage(hel);
        playerUnit.setHP(playerUnit.currentHP);

        state = battleState.ENEMY;
        StartCoroutine(enemyAttack());
        yield return new WaitForSeconds(2f);
    }

    //Skip player turn to move to the enemy turn
    IEnumerator skipTurn()
    {
        dialogue.text = "Player does nothing";

        yield return new WaitForSeconds(1f);

        state = battleState.ENEMY;
        StartCoroutine(enemyAttack());
        yield return new WaitForSeconds(1f);
    }

    //Deal damage to player, check if they're dead, and act accordingly (lose battle or player turn)
    IEnumerator enemyAttack()
    {
        dialogue.text = enemyUnit.unitName + " is attacking";

        yield return new WaitForSeconds(1f);

        bool dead = playerUnit.takeDamage(3);
        playerUnit.setHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        //If player is dead, lose battle
        if (dead)
        {
            state = battleState.LOSE;
            battleEnd();
        }
        //If player lives, they attack
        else
        {
            state = battleState.PLAYER;
            playerTurn();
        }
    }

    //Display relevant text based on who wins the battle
    void battleEnd()
    {
        if (state == battleState.WIN)
        {
            dialogue.text = "The " + enemyUnit.unitName + " has been defeated";
        }
        else if (state == battleState.LOSE)
        {
            dialogue.text = "You Died";
        }
    }

    //Player chooses to attack
    public void AttackButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(playerAttack());
    }

    //Player chooses to heal themself
    public void ItemButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(healPlayer(5));
    }

    public void SkipButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(skipTurn());
    }


}
