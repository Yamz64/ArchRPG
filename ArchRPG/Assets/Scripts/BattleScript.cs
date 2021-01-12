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

    public battleState state;

    public Text dialogue;

    public GameObject playerPrefab;
    public GameObject member1Prefab;
    public GameObject member2Prefab;
    public GameObject member3Prefab;
    public GameObject enemyPrefab;

    public Transform playerStation;
    public Transform member1Station;
    public Transform member2Station;
    public Transform member3Station;
    public Transform enemyStation;

    unit playerUnit;
    unit member1Unit;
    unit member2Unit;
    unit member3Unit;
    unit enemyUnit;

    void Start()
    {
        state = battleState.START;
        StartCoroutine( setupBattle() );
    }

    IEnumerator setupBattle()
    {
        GameObject playerGo = Instantiate(playerPrefab, playerStation);
        playerUnit = playerGo.GetComponent<unit>();

        GameObject enemyGo = Instantiate(enemyPrefab, enemyStation);
        enemyUnit = enemyGo.GetComponent<unit>();

        playerUnit.setHUD();
        enemyUnit.setHUD();

        if (member1Prefab && member1Station)
        {
            GameObject member1Go = Instantiate(member1Prefab, member1Station);
            member1Unit = member1Go.GetComponent<unit>();
            member1Unit.setHUD();
        }

        if (member2Prefab && member2Station)
        {
            GameObject member2Go = Instantiate(member2Prefab, member2Station);
            member2Unit = member2Go.GetComponent<unit>();
            member2Unit.setHUD();
        }

        if (member3Prefab && member3Station)
        {
            GameObject member3Go = Instantiate(member3Prefab, member3Station);
            member3Unit = member3Go.GetComponent<unit>();
            member3Unit.setHUD();
        }

        dialogue.text = "The " + enemyUnit.unitName + " appears.";

        yield return new WaitForSeconds(2f);
        state = battleState.PLAYER;
        playerTurn();
    }

    void playerTurn()
    {
        dialogue.text = "Player's Turn";
    }

    IEnumerator playerAttack()
    {
        dialogue.text = "Player is attacking";
        bool dead = enemyUnit.takeDamage(5);

        enemyUnit.setHUD();

        if (dead)
        {
            state = battleState.WIN;
            battleEnd();
        }
        else
        {
            state = battleState.ENEMY;
            StartCoroutine(enemyAttack());
        }
        yield return new WaitForSeconds(2f);
    }

    IEnumerator enemyAttack()
    {
        dialogue.text = enemyUnit.unitName + " is attacking";

        yield return new WaitForSeconds(1f);

        bool dead = playerUnit.takeDamage(3);
        playerUnit.setHUD();

        if (dead)
        {
            state = battleState.LOSE;
            battleEnd();
        }
        else
        {
            state = battleState.PLAYER;
            playerTurn();
        }
    }

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

    public void AttackButton()
    {
        if (state != battleState.PLAYER) return;
        StartCoroutine(playerAttack());
    }


}
